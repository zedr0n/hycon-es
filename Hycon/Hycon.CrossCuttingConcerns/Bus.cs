using System;
using System.Threading.Tasks;
using Hycon.Infrastructure.Bus;
using Hycon.Infrastructure.Logging;
using Hycon.Interfaces.Domain;
using SimpleInjector;

namespace Hycon.CrossCuttingConcerns
{
    public class Bus : IBus
    {
        private readonly Container _container;
        private readonly Func<Type, object> _activator;
        private readonly ILog _log;

        private object GetInstance(Type type)
        {
            try
            {
                var instance = _container.GetInstance(type);
                return instance;
            }
            catch (Exception e)
            {
                _log.WriteLine("Failed to create handler " + type.Name );
                if (e is ActivationException)
                    return null;
                throw;
            }
            
        }
        
        public Bus(Container container, ILog log)
        {
            _container = container;
            _log = log;
        }

        public bool Command(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = GetInstance(handlerType);
            if (handler == null)
                return false;
            
            handler.Handle(command as dynamic);
            return true;
        }

        public async Task CommandAsync(ICommand command)
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = GetInstance(handlerType);
            if (handler != null)
                await handler.Handle(command as dynamic); 
        }

    }
}
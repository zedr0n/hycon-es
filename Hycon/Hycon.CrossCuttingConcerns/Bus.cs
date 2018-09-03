using System;
using System.Threading.Tasks;
using Hycon.Infrastructure.Logging;
using Hycon.Infrastructure.Pipes;
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

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = GetInstance(handlerType);
            if (handler != null)
            {
                var task = handler.Handle(query as dynamic) as Task<TResult>;
                if (task != null && task.IsCompleted)
                    return task.Result;
            }
            return default(TResult);            
        }
        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = GetInstance(handlerType);
            if (handler != null)
                return await handler.Handle(query as dynamic);
            return default(TResult);            
        }

    }
}
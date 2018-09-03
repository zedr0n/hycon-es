using System.Linq;
using Hycon.Core.Blocks.Commands;
using Hycon.EventStore.Redis;
using Hycon.Infrastructure.Bus;
using Hycon.Infrastructure.Clock;
using Hycon.Infrastructure.Domain;
using Hycon.Infrastructure.Logging;
using Hycon.Infrastructure.Streams;
using Hycon.Interfaces.Domain;
using Hycon.Interfaces.EventStore;
using NodaTime;
using SimpleInjector;

namespace Hycon.CrossCuttingConcerns
{
    public class CompositionRoot 
    {
        public virtual void ComposeApplication(Container container)
        {
            // register infrastructure
            container.Register<ILog, ConsoleLog>(Lifestyle.Singleton);
            container.Register<IClock,HighPrecisionClock>(Lifestyle.Singleton);
            
            container.Register<IStreamLocator, StreamLocator>();
            container.Register<IEventStore,RedisEventStore>();
            container.Register<IDomainRepository,DomainRepository>(Lifestyle.Singleton);
            
            container.Register(typeof(ICommandHandler<>),new[] {
                typeof(CreateBlockCommandHandler),
                typeof(PutBlockCommandHandler)
            } ,Lifestyle.Singleton);
            container.RegisterDecorator(typeof(ICommandHandler<>),typeof(CommandRecorder<>),Lifestyle.Singleton, context =>
                !context.AppliedDecorators.Any(d => d.IsClosedTypeOf(typeof(CommandRecorder<>)))); 
            
            container.Register<IBus,Bus>(Lifestyle.Singleton);
            
        }
    }
}
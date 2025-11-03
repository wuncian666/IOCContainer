using System;

namespace IOCContainer
{
    public class ServiceDescriptor
    {
        public ServiceLifetime LifeTime { get; set; }

        public Type ServiceType { get; set; }

        public Type ImplementationType { get; set; }

        public Func<IServiceProvider, object> ImplementationFactory { get; set; }

        public ServiceDescriptor()
        { }

        public ServiceDescriptor(
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationFactory = implementationFactory;
            LifeTime = lifetime;
        }

        public ServiceDescriptor(
            Type serviceType,
            Type implementationType,
            ServiceLifetime type)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeTime = type;
        }

        public bool IsSingleton()
        {
            return LifeTime == ServiceLifetime.SINGLETON;
        }
    }
}
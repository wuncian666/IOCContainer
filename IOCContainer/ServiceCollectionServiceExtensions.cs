using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services)
        {
            return services.AddTransient(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, Type implementationType)
        {
            return Add(services, serviceType, implementationType, ServiceLifetime.TRANSIENT);
        }

        public static IServiceCollection AddTransient(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            return Add(services, serviceType, implementationFactory, ServiceLifetime.TRANSIENT);
        }

        public static IServiceCollection AddTransient(
            this IServiceCollection services,
            Type serviceType)
        {
            return services.AddTransient(serviceType, serviceType);
        }

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, Func<IServiceProvider, object> implementationFactory)
        {
            return services.AddTransient(typeof(TService), implementationFactory);
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType)
        {
            return Add(services, serviceType, implementationType, ServiceLifetime.SINGLETON);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services)
        {
            return services.AddSingleton(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            return Add(services, serviceType, implementationFactory, ServiceLifetime.SINGLETON);
        }

        private static IServiceCollection Add(
            IServiceCollection collection,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            collection.Add(descriptor);
            return collection;
        }

        private static IServiceCollection Add(
            IServiceCollection collection,
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationFactory, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IOCContainer
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _classMap = new Dictionary<Type, List<ServiceDescriptor>>();

        public ServiceDescriptor this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        IServiceProvider IServiceCollection.BuildServiceProvider()
        {
            return new ServiceProvider(this._classMap);
        }

        //public void AddSingleton<TIn, TOut>()
        //{
        //    var serviceType = typeof(TIn);
        //    var implementType = typeof(TOut);
        //    ServiceDescriptor typeWithClassName = new ServiceDescriptor(ServiceLifetime.SINGLETON, serviceType, implementType);
        //    Boolean hasClass = classMap.TryGetValue(serviceType.FullName, out List<ServiceDescriptor> classList);
        //    if (hasClass)
        //    {
        //        classList.Add(typeWithClassName);
        //    }
        //    else
        //    {
        //        List<ServiceDescriptor> typeWithClassNames =
        //            new List<ServiceDescriptor>() { typeWithClassName };
        //        classMap.Add(serviceType.FullName, typeWithClassNames);
        //    }
        //}

        //public void AddTransient<TIn, TOut>()
        //{
        //    var serviceType = typeof(TIn);
        //    var implementType = typeof(TOut);
        //    var typeWithClassName = new ServiceDescriptor(ServiceLifetime.TRANSIENT, serviceType, implementType);
        //    Boolean hasClass = classMap.TryGetValue(serviceType.FullName, out List<ServiceDescriptor> classList);
        //    if (hasClass)
        //    {
        //        classList.Add(typeWithClassName);
        //    }
        //    else
        //    {
        //        List<ServiceDescriptor> typeWithClassNames =
        //            new List<ServiceDescriptor>() { typeWithClassName };
        //        classMap.Add(serviceType.FullName, typeWithClassNames);
        //    }
        //}

        //public void AddTransient<T>(Func<ServiceCollection, object> implementationFactory)
        //{
        //    var serviceType = typeof(T);
        //    var serviceDescriptor = new ServiceDescriptor(ServiceLifetime.TRANSIENT, serviceType, null);
        //    serviceDescriptor.ImplementationFactory = implementationFactory;
        //    Boolean hasClass = classMap.TryGetValue(serviceType.FullName, out List<ServiceDescriptor> classList);
        //    if (hasClass)
        //    {
        //        classList.Add(serviceDescriptor);
        //    }
        //    else
        //    {
        //        List<ServiceDescriptor> typeWithClassNames =
        //            new List<ServiceDescriptor>() { serviceDescriptor };
        //        classMap.Add(serviceType.FullName, typeWithClassNames);
        //    }
        //}

        public IServiceCollection AddTransient(Type serviceType, Type implementationType)
        {
            return Add(this, serviceType, implementationType, ServiceLifetime.TRANSIENT);
        }

        public IServiceCollection AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            return Add(this, serviceType, implementationFactory, ServiceLifetime.TRANSIENT);
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

        public void Add(ServiceDescriptor item)
        {
            bool isExist = _classMap.TryGetValue(item.ServiceType, out var descriptors);
            if (!isExist)
            {
                descriptors = new List<ServiceDescriptor>();
                _classMap.Add(item.ServiceType, descriptors);
            }
            descriptors.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ServiceDescriptor item)
        {
            _classMap.TryGetValue(item.ServiceType, out var descriptors);
            return descriptors.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            foreach (var item in _classMap.Values.SelectMany(x => x))
            {
                yield return item;
            }
        }

        public int IndexOf(ServiceDescriptor item)
        {
            _classMap.TryGetValue(item.ServiceType, out var descriptors);
            return descriptors.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _classMap.TryGetValue(item.ServiceType, out var descriptors);
            descriptors.Insert(index, item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            _classMap.TryGetValue(item.ServiceType, out var descriptors);
            return descriptors.Remove(item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
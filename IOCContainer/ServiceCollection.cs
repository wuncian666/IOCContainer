using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace IOCContainer
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> _items = new List<ServiceDescriptor>();

        private readonly Dictionary<Type, List<ServiceDescriptor>> _classMap = new Dictionary<Type, List<ServiceDescriptor>>();

        public ServiceDescriptor this[int index]
        {
            get => _items[index];
            set
            {
                var old = _items[index];

                if (_classMap.TryGetValue(old.ServiceType, out var oldList))
                {
                    oldList.Remove(old);
                    if (oldList.Count == 0) _classMap.Remove(old.ServiceType);
                }

                _items[index] = value;

                if (!_classMap.TryGetValue(value.ServiceType, out var newList))
                {
                    newList = new List<ServiceDescriptor>();
                    _classMap.Add(value.ServiceType, newList);
                }
                newList.Add(value);
            }
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        ServiceDescriptor IList<ServiceDescriptor>.this[int index]
        {
            get => this[index];
            set => this[index] = value;
        }

        public IServiceProvider BuildServiceProvider()
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
            return Add(this, serviceType, implementationType, ServiceLifetime.Transient);
        }

        public IServiceCollection AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            return Add(this, serviceType, implementationFactory, ServiceLifetime.Transient);
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
            _items.Add(item);

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
            _items.Clear();
            _classMap.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            if (item == null) return false;
            return _items.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(ServiceDescriptor item)
        {
            if (item == null) return -1;
            return _items.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            _items.Insert(index, item);

            if (!_classMap.TryGetValue(item.ServiceType, out var list))
            {
                list = new List<ServiceDescriptor>();
                _classMap.Add(item.ServiceType, list);
            }
            list.Add(item);
        }

        public bool Remove(ServiceDescriptor item)
        {
            if (item == null) return false;

            var removed = _items.Remove(item);
            if (removed && _classMap.TryGetValue(item.ServiceType, out var list))
            {
                list.Remove(item);
                if (list.Count == 0) _classMap.Remove(item.ServiceType);
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            var item = _items[index];
            _items.RemoveAt(index);

            if (_classMap.TryGetValue(item.ServiceType, out var list))
            {
                list.Remove(item);
                if (list.Count == 0) _classMap.Remove(item.ServiceType);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
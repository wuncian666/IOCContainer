using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IOCContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _classMap;

        private readonly Dictionary<ServiceDescriptor, object> _singletonInstances = new Dictionary<ServiceDescriptor, object>();

        public ServiceProvider(Dictionary<Type, List<ServiceDescriptor>> classMap)
        {
            this._classMap = classMap;
        }

        //GetService => 給對外呼叫的，用來取得某一個類別，例如: provider.GetService<ILogger<Student>>()
        //判斷該類型是否需要進一步拆解: ILogger(泛型判斷), IEnumerable<Student> => 用來判斷是 一般類型/泛型類型/集合類型

        //GetImplementationInstance(ServiceDesiptor) => 判斷該類型是哪一種 ServiceType(Signleton/Transient/ImplementationFactory)

        //CreateInstance => 從 Desiptor中找到 ImplementaionType來創建，創建的時候需要找尋建構元，從建構元中依序將服務給創建出來
        //=> 如果建構元中有其他服務，反覆呼叫GetService 形成一個遞迴,直到最後是空的建構元時，才呼叫 Activator.CreateInstance()

        public T GetService<T>()
        {
            Type type = typeof(T);
            T instance = (T)GetService(type);
            return instance;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsGenericType)
            {
                this.GetGenericService(serviceType);
            }

            // 不是泛型
            if (_classMap.TryGetValue(serviceType, out List<ServiceDescriptor> descriptors) &&
                descriptors.Count > 0)
            {
                return this.GetImplementationInstance(descriptors.Last());
            }

            return null;
        }

        private object GetGenericService(Type serviceType)
        {
            var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
            var genericArgs = serviceType.GetGenericArguments();

            if (genericTypeDefinition == typeof(IEnumerable<>))
            {
                return this.GetIEnumerableService(genericArgs);
            }

            // 一般泛型 open generic <>
            if (_classMap.TryGetValue(genericTypeDefinition, out List<ServiceDescriptor> descriptors) &&
                descriptors.Count > 0)
            {
                var baseDescriptor = descriptors.Last();
                if (baseDescriptor.ImplementationType == null)
                {
                    return this.GetImplementationInstance(baseDescriptor);
                }

                var implementationType = baseDescriptor.ImplementationType.IsGenericTypeDefinition
                    ? baseDescriptor.ImplementationType.MakeGenericType(genericArgs)//ILogger<ILogger<>>
                    : baseDescriptor.ImplementationType;// Logger<Form1>

                var descriptor = new ServiceDescriptor(serviceType, implementationType, baseDescriptor.Lifetime);
                return this.GetImplementationInstance(descriptor);
            }

            // ILogger<student>
            // 找不到 open generic 改找關閉型別
            // <student>
            if (_classMap.TryGetValue(serviceType, out descriptors) && descriptors.Count > 0)
            {
                return this.GetImplementationInstance(descriptors.Last());
            }
            return null;
        }

        private object GetIEnumerableService(Type[] genericArgs)
        {
            // args = IConfigOptions 的時候取得的 descriptors 的 implement type = null
            var elementType = genericArgs[0];
            // 製作 List<> 因為與 IEnumerable<> 互通
            var listType = typeof(List<>).MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(listType);

            // 回傳空陣列
            if (!_classMap.TryGetValue(elementType, out List<ServiceDescriptor> descriptors) || descriptors.Count == 0)
                return list;

            foreach (var item in descriptors)
            {
                var itemInstance = this.GetImplementationInstance(item);
                list.Add(itemInstance);
            }
            return list;
        }

        private object GetImplementationInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationFactory != null)
            {
                return this.GetImplementFactoryInstance(descriptor);
            }

            if (descriptor.ImplementationType == null)
            {
                Console.WriteLine($"Service Descriptor for {descriptor.ServiceType} 缺少");
            }
            return this.GetImplementTypeInstance(descriptor);
        }

        private object GetImplementFactoryInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (_singletonInstances.TryGetValue(descriptor, out var cached))
                {
                    return cached;
                }

                var createdOnce = descriptor.ImplementationFactory(this);
                _singletonInstances[descriptor] = createdOnce;
                return createdOnce;
            }

            return descriptor.ImplementationFactory(this);
        }

        private object GetImplementTypeInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (_singletonInstances.TryGetValue(descriptor, out var existEntity))
                {
                    return existEntity;
                }
                var instance = this.CreateInstance(descriptor.ImplementationType);
                _singletonInstances[descriptor] = instance;

                return instance;
            }
            return this.CreateInstance(descriptor.ImplementationType);
        }

        private object CreateInstance(Type type)
        {
            var constructors = type.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length);

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 0)
                {
                    return Activator.CreateInstance(type);
                }

                var instances = new object[parameters.Length];
                bool allParametersMatch = false;

                for (int i = 0; i < parameters.Length; i++)
                {
                    // IEnumerable<IProvider>
                    //var methodInfo = typeof(ServiceProvider).GetMethod("GetService", new Type[] { typeof(Type) });
                    //var instance = methodInfo.Invoke(this, new object[] { t });

                    var instance = this.GetService(parameters[i].ParameterType);
                    if (instance == null)
                    {
                        allParametersMatch = false;
                        break;// 嘗試下一個建構式
                    }

                    instances[i] = instance;
                }

                if (allParametersMatch)
                {
                    return Activator.CreateInstance(type, instances.ToArray());
                }
            }

            // 為能滿足所有建構元
            return null;
        }
    }
}
}
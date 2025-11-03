using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IOCContainer
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _classMap;

        private readonly Dictionary<Type, object> _existEntityMap = new Dictionary<Type, object>();

        public ServiceProvider(Dictionary<Type, List<ServiceDescriptor>> classMap)
        {
            this._classMap = classMap;
        }

        //GetService => 給對外呼叫的，用來取得某一個類別，例如: provider.GetService<ILogger<Student>>()
        //判斷該類型是否需要進一步拆解: ILogger(泛型判斷), IEnumerable<Student> => 用來判斷是 一般類型/泛型類型/集合類型

        //GetImplementationInstance(ServiceDesiptor) => 判斷該類型是哪一種 ServiceType(Signleton/Transient/ImplementationFactory)

        //CreateInstance => 從 Desiptor中找到 ImplementaionType來創建，創建的時候需要找尋建構元，從建構元中依序將服務給創建出來
        //=> 如果建構元中有其他服務，反覆呼叫GetService 形成一個遞迴,直到最後是空的建構元時，才呼叫 Activator.CreateInstance()

        public object GetImplementationInstance(ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(this);
            }

            if (descriptor.IsSingleton())
            {
                if (_existEntityMap.TryGetValue(descriptor.ServiceType, out var existEntity))
                {
                    return existEntity;
                }
                else
                {
                    var methodInfo = typeof(ServiceProvider).GetMethod("CreateInstance", BindingFlags.NonPublic | BindingFlags.Instance);
                    object[] parameters = { descriptor.ImplementationType };
                    var instance = methodInfo.Invoke(this, parameters);

                    _existEntityMap.Add(descriptor.ServiceType, instance);
                    return instance;
                }
            }

            return this.CreateInstance(descriptor.ImplementationType);
        }

        // 1. ILogger<Student> -> SerialLog<Student>
        public object GetService(Type serviceType)
        {
            // 判斷泛型
            if (serviceType.IsGenericType)
            {
                Type serviceTypeDefinition = serviceType.GetGenericTypeDefinition();// ILogger<>
                var serviceTypeGenericArguments = serviceType.GetGenericArguments();// Student
                _classMap.TryGetValue(serviceTypeDefinition, out var genDescriptors);// ILogger<> find SerialLog<>
                var genDescriptor = genDescriptors.Last();// SerialLog 的 Descriptor

                var completeType = genDescriptor.ImplementationType.MakeGenericType(serviceTypeGenericArguments);// SerialLog<Student>
                // SerialLog 的 Descriptor (SerialLog, SerialLog<Student>, Lifetime)
                var childDescriptor = new ServiceDescriptor(genDescriptor.ServiceType, completeType, genDescriptor.LifeTime);
                _classMap.TryGetValue(genDescriptor.ServiceType, out var serviceDescriptors);
                if (serviceDescriptors == null)
                {
                    serviceDescriptors = new List<ServiceDescriptor>();
                }
                serviceDescriptors.Add(childDescriptor);

                return this.GetImplementationInstance(childDescriptor);// create SerialLog instance
            }

            _classMap.TryGetValue(serviceType, out var descriptors);
            if (descriptors == null || descriptors.Count == 0) return null;
            ServiceDescriptor descriptor = descriptors.Last();

            return this.GetImplementationInstance(descriptor);
        }

        public T GetService<T>()
        {
            Type type = typeof(T);
            T instance = (T)GetService(type);

            return instance;
        }

        private object CreateInstance(Type type) //SerialLog<Student>
        {
            ConstructorInfo constructorInfo = type.GetConstructors().Last();
            var parameters = constructorInfo.GetParameters();
            if (parameters.Length == 0)
            {
                return Activator.CreateInstance(type);
            }

            List<object> instances = new List<object>();
            foreach (ParameterInfo parameter in parameters)
            {
                Type t = parameter.ParameterType;
                var methodInfo = typeof(ServiceProvider).GetMethod("GetService", new Type[] { typeof(Type) });
                var instance = methodInfo.Invoke(this, new object[] { t });
                instances.Add(instance);
            }
            return Activator.CreateInstance(type, instances.ToArray());
        }
    }
}
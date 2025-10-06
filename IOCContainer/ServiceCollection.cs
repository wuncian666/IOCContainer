using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class ServiceCollection
    {
        private readonly Dictionary<string, List<TypeWithClassName>> classMap = new Dictionary<string, List<TypeWithClassName>>();

        private readonly Dictionary<string, object> existEntityMap = new Dictionary<string, object>();

        public void RegisteSingletionClass<TIn, TOut>()
        {
            var inClassName = typeof(TIn).FullName;
            var outClassName = typeof(TOut).FullName;
            TypeWithClassName typeWithClassName = new TypeWithClassName(CollectionEnum.SINGLETON, outClassName);
            Boolean hasClass = classMap.TryGetValue(inClassName, out List<TypeWithClassName> classList);
            if (hasClass)
            {
                classList.Add(typeWithClassName);
            }
            else
            {
                List<TypeWithClassName> typeWithClassNames =
                    new List<TypeWithClassName>() { typeWithClassName };
                classMap.Add(inClassName, typeWithClassNames);
            }
        }

        public void RegisteTransientClass<TIn, TOut>()
        {
            var inClassName = typeof(TIn).FullName;
            var outClassName = typeof(TOut).FullName;
            var typeWithClassName = new TypeWithClassName(CollectionEnum.TRANSIENT, outClassName);
            Boolean hasClass = classMap.TryGetValue(inClassName, out List<TypeWithClassName> classList);
            if (hasClass)
            {
                classList.Add(typeWithClassName);
            }
            else
            {
                List<TypeWithClassName> typeWithClassNames =
                    new List<TypeWithClassName>() { typeWithClassName };
                classMap.Add(inClassName, typeWithClassNames);
            }
        }

        // form
        public T GetService<T>()
        {
            string name = typeof(T).FullName;
            classMap.TryGetValue(name, out List<TypeWithClassName> outNames);
            TypeWithClassName outName = outNames.Last();

            if (outName.IsSingleton())
            {
                if (existEntityMap.TryGetValue(name, out var existEntity))
                {
                    return (T)existEntity;
                }
                else
                {
                    T newEntity = (T)this.CreateInstance<T>();
                    existEntityMap.Add(name, newEntity);
                    return newEntity;
                }
            }

            return (T)this.CreateInstance<T>();
        }

        private T CreateInstance<T>()
        {
            Type t = typeof(T);
            ConstructorInfo constructorInfo = t.GetConstructors().Last();
            var parameters = constructorInfo.GetParameters();
            if (parameters.Length == 0)
            {
                return (T)Activator.CreateInstance(t);
            }

            List<object> instances = new List<object>();
            foreach (ParameterInfo parameter in parameters)
            {
                Type type = parameter.ParameterType;
                var methodInfo = typeof(ServiceCollection).GetMethod("GetService", BindingFlags.Public | BindingFlags.Instance);
                var genericMethod = methodInfo.MakeGenericMethod(type);
                var instance = genericMethod.Invoke(this, null);
                instances.Add(instance);
            }
            return (T)Activator.CreateInstance(t, instances.ToArray());
        }
    }
}
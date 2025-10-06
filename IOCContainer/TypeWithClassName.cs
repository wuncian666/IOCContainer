using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class TypeWithClassName
    {
        public CollectionEnum CollectionEnum { get; set; }

        public string ClassName { get; set; }

        public TypeWithClassName(CollectionEnum type, string name)
        {
            CollectionEnum = type;
            ClassName = name;
        }

        public bool IsSingleton()
        {
            return CollectionEnum == CollectionEnum.SINGLETON;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class Tesla : Car
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public void PrintName()
        {
            Console.WriteLine(Name);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    internal class Student
    {
        public string Name
        { get { return this.name; } set { this.name = value; } }

        private string name = "old student";
    }
}
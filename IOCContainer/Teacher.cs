using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    internal class Teacher
    {
        public string Name { get; set; }

        public Student Student { get; set; }

        public School School { get; set; }

        public Teacher(Student student, School school)
        {
            this.Student = student;
            this.School = school;
        }
    }
}
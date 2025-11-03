using System;

namespace IOCContainer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            //services.RegisteTransientClass<十二生肖, 十二生肖>();
            //services.RegisteTransientClass<鼠, 鼠>();
            //services.RegisteSingletionClass<牛, 牛>();
            //services.RegisteSingletionClass<虎, 虎>();
            //services.RegisteTransientClass<兔, 兔>();
            //services.RegisteTransientClass<龍, 龍>();
            //services.RegisteSingletionClass<蛇, 蛇>();
            //services.RegisteSingletionClass<馬, 馬>();
            //services.RegisteSingletionClass<羊, 羊>();
            //services.RegisteSingletionClass<猴, 猴>();
            //services.RegisteSingletionClass<雞, 雞>();
            //services.RegisteTransientClass<狗, 狗>();
            //services.RegisteSingletionClass<豬, 豬>();

            //十二生肖 十二生肖實體 = services.GetService<十二生肖>();
            //十二生肖實體.Show();
            //龍 龍 = services.GetService<龍>();
            //龍.Str = "+++++";
            //十二生肖 second = services.GetService<十二生肖>();
            //second.Show();

            //services.RegisteSingletionClass<Car, Toyota>();
            //services.RegisteTransientClass<Car, Tesla>();
            //Car tesla = services.GetService<Car>();
            //tesla.PrintName();
            //tesla.Name = "tesla";
            //tesla.PrintName();
            //Car tesla1 = services.GetService<Car>();
            //tesla1.PrintName();

            services.AddTransient<Student, Student>();
            services.AddTransient<School, School>();
            services.AddSingleton<Teacher, Teacher>();

            var provider = services.BuildServiceProvider();
            var teacher = provider.GetService<Teacher>();
            var student = provider.GetService<Student>();
            var school = provider.GetService<School>();

            student.Name = "new student";
            Console.WriteLine($"{student.Name}");
            school.Name = "new school";
            Console.WriteLine(school.Name);

            Console.WriteLine($"{teacher.Student.Name} and {teacher.School.Name}");

            teacher.Student.Name = "new new student";
            Console.WriteLine($"{teacher.Student.Name} and {teacher.School.Name}");

            var secondSchool = provider.GetService<School>();
            Console.WriteLine(secondSchool.Name);

            var secondTeacher = provider.GetService<Teacher>();
            Console.WriteLine($"{secondTeacher.Student.Name} and {secondTeacher.School.Name}");

            Console.ReadKey();
        }
    }
}
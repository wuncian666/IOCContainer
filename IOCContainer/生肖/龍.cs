using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 龍
    {
        public static string Str { get; set; }

        private 蛇 蛇;

        public 龍(蛇 蛇)
        {
            this.蛇 = 蛇;
        }

        public void ShowInfo()
        {
            if (Str == null)
            {
                Console.WriteLine("我是龍");
            }
            else
            {
                Console.WriteLine($"我是{Str}");
            }
            this.蛇.ShowInfo();
        }
    }
}
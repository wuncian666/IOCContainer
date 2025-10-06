using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 蛇
    {
        private 馬 馬;

        public 蛇(馬 馬)
        {
            this.馬 = 馬;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是蛇");
            this.馬.ShowInfo();
        }
    }
}
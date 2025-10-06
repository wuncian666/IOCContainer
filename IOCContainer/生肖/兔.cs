using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 兔
    {
        private 龍 龍;

        public 兔(龍 龍)
        {
            this.龍 = 龍;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是兔");
            this.龍.ShowInfo();
        }
    }
}
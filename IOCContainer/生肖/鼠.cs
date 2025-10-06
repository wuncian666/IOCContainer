using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 鼠
    {
        private 牛 牛;

        public 鼠(牛 牛)
        {
            this.牛 = 牛;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是鼠");
            this.牛.ShowInfo();
        }
    }
}
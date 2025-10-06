using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 牛
    {
        private 虎 虎;

        public 牛(虎 虎)
        {
            this.虎 = 虎;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是牛");
            this.虎.ShowInfo();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 虎
    {
        private 兔 兔;

        public 虎(兔 兔)
        {
            this.兔 = 兔;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是虎");
            this.兔.ShowInfo();
        }
    }
}
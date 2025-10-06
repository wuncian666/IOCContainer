using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 雞
    {
        private 狗 狗;

        public 雞(狗 狗)
        {
            this.狗 = 狗;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是雞");
            this.狗.ShowInfo();
        }
    }
}
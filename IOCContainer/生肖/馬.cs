using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 馬
    {
        private 羊 羊;

        public 馬(羊 羊)
        {
            this.羊 = 羊;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是馬");
            this.羊.ShowInfo();
        }
    }
}
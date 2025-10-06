using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 羊
    {
        private 猴 猴;

        public 羊(猴 猴)
        {
            this.猴 = 猴;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是羊");
            this.猴.ShowInfo();
        }
    }
}
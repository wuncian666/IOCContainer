using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 狗
    {
        private 豬 豬;

        public 狗(豬 豬)
        {
            this.豬 = 豬;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是狗");
            this.豬.ShowInfo();
        }
    }
}
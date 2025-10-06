using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer.生肖
{
    internal class 猴
    {
        private 雞 雞;

        public 猴(雞 雞)
        {
            this.雞 = 雞;
        }

        public void ShowInfo()
        {
            Console.WriteLine("我是猴");
            this.雞.ShowInfo();
        }
    }
}
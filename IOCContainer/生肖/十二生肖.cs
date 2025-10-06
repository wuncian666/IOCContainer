using System;

namespace IOCContainer.生肖
{
    internal class 十二生肖
    {
        private 鼠 鼠;
        private 牛 牛;

        public 十二生肖(鼠 鼠)
        {
            this.鼠 = 鼠;
            //this.牛 = 牛;
        }

        public void Show()
        {
            Console.WriteLine("開始介紹十二生肖:");
            this.鼠.ShowInfo();
            //this.牛.ShowInfo();
        }
    }
}
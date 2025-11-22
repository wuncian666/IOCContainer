using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace NLogPratice
{
    public partial class Form1 : Form
    {
        public ILogger<Form1> Logger { get; set; }

        public Form1(ILogger<Form1> logger)
        {
            InitializeComponent();
            this.Logger = logger;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var name = button.Name;

            this.Logger.LogInformation($"{name} 按鈕被按了", name);
            this.Logger.LogInformation("LogEvent.UpdateItem", "執行更新");

            this.Logger.LogInformation("LogEvent.GenerateItem", "完成");
        }
    }
}
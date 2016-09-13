using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace winFrm_Async
{
    public partial class Form1 : Form
    {
        long m_state = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = "m_state: " + m_state.ToString();
            txtMessage.AppendText(msg + "\r\n");
            textBox1.Text = msg;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            txtMessage.AppendText("start → Timer\r\n");

            // non stop timer
            while(!chkStopTimer.Checked)
            {
                txtMessage.AppendText(DateTime.Now.ToLongTimeString() + "\r\n");
                await Task.Delay(1000);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CountTo10Async();
            txtMessage.AppendText("start → CountTo10Async()\r\n");
        }

        private async void CountTo10Async()
        {
            txtMessage.AppendText("ConutTo10 Begin\r\n"); 
            
            if (m_state >= 10)
                m_state = 0;

            while(m_state < 10)
            {
                m_state++;
                await Task.Delay(1000);
                txtMessage.AppendText(m_state.ToString() + "\r\n");
            }

            txtMessage.AppendText("ConutTo10 Finished\r\n");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            ShowThreadInfo(1, "正要起始非同步工作 MyDownloadPageAsync()。");

            Task<string> task = MyDownloadPageAsync("http://www.google.com.tw");

            ShowThreadInfo(3, "已經起始非同步工作 MyDownloadPageAsync()，但尚未取得工作結果。");

            string content = await task; // 等待"task"完成。

            ShowThreadInfo(5, "已經取得 MyDownloadPageAsync() 的結果。");

            txtMessage.AppendText(string.Format("網頁內容總共為 {0} 個字元。", content.Length));
        }

        async Task<string> MyDownloadPageAsync(string url)
        {
            ShowThreadInfo(2, "正要呼叫 WebClient.DownloadStringTaskAsync()。");

            var webClient = new WebClient();
            Task<string> task = webClient.DownloadStringTaskAsync(url);
            string content = await task;

            ShowThreadInfo(4, "已經取得 WebClient.DownloadStringTaskAsync() 的結果。");

            return content;
        }

        void ShowThreadInfo(int num, string msg)
        {
            txtMessage.AppendText(string.Format("({0}) T{1}: {2}\r\n", num, Thread.CurrentThread.ManagedThreadId, msg));
        }
    }
}

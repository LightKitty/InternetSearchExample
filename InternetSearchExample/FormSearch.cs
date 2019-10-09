using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//添加的命名空间引用
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
namespace InternetSearchExample
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            listBoxLinks.Items.Clear();
            listBoxImage.Items.Clear();
            pictureBox1.Image = null;
            webBrowser1.Url = new Uri("about:blank");
            richTextBox1.Clear();
            string urlString = textBoxUrl.Text.Trim();
            if (urlString.StartsWith("http://") == false&& urlString.StartsWith("https://") == false)
            {
                urlString = "http://" + urlString;
                textBoxUrl.Text = urlString;
            }
            string httpSource;
            try
            {
                //设置鼠标形状为沙漏形状
                Cursor.Current = Cursors.WaitCursor;
                WebClient webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8; // Encoding.GetEncoding("GB2312");
                //获取包含网页源代码的字符串。
                httpSource = webClient.DownloadString(textBoxUrl.Text);
                richTextBox1.Text = httpSource;
                webBrowser1.Url = new Uri(textBoxUrl.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }
            finally
            {
                //设置鼠标形状为默认形状
                Cursor.Current = Cursors.Default;
            }
            string regexHrefPattern = @"<a\s+href\s*=\s*""?([^"" >]+)""?>(.+)</a>";
            Regex myRegex = new Regex(regexHrefPattern, RegexOptions.IgnoreCase);
            Match myMatch = myRegex.Match(httpSource);
            while (myMatch.Success == true)
            {
                listBoxLinks.Items.Add(myMatch.Groups[0].Value);
                myMatch = myMatch.NextMatch();
            }
            string regexImgPattern =
            @"<img[^>]+(src)\s*=\s*""?([^ "">]+)""?(?:[^>]+([^"">]+)""?)?";
            myRegex = new Regex(regexImgPattern, RegexOptions.IgnoreCase);
            myMatch = myRegex.Match(httpSource);
            while (myMatch.Success == true)
            {
                listBoxImage.Items.Add(myMatch.Groups[2].Value);
                myMatch = myMatch.NextMatch();
            }
        }
        private void listBoxImage_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                pictureBox1.Image =
                Image.FromStream(client.OpenRead(listBoxImage.SelectedItem.ToString()));
            }
            catch
            {
                pictureBox1.Image = null;
            }
        }
    }
}
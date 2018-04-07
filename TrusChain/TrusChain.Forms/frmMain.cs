using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrusChain.Storage;

namespace TrusChain.Forms
{
    public partial class frmMain : Form
    {

         Storage.IpfsWrapper storage;
        public frmMain()
        {
            InitializeComponent();
            storage = new IpfsWrapper();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //try
            //{
               
            storage.Init();
            //    string hash = await storage.Add(@"C:\Users\JuniorCarvalho\Desktop\block.txt");
            //    Console.WriteLine("Your Hash is: " + hash);

            //    Console.ReadLine();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //storage.Dispose();
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txtFile.Text))
            {
                string hash = await storage.Add(txtFile.Text);
                txtHash.Text = hash;
                txtFile.Text = "";
            }
          
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofd.FileName;
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!txtDownloadHash.Text.Equals(String.Empty))
            {
                storage.Get(txtDownloadHash.Text, @"D:\file.pdf");
            }
        }
    }
}

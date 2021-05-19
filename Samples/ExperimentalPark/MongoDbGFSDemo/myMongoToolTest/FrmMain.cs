using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZZH.MongoDB.Service;
using System.IO;

namespace myMongoToolTest
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtFileName.Text = openFileDialog1.FileName;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.txtFileName.Text.Trim() != "")
            {
                if (File.Exists(this.txtFileName.Text))
                {
                    var config = new MongoConfig
                    {
                        AuthDb = "admin",
                        ConnectTimeout = 300000,
                        DefaultDb = "yudiefly",
                        PassWord = "zzh203@126.com",
                        UserName = "yudiefly",
                        ServerPort = 27017,
                        ServerConStr = "139.224.196.218",
                        SocketTimeout = 180000,
                        MaxConnectionIdleTime = 60000,
                        MaxConnectionLifeTime = 600000,
                        MaxConnectionPoolSize = 600,
                        WaitQueueSize = 10,
                        WaitQueueTimeout = 120000
                    };
                    var repostory = new DoMongoRepostory<myValueObject>("yudiefly", "test_file_upload", config);
                    var result = repostory.UploadFile(this.txtFileName.Text, "test_file_upload");
                   
                }
            }




        }
    }
    public class myValueObject : AggregateBase
    {

    }
}

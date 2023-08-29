using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Runtime.Remoting.Contexts;

namespace WF_HW_Search_Folders_and_Files
{
    public partial class Form1 : Form
    {

        


        Controller Controller = new Controller();
        bool FlagThread = true;
        string wordSearch = string.Empty;
        Thread threadFind;


        public Form1()
        {
            InitializeComponent();
            checkBox1.Checked = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.HeaderStyle = ColumnHeaderStyle.Clickable;
        
            buttonFind.Enabled = false;

            comboBoxDisk.DataSource = DriveInfo.GetDrives();


        }

  
        private void buttonFind_Click(object sender, EventArgs e)
        {
           
            wordSearch = "*"+textBoxFile.Text+"*";
            FlagThread = true;
            buttonStop.Enabled = true;
           


            ShowFileInfo();

            textBoxFile.Text = "";
            textBoxWordInFile.Text = "";



        }

        private void textBoxFile_TextChanged(object sender, EventArgs e)
       {
            if(textBoxFile.Text.Length > 0) 
            {
                buttonFind.Enabled = true;
                
            }
            else { buttonFind.Enabled = false; wordSearch = String.Empty; }


        }


        void foo(List<FileInfo> files)
        {
            
            foreach (var el in files)
            {
               
                listView1.Items.Add(el.Name);
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(el.DirectoryName);
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(el.Length.ToString());
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(el.CreationTime.ToString());
             
            }
            
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) Controller.Flagsubdirectories = true;
            if (!checkBox1.Checked) Controller.Flagsubdirectories = false;
        }

        private void textBoxWordInFile_TextChanged(object sender, EventArgs e)
        {
            if(textBoxWordInFile.Text.Length > 0)
                textBoxFile.Text = "*.txt";
           if(textBoxWordInFile.Text == "")
                textBoxWordInFile.Text = string.Empty;
            
        }







        void ShowFileInfo()
        {
            Controller.list = new List<FileInfo>();
            string[] str = { wordSearch, textBoxWordInFile.Text, comboBoxDisk.SelectedItem.ToString() };
            if (textBoxWordInFile.Text == "")
            { 
                threadFind = new Thread(Controller.FindFile);
                threadFind.Start(str);
                Thread.Sleep(1000);

                while (Controller.FlagThread)   //сдедил для контроля потока
                {
                    Thread.Sleep(1000);
                    continue;
                   
                }
                
                if (Controller.list.Count > 0)
                {
                    listView1.Items.Clear();
                    foo(Controller.list);
                    labelFindFiles.Text = "количество найденых файлов : " + listView1.Items.Count.ToString();
                   

                }

            }


            else
            {
                threadFind = new Thread(Controller.FindFile);
                threadFind.Start(str);
                Thread.Sleep(1000);

                while (Controller.FlagThread)
            {
                    Thread.Sleep(1000);
                    continue;
            }
                
                if (Controller.listByWors.Count > 0)
                {
                    
                    listView1.Items.Clear();
                    foo(Controller.listByWors);
                    labelFindFiles.Text = "количество найденых файлов : " + listView1.Items.Count.ToString();
                    
                }


            }




            if (listView1.Items.Count == 0)
            {
                listView1.Items.Add("Файлов не найдено");
            }





            textBoxFile.Text = "";
            textBoxWordInFile.Text = "";
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

            FlagThread = false;
            buttonStop.Enabled = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbxPatcher
{
    public partial class Form1 : Form
    {
        public List<string> ebxfiles = new List<string>();
        public string SbFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Main knopka   PATCH!
            Start();
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }

        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            foreach (string s in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (Directory.Exists(s))
                    ebxfiles.AddRange(Directory.GetFiles(s, "*.ebx", SearchOption.AllDirectories));
                else
                    ebxfiles.Add(s);

                richTextBox1.Text = string.Join("\r\n", ebxfiles);
            }        
        }

        public void Start()
        {
            ebxfiles.ForEach(x =>
           {
               readHeader(x);
           });
            
        }

        public  void readHeader(string filename)
        {
            var finfo = new FileInfo(filename);
            byte[] buffer = new byte[finfo.Length];

            buffer = File.ReadAllBytes(filename);

            // Skip 48 bytes Headers and prints GUIDS
            string guid = BitConverter.ToString(buffer, 48, 16).Replace("-", "");
//////////////////////////////////////////////////////////////////////////////////////////////////////
            string pattern = @"\S{1,48}";           // try to convert
            var result = Regex.Split(guid, pattern);// into 16 byte hex view not succesfull 

            richTextBox2.Text += finfo.Name + "\r\n" + guid + "\r\n \r\n";
        }

        public void writeGovnoInFile(string file, long offset)
        {
            byte[] govno = {255,255,255,255,255,255,255,255,255,255,255,255,255,255,255
        ,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,
        255,255,255,255,255,255,255,255,255,255,255,255,255,255,
        255,255,255,255};

            FileStream fs = new FileStream(file,FileMode.Open);

        }


        private void label2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SB File(*.sb)| *.sb";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SbFilePath = dialog.FileName;
                    label2.Text += " " + dialog.FileName;
                }
            }
        }
    }
}

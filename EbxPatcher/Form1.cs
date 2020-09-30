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
            // ebxfiles.ForEach(x =>
            //{
            //    readHeader(x);
            //});

            TestMethod();
        }

        public void readHeader(string filename)
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
            byte[] govno = {255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255};

            //FileStream fs = new FileStream(file, FileMode.Open);

        }

        //   public int findPos(int pos,byte[] Source, byte[] pattern)
        public void TestMethod()
        {
           int position = 0;
            string filename1 = @"C:\Users\Glock\Desktop\an94.ebx";
            string filename2 = @"C:\Users\Glock\Desktop\an94_moded.ebx";

            var finfo1 = new FileInfo(filename1);
            var finfo2 = new FileInfo(filename2);
            byte[] zerno = new byte[48];
            byte[] bytebuffer1 = new byte[finfo1.Length];
            byte[] bytebuffer2 = new byte[finfo2.Length];


            byte[] govno = {255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255,
                            255,255,255,255,255,255,255,255};


            bytebuffer1 = File.ReadAllBytes(filename1);
            bytebuffer2 = File.ReadAllBytes(filename2);

            for (int i = 0; i < zerno.Length; i++)
            {
                zerno[i] = bytebuffer1[i];
                richTextBox2.Text +=  string.Join(" ",bytebuffer1[i].ToString("x2"));
            }




            //  position = GetPos(bytebuffer2, zerno);

            position = IndexOf(bytebuffer2, zerno, 0);

            richTextBox2.Text += ($" \n Позиция в файле :  {position} \n");

            if (position > 0)
            {
                string findpos = BitConverter.ToString(bytebuffer2, position, 128).Replace("-", "");
                richTextBox2.Text += findpos;
                FileStream fs = new FileStream(filename2, FileMode.Open);
                fs.Seek((long)position, SeekOrigin.Begin);
                foreach (byte b in govno)
                {
                    fs.WriteByte(b);
                }
                fs.Close();
            }
            
        }


        public int IndexOf(byte[] source, byte[] pattern, int offset)
        {
            int success = 0;
            for (int i = offset; i < source.Length; i++)
            {
                if (source[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return i - pattern.Length + 1;
                }
            }
            return -1;
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

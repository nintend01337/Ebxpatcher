using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbxPatcher.Classes
{
    public class Ebx
    {
        public string path { get; set; }
        public string head { get; set; }
        public string guid { get; set; }
        public byte[] ebxdata;
        public byte[] bytehead;
        

        public Ebx(string filename)
        {
            path = filename;
            FileInfo info = new FileInfo(path);
            ebxdata = new byte[info.Length];
            bytehead = new byte[64];
            readData(path);
            formHeader();
            setGuid();
        }


        private void readData(string path)
        {
            path = this.path;
            ebxdata = File.ReadAllBytes(path);
        }

        private void formHeader()
        {
            for (int i = 0; i < bytehead.Length; i++)
            {
                bytehead[i] = ebxdata[i];
            }
                head = BitConverter.ToString(bytehead, 0, 48).ToLower().Replace("-", " ");
        }

        //private string getStringHeader()
        //{
        //    return BitConverter.ToString(bytehead, 0, 48).ToLower().Replace("-", " ");
        //}

        private void setGuid()
        {
           guid =  BitConverter.ToString(bytehead, 48, 64).ToLower().Replace("-", " ");
        }
    }
}

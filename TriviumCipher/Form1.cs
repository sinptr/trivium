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
using System.Collections;

namespace TriviumCipher
{
    public partial class Form1 : Form
    {
        private Trivium trivium;

        public Form1()
        {
            InitializeComponent();
            BitArray key = new BitArray(System.Text.Encoding.UTF8.GetBytes("A0oIll983D"));
            BitArray initVector = new BitArray(System.Text.Encoding.UTF8.GetBytes("321I11113D"));
            trivium = new Trivium(key, initVector);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(openFileDialog1.FileName))
                {
                    label1.Text = openFileDialog1.SafeFileName.ToString();
                    button2.Visible = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(openFileDialog1.FileName))
            {
                BitArray source = new BitArray(File.ReadAllBytes(openFileDialog1.FileName));
                BitArray newData = trivium.Encrypt(source);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    byte[] byteArray = new byte[(int)Math.Ceiling((double)newData.Length / 8)];
                    newData.CopyTo(byteArray, 0);
                    File.WriteAllBytes(saveFileDialog1.FileName, byteArray);
                }
            }
        }
    }
}

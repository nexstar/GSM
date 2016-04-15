using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GSM_GUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                Comport.Items.Add(port);
            }
        }
        int F2_Comport;
        private void button1_Click(object sender, EventArgs e)
        {
            string Cutstring = Comport.SelectedItem.ToString().ToString();
            int dd = Cutstring.IndexOf("M");
            F2_Comport = Int32.Parse(Cutstring.Substring(dd + 1));
            Com.SetCom(F2_Comport);
            this.Close();
        }       
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using Microsoft.Win32;
using System.IO;

namespace GSM_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int baudRate = 9600;
        private int timeout = 300;
        private OleDbConnection connection = new OleDbConnection();
        private OleDbCommand command = new OleDbCommand();
        static ManualResetEvent Dome = new ManualResetEvent(false);
        static SmsSubmitPdu pdu;
        static GsmCommMain comm;
        static bool DB_;
        Form2 f2;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("GSM_Message" + "({0}/{1})", sentcount, limit_char);
            this.ControlBox = false;
            f2 = new Form2();
            if (ReadKey("Comport") == null) // 第一次
            {
                f2.ShowDialog();
                WriteRegKey("Comport", Com.GetCom().ToString());
                StartCom();
            }
            else
            {
                StartCom();
            }
        }
        private void StartCom()
        {
            comprot();
            if (comm.IsOpen())
            {
                AccessBook();
                StartUp();
            }
        }
        // StartCom function


        private void WriteRegKey(string keyName, Object value)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey sk1 = rk.CreateSubKey(@"SOFTWARE\Comport");
                sk1.SetValue(keyName, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        // Write Registry column

        private string ReadKey(string key)
        {
            string kq = "";
            try
            {
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey sk1 = rk.CreateSubKey(@"SOFTWARE\Comport");
                kq = (string)sk1.GetValue(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return kq;
        }
        // Read Registry column
        static Thread startup, DbThread;
        private void StartUp()
        {
            textBox1.Text += "Server StartUp......" + Environment.NewLine;
            startup = new Thread(Startup);
            startup.Start();
            DbThread = new Thread(DB);
            DbThread.Start();
        }
        // run All Thread 

        private void AccessBook()
        {
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            command.Connection = connection;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mydocpath + @"\GSM_GUI\Database1.accdb;Persist Security Info=False;Jet OLEDB:Database Password=@1436;";// @1436 如需要修改請務必記得AccessLockCode要更換                   
            connection.Open();
            textBox1.Text += "AccessBook:\t" + "Ok" + Environment.NewLine;
        }
        // Is Look for Access exist ?

        private void comprot()
        {
            comm = new GsmCommMain(Int32.Parse(ReadKey("Comport")), baudRate, timeout);
            try
            {
                comm.Open();
            }
            catch (Exception e) 
            {
                MessageBox.Show(e.ToString());
            }
             
            try
            {
                if (!(comm.IsConnected()))
                {
                    textBox1.Text += "Device:\t" + "Com" + Int32.Parse(ReadKey("Comport")) + "\t" + "No" + Environment.NewLine;

                }
                else
                {
                    textBox1.Text += "Device:\t" + "Com" + Int32.Parse(ReadKey("Comport")) + "\t" + "Ok" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Connection error: " + ex.Message, "Connection setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        // write Comport number and connect Comprot
        static Socket listener;
        System.DateTime currentTime;
        string[] cut_PhGr;
        static OleDbDataReader reader;


        private void getIP()
        {
            limit = new List<string>();

            try
            {
                command.CommandText = "select IpAddress from IP ";
                command.CommandType = System.Data.CommandType.Text;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    limit.Add(reader["IpAddress"].ToString());
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                reader.Close();
            }
        }

        private void DB(object obj)  
        {
            getIP();

            //在此可以加入可通過名單
            do
            {
                if (DB_)
                {
                    DB_ = false;
                    cut_PhGr = number[0].Split(',');
                    foreach (string i in cut_PhGr)
                    {
                        while (true)
                        {
                            Sql_Getgroup(i);
                            sql_Getphone(i);
                            SendTotal();
                            break;
                        }
                    }
                }
            } while (true);
        }
        //default Array space and waiting check Data came

        static string CutString;
        static List<string> limit;
        int sentcount = 0;
        static int limit_char = 500; //SMS總量限制改數字(500)
        private void SendTotal()
        {

            Denom_count = 1;
            Clearphone = setphone.Distinct().ToList();
            setphone.RemoveRange(0, setphone.Count());
            foreach (string clearchar in Clearphone)
            {
                setphone.Add(clearchar);
            }
            foreach (string p in setphone)
            {
                try
                {
                    if (number[1].Length >= 70)
                    {
                        CutString = number[1].Substring(0, 70);
                    }
                    else
                    {
                        CutString = number[1];
                    }
                    int Tcount = 0;
                    foreach (string lim in limit)
                    {
                        if (!(number[2] == lim))
                        {
                            Tcount++;
                            if (Tcount == limit.Count())
                            {
                                limit_("");
                                return;
                            }
                        }
                    }
                    sentcount++;
                    this.Labeldispaly("");
                    if (sentcount <= limit_char)
                    {
                        pdu = new SmsSubmitPdu(CutString, p, DataCodingScheme.NoClass_16Bit);
                        this.SetText("", p);
                        comm.SendMessage(pdu);   
                    }
                    else
                    {
                        Limitlog("");
                    }
                }
                catch (Exception issue)
                {
                    writeSMSLog(p);
                    this.Denom("");
                    Thread error = new Thread(Error);
                    error.Start(issue.ToString());
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string write = String.Format("{0}\t{1}\t", currentTime = System.DateTime.Now, issue);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(desktop + @"\GSM_GUI\issue_wrong.txt", true))
                    {
                        file.WriteLine(write);
                    }                    
                }           
            }
            setphone.RemoveRange(0, setphone.Count());
        }

        private void writeSMSLog(string p)
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string write = String.Format("{0}\t{1}\t{2}\t{3}\t", currentTime = System.DateTime.Now, p, number[1], number[2]);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(desktop + @"\GSM_GUI\SMSlog.txt", true))
            {
                file.WriteLine(write);
            }
        }
        
        // Assemble exist phone , cut more than the 70 (string)  but this string writing on file folder
        delegate void labeldispaly(string text);
        delegate void limitlog(string text);        
        delegate void Limitation(string text);
        delegate void Denominator(string text);
        delegate void SetTextCallback(string text, string i);
       
        private void Labeldispaly(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                labeldispaly d = new labeldispaly(Labeldispaly);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.Text = String.Format("GSM_Message" + "({0}/{1})", sentcount, limit_char);
            }
        }
        private void Limitlog(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                limitlog d = new limitlog(Limitlog);
                this.Invoke(d, new object[] { text});
            }
            else
            {
                textBox1.Text += String.Format("{0}\t", "Out of range") + Environment.NewLine; ;
            }
        }
        private void SetText(string text, string i)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, i });
            }
            else
            {
                textBox1.Text += String.Format("{0}\t{1}\t{2}\t{3}\t", currentTime = System.DateTime.Now, i, CutString, number[2]);
            }
        }

        int Denom_count;
        private void Denom(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                Denominator d = new Denominator(Denom);
                this.Invoke(d, new object[] { text });
                Denom_count++;
            }
            else
            {
                textBox1.Text += String.Format("{0}/{1}", Denom_count, setphone.Count()) + Environment.NewLine;
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
        }

        private void limit_(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                Limitation d = new Limitation(limit_);
                this.Invoke(d, new object[] { text });                
            }
            else
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string ErrorLog = String.Format("{0}\t{1}\t{2}\t", currentTime = System.DateTime.Now, number[2], "Lllegal connection");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(desktop + @"\GSM_GUI\SMSlog.txt", true))
                {
                    file.WriteLine(ErrorLog);
                }
                textBox1.Text += String.Format("{0}\t{1}\t{2}\t", currentTime = System.DateTime.Now, number[2], "illegal connection") + Environment.NewLine;                
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
        }


        //delegate function 
        List<string> setphone = new List<string>();
        List<string> Clearphone;

        private void sql_Getphone(string i)
        {
            try
            {
                command.CommandText = "select Phone from AddressBook where phone='" + i + "'";
                command.CommandType = System.Data.CommandType.Text;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    setphone.Add(reader["Phone"].ToString());
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                reader.Close();
            }
        }
        private void Sql_Getgroup(string i)
        {
            try
            {
                command.CommandText = "select Phone from AddressBook where gp='" + i + "'";
                command.CommandType = System.Data.CommandType.Text;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    setphone.Add(reader["Phone"].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                reader.Close();
            }
        }

        // input phone & group output column exist

        private void Startup(object obj)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Helper.GetIP4Address()), "*1*"); // "*1*"為PortCode
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(ipe);
                listener.Listen(100);
                while (true)
                {
                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);
                    Thread.Sleep(10);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("StartUp : " + ex.ToString());
            }
        }

        // StartUp Function

        public class StateOject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        // default Type 
        static Socket handler;
        public static void AcceptCallBack(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            handler = listener.EndAccept(ar);
            StateOject so = new StateOject();
            so.workSocket = handler;
            handler.BeginReceive(so.buffer, 0, StateOject.BufferSize, 0, new AsyncCallback(ReadCallBack), so);
        }

        //Accept Connect function
        static string[] number;
        static int count = 0;

        public static void ReadCallBack(IAsyncResult ar)
        {

            number = new string[3];
            String Content = String.Empty;
            StateOject so = (StateOject)ar.AsyncState;
            Socket handler = so.workSocket;
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                so.sb.Append(Encoding.Unicode.GetString(so.buffer, 0, bytesRead));
                Content = so.sb.ToString();
                if (Content.Length > -1)
                {
                    string[] cut = Content.Split('_');
                    foreach (string i in cut)
                    {
                        number[count] = i.ToString();
                        count++;
                    }
                    count = 0;
                    DB_ = true;

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
        }

        //Read Client Send Stream Data
        private static void Error(object obj)
        {
            obj = string.Empty;
        }

        // Sms Error Empty function
        class Helper
        {
            public static string GetIP4Address()
            {
                IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress i in ips)
                {
                    if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return i.ToString();
                    }
                }
                return "127.0.0.1";
            }
        }
        // get local IP4 Address function

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comm.IsOpen())
            {
                comm.Close();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            textBox1.Width = this.Width;
            this.Size = new Size(this.Size.Width, 314);

        }
        // Form Closeding start function
    }
}

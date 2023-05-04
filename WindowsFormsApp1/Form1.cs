using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApp1.Functions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CloseConnection();

            String[] portlists = SerialPort.GetPortNames().Distinct().ToArray();
            cbPort.Items.AddRange(portlists);

            Connect();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void Connect()
        {
            try
            {
                serialPort1.PortName = ReadFileConfiguration.PortName();
                serialPort1.BaudRate = Convert.ToInt32(ReadFileConfiguration.BaudRate());
                serialPort1.Open();

                if (serialPort1.IsOpen)
                {
                    OpenConnection();
                    Thread.Sleep(1000);
                    MessageBox.Show("conectado");
                }
                else
                {
                    MessageBox.Show("falha na conexao");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenConnection()
        {
            btnOpen.Enabled = false;
            btnClose.Enabled = true;
            btnSend.Enabled = true;
            lblStatus.Text = "Connected";
            lblStatus.ForeColor = Color.Green;
        }
        private void CloseConnection()
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }

            btnOpen.Enabled = true;
            btnClose.Enabled = false;
            btnSend.Enabled = false;
            lblStatus.Text = "Disconnected";
            lblStatus.ForeColor = Color.Red;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                String texto = txtInLine.Text + "#";
                serialPort1.Write(texto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        String serialDataIn;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialDataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(ShowData));
        }
        String dado = "";
        private void ShowData(object sender, EventArgs e)
        {
            txtMultLine.Text += serialDataIn;
            dado += serialDataIn;
            if (serialDataIn == "Conectado")
            {

                MessageBox.Show("Conectado na porta " + serialPort1.PortName);
                return;
            }

            //link abaixo conte a tabela de teclas usadas via c#
            //https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?redirectedfrom=MSDN&view=windowsdesktop-7.0
            if (dado.Contains("\r\n"))
            {

                if (dado.StartsWith("vol@"))
                {
                    new ManagerFunctions(this.Handle, dado);
                }
                else
                {
                    string key = new string(dado.Where(char.IsLetterOrDigit).ToArray());
                    if (buttons.Any(x => x == key))
                    {
                        new ManagerFunctions(this.Handle, key);
                    }
                    else if (pages.Any(x => x == key))
                    {
                        String dadoLinhaLcd = String.Concat(ReadFileConfiguration.Page(key), "#");

                        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(dadoLinhaLcd);

                        serialPort1.Write(bytes, 0, bytes.Length);
                        Debug.WriteLine(dadoLinhaLcd);
                        Thread.Sleep(500);
                    }
                }

                dado = "";
            }



        }
        List<String> pages = new List<String>() { "pageA", "pageB", "pageC", "pageD" };
        List<String> buttons = new List<string>() { "A1", "A2", "A3", "A4", "A5", "A6",
                                                    "B1", "B2", "B3", "B4", "B5", "B6",
                                                    "C1", "C2", "C3", "C4", "C5", "C6",
                                                    "D1", "D2", "D3", "D4", "D5", "D6",
                                                    "vol","volU", "volD", "volmute"};
        private void txtMultLine_TextChanged(object sender, EventArgs e)
        {
            txtMultLine.SelectionStart = txtMultLine.Text.Length;
            txtMultLine.ScrollToCaret();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtMultLine.Text = "";
            txtInLine.Text = "";
        }
    }
}

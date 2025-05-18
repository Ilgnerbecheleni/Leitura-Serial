using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leitura_Serial
{
    public partial class Form1 : Form
    {
        public string[] portasatuais = new string[0];
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            atualizarListaPortas();
        }

        private void atualizarListaPortas()
        {
            string[] portas = SerialPort.GetPortNames();

            if (!portas.SequenceEqual(portasatuais))
            {
                portasatuais = portas;
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(portas);


                if (portas.Length > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void btnConecta_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("selecione uma porta serial para se conectar!!");
                    return;
                }

                try
                {
                    serialPort1.PortName = comboBox1.SelectedItem.ToString();
                    serialPort1.BaudRate = 9600;
                    serialPort1.Open();

                    btnConecta.Text = "Desconectar";
                    comboBox1.Enabled = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("erro ao se conectar" + ex.Message);
                }




            }
            else
            {
                try
                {
                    serialPort1.Close();
                    btnConecta.Text = "Conectar";
                    comboBox1.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("erro ao se desconectar" + ex.Message);
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen){
                serialPort1.Write("READ");
                // A:512,D1:1,D2:0
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string resposta = serialPort1.ReadLine();
                
              
                this.Invoke((MethodInvoker)(() => {
                    Processarresposta(resposta);
                }));

            }
            catch
            {

            }
        }

        public void Processarresposta( string resposta)
        {
            string[] partes = resposta.Split(',');
            value.Text = resposta;
            foreach (string parte in partes)
            {
                if (parte.StartsWith("A:"))
                {
                    string v1 = parte.Substring(2);
                    if( float.TryParse(v1 , out float val))
                    {
                        aGauge1.Value = val;
                    }

                }else if (parte.StartsWith("D1:"))
                {
                    textBox1.BackColor = parte.EndsWith("1")? Color.Green : Color.Red;
                }
                else if (parte.StartsWith("D2:"))
                {
                    textBox2.BackColor = parte.EndsWith("1") ? Color.Green : Color.Red;
                }




            }

        }



    }
}

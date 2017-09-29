using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logger;
using System.IO;

namespace LogTest
{
    public partial class Form1 : Form
    {
        ILogger logger;
        public Form1()
        {
            InitializeComponent();

            // can be resolved based on config
            logger = new WebAPILogger.WebAPILogger();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                int j = 5;


                int x = j / i;

            }
            catch (Exception ex)
            {
                for (int count = 0; count < 500; count++)
                {

                    logger.WriteLog(Common.LogType.Error, ex.Message + " : " + ex.StackTrace + " : " + ex.InnerException);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int[] iArray = new int[2];
                Console.Write(iArray[3].ToString());
            }
            catch (Exception ex)
            {
                sendLogs(ex);
            }
        }

        private void sendLogs(Exception ex)
        {
            for (int count = 0; count < int.Parse(txtIteration.Text) ; count++)
            {

                logger.WriteLog(Common.LogType.Error, ex.Message + " : " + ex.StackTrace + " : " + ex.InnerException);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> lstSTring = null;
                lstSTring.Add("text");
            }
            catch (Exception ex)
            {
                sendLogs(ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.File.ReadLines("D:\\NotAvailable.txt");
            }
            catch (Exception ex)
            {
                sendLogs(ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists("D:\\TestFile.txt")) File.Create("D:\\TestFile.txt");
                using (StreamReader fileReader = new StreamReader("D:\\TestFile.txt"))
                {
                    using (StreamWriter fileWriter = File.AppendText("D:\\TestFile.txt"))

                    {
                        fileWriter.WriteLine("Hello");
                    }
                }
            }
            catch (Exception ex)
            {
                sendLogs(ex);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string invalidDate = "123;131";
                DateTime iDate = Convert.ToDateTime(invalidDate);
            }
            catch (Exception ex)
            {
                sendLogs(ex);
            }
        }
    }
}

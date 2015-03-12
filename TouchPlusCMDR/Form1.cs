using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TouchPlusCMDR
{
    public partial class Form1 : Form
    {
        Viewer _Viewer;                                                             // The Viewer form
        TouchPlus _TouchPlus;                                                       // The Touch+ DLL Library class
        int IRLED = -1;                                                             // IR light is on or off
        int Filters = 1;                                                            // Filters enabled or not?

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateErrors()
        {
            for (int x = 0; x < _TouchPlus.errors.Count; x++)
                listBox1.Items.Add(_TouchPlus.errors[x].ToString());
            _TouchPlus.errors.Clear();
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private void UpdateMessages()
        {
            for (int x = 0; x < _TouchPlus.messages.Count; x++)
                listBox1.Items.Add(_TouchPlus.messages[x].ToString());
            _TouchPlus.messages.Clear();
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _TouchPlus.LoadLibs();
            while (_TouchPlus.busy) ;                                               // Wait for the library to finish
            if (_TouchPlus.errors.Count == 0)
            {
                _TouchPlus.InitTouchPlus();
                while (_TouchPlus.busy) ;                                           // Wait for the library to finish
                if (_TouchPlus.errors.Count == 0)
                {
                    _TouchPlus.UnlockTouchPlus();
                    while (_TouchPlus.busy) ;                                       // Wait for the library to finish
                    UpdateMessages();

                    _Viewer = new Viewer();
                    _Viewer.Show();
                    _Viewer.InitDisplay(_TouchPlus.GetDeviceNum());
                }
                else
                {
                    UpdateErrors();
                }
            }
            else
            {
                UpdateErrors();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _TouchPlus = new TouchPlus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_Viewer != null)
            {
                _Viewer.CloseDisplay();
            }
            try
            {
                while (_Viewer.running == true) ;
                _Viewer.Close();
            }
            catch
            {

            }
            
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_Viewer != null)
            {
                _Viewer.CloseDisplay();
                _Viewer.Close();
                _Viewer.Dispose();
            }
            _TouchPlus.LockTouchPlus();
            while (_TouchPlus.busy) ;                                               // Wait for the library to finish
            if (_TouchPlus.errors.Count == 0)
            {
                UpdateMessages();
            }
            else
            {
                UpdateErrors();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IRLED == -1)
            {
                _TouchPlus.IRLedOFF();
                _Viewer.SetThreshold(60);
                IRLB.Text = "IR: OFF";
                IRLED = 0;
                if (_TouchPlus.errors.Count == 0)
                {
                    UpdateMessages();
                }
                else
                {
                    UpdateErrors();
                }
            }
            else if (IRLED == 0)
            {
                _TouchPlus.IRLedON();
                _Viewer.SetThreshold(20);
                IRLB.Text = "IR: ON";
                IRLED = 1;
                if (_TouchPlus.errors.Count == 0)
                {
                    UpdateMessages();
                }
                else
                {
                    UpdateErrors();
                }
            }
            else if (IRLED == 1)
            {
                _TouchPlus.IRLedOFF();
                _Viewer.SetThreshold(60);
                IRLB.Text = "IR: OFF";
                IRLED = 0;
                if (_TouchPlus.errors.Count == 0)
                {
                    UpdateMessages();
                }
                else
                {
                    UpdateErrors();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Filters == 1)
            {
                _Viewer.SetNoFilters();
                Filters = 0;
                FilterLB.Text = "FILTER: OFF";
            }
            else
            {
                _Viewer.SetFilters();
                Filters = 1;
                FilterLB.Text = "FILTER: ON";
            }
        }

        private void PictureBN_Click(object sender, EventArgs e)
        {
            Bitmap pic = _Viewer.SavePicture();
            saveFileDialog1.Title = "Picture taken...";
            saveFileDialog1.Filter = "JPG (*.jpg)|*.jpg";
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                pic.Save(saveFileDialog1.FileName,System.Drawing.Imaging.ImageFormat.Jpeg);
            }         
        }

        private void UnlockOnly_Click(object sender, EventArgs e)
        {
            _TouchPlus.LoadLibs();
            while (_TouchPlus.busy) ;                                               // Wait for the library to finish
            if (_TouchPlus.errors.Count == 0)
            {
                _TouchPlus.InitTouchPlus();
                while (_TouchPlus.busy) ;                                           // Wait for the library to finish
                if (_TouchPlus.errors.Count == 0)
                {
                    _TouchPlus.UnlockTouchPlus();
                    while (_TouchPlus.busy) ;                                       // Wait for the library to finish
                    UpdateMessages();
                }
                else
                {
                    UpdateErrors();
                }
            }
            else
            {
                UpdateErrors();
            }
        }
    }
}

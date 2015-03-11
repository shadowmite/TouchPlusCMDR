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

        public Form1()
        {
            InitializeComponent();
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
                    for (int x = 0; x < _TouchPlus.messages.Count; x++)
                        listBox1.Items.Add(_TouchPlus.messages[x].ToString());
                    _TouchPlus.messages.Clear();

                    _Viewer = new Viewer();
                    _Viewer.Show();
                    _Viewer.InitDisplay(_TouchPlus.GetDeviceNum());
                }
                else
                {
                    for (int x = 0; x < _TouchPlus.errors.Count; x++)
                        listBox1.Items.Add(_TouchPlus.errors[x].ToString());
                    _TouchPlus.errors.Clear();
                }
            }
            else
            {
                for (int x = 0; x < _TouchPlus.errors.Count; x++)
                    listBox1.Items.Add(_TouchPlus.errors[x].ToString());
                _TouchPlus.errors.Clear();
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
                for (int x = 0; x < _TouchPlus.messages.Count; x++)
                    listBox1.Items.Add(_TouchPlus.messages[x].ToString());
                _TouchPlus.messages.Clear();
            }
            else
            {
                for (int x = 0; x < _TouchPlus.errors.Count; x++)
                    listBox1.Items.Add(_TouchPlus.errors[x].ToString());
                _TouchPlus.errors.Clear();
            }
        }
    }
}

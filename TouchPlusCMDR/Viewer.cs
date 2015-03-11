﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math;
using AForge.Math.Geometry;
using AForge.Vision;
using AForge.Vision.Motion;
using AForge.Video;
using AForge.Video.DirectShow;

namespace TouchPlusCMDR
{
    public partial class Viewer : Form
    {
        private FilterInfoCollection VideoCaptureDevices;                                                           // All video devices available
        private VideoCaptureDevice FinalVideoSource;                                                                // Video device we will be using
        public Boolean running = true;                                                                              // Indicate if we have shut down or not
        Crop filterL = new Crop(new Rectangle(0, 0, 640, 480));                                                     // Filter the left cam feed
        Crop filterR = new Crop(new Rectangle(641, 0, 640, 480));                                                   // Filter the right cam feed
        Bitmap background = null;                                                                                   // Hold the background image
        Boolean NoFilters = false;
        Boolean PictureTime = false;
        Bitmap SavePic;

        public Viewer()
        {
            InitializeComponent();
        }

        public Bitmap SavePicture()
        {
            PictureTime = true;
            while (PictureTime) ;                       // Wait for the picture to be taken by the event function
            return SavePic;
        }
        public void InitDisplay(int DeviceNum)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            FinalVideoSource = new VideoCaptureDevice(VideoCaptureDevices[DeviceNum].MonikerString);
            FinalVideoSource.VideoResolution = FinalVideoSource.VideoCapabilities[1];
            //FinalVideoSource.DisplayPropertyPage(this.Handle);
            FinalVideoSource.NewFrame += new NewFrameEventHandler(FinalVideoSource_NewFrame);
            FinalVideoSource.Start();
        }

        public void CloseDisplay()
        {
            FinalVideoSource.Stop();
            FinalVideoSource.NewFrame -= new NewFrameEventHandler(FinalVideoSource_NewFrame);
            running = false;
        }

        public void SetNoFilters()
        {
            NoFilters = true;
            background = null;
        }

        public void SetFilters()
        {
            NoFilters = false;
        }

        void FinalVideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();                                                 // Get a local copy from the event
            if (PictureTime)
            {
                FinalVideoSource.NewFrame -= new NewFrameEventHandler(FinalVideoSource_NewFrame);
                StereoAnaglyph SAfilter = new StereoAnaglyph( );
                // set right image as overlay
                SAfilter.OverlayImage = filterR.Apply(image);
                // apply the filter (providing left image)
                SavePic = SAfilter.Apply(filterL.Apply(image));
                PictureTime = false;
                FinalVideoSource.NewFrame += new NewFrameEventHandler(FinalVideoSource_NewFrame);
            }
            if (NoFilters)
            {
                pictureBox1.Image = image;
            }
            else
            {
                if (background == null)                                                                         // This will pass the first time or when we nuke the background var
                {
                    background = new Bitmap(1280, 480);
                    background = Grayscale.CommonAlgorithms.BT709.Apply(image);
                }
                else                                                                                            // The rest of the time lets do this...
                {
                    Difference differenceFilter = new Difference(background);
                    Threshold thresholdFilter = new Threshold(30);
                    pictureBox1.Image = thresholdFilter.Apply(differenceFilter.Apply(Grayscale.CommonAlgorithms.BT709.Apply(image)));
                }
                image.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Let's reset the background image...
            background = null;
        }
    }
}

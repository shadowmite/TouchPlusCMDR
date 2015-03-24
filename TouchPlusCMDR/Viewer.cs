 using System;
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
using Emgu.CV;
using Emgu.CV.Structure;

namespace TouchPlusCMDR
{
    public partial class Viewer : Form
    {
        enum Input { Left, Right };                                                                                 // Define a type to define left or right input image
        Hand hand = new Hand();                                                                                     // The class which will hold our hand position model

        private FilterInfoCollection VideoCaptureDevices;                                                           // All video devices available
        private VideoCaptureDevice FinalVideoSource;                                                                // Video device we will be using
        public Boolean running = true;                                                                              // Indicate if we have shut down or not
        static int xMax = 1280;                                                                                     // Holds the bitmap x max - double wide
        static int yMax = 480;                                                                                      // Holds the bitmap y max - single high
        Crop filterL = new Crop(new Rectangle(0, 0, (xMax / 2), yMax));                                             // Filter the left cam feed
        Crop filterR = new Crop(new Rectangle((xMax / 2) + 1, 0, (xMax / 2), yMax));                                // Filter the right cam feed
        Bitmap background = null;                                                                                   // Hold the background image
        int ThresVal = 30;                                                                                          // Threshold value to use
        int minHeight = 100;                                                                                        // Minimum height of blobs detected?
        int maxWidth = 80;                                                                                          // Maximum width of blobs detected?
        Boolean NoFilters = false;
        Boolean PictureTime = false;
        Bitmap SavePic;
        Image<Gray, float> disparity;                                                                               // Hold the disparity map for the frame
        int x = 0;
        int y = 0;
        int z = 0;

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
            // 0 - 640x480
            // 1 - 1280x480 - Dual
            // 2 - 320x240
            // 3 - 640x240 - Dual
            // The other modes were in between sizes and/or wide sizes... The smaller sizes seem to be crops of the image sensor, not scaled.
            FinalVideoSource.VideoResolution = FinalVideoSource.VideoCapabilities[1];
            //FinalVideoSource.DisplayPropertyPage(this.Handle);
            FinalVideoSource.NewFrame += new NewFrameEventHandler(FinalVideoSource_NewFrame);
            FinalVideoSource.VideoSourceError += new VideoSourceErrorEventHandler(FinalVideoSource_ErrorEvent);
            FinalVideoSource.Start();
        }

        public void CloseDisplay()
        {
            FinalVideoSource.VideoSourceError -= new VideoSourceErrorEventHandler(FinalVideoSource_ErrorEvent);
            FinalVideoSource.Stop();
            FinalVideoSource.NewFrame -= new NewFrameEventHandler(FinalVideoSource_NewFrame);
            running = false;
        }

        private void FinalVideoSource_ErrorEvent(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            // Try to capture some errors and print them...
            MessageBox.Show("Error: " + eventArgs.Description);
        }

        private void FinalVideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Unregister the event until we get finished with this frame...
            FinalVideoSource.NewFrame -= new NewFrameEventHandler(FinalVideoSource_NewFrame);
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();                                                     // Get a local copy from the event
            if (PictureTime)                                                                                    // Take a 3D snapshot
            {
                StereoAnaglyph SAfilter = new StereoAnaglyph( );
                // set right image as overlay
                SAfilter.OverlayImage = filterR.Apply(image);
                // apply the filter (providing left image)
                SavePic = SAfilter.Apply(filterL.Apply(image));
                PictureTime = false;
            }

            // Not taking a picture, handle this as a normal frame
            if (background == null)                                                                         // This will pass the first time or when we nuke the background var
            {
                background = new Bitmap(xMax, yMax);
                background = Grayscale.CommonAlgorithms.BT709.Apply(image);
            }
            else                                                                                            // The rest of the time lets do this...
            {
                hand.ClearFingers();                                                                        // Since this is a new data frame, let's clear the old data
                ProcessImage(image);                                                                        // Process the frame
            }
            image.Dispose();

            // Re-register for the event now that we are done
            FinalVideoSource.NewFrame += new NewFrameEventHandler(FinalVideoSource_NewFrame);
        }

        private Bitmap FilterImage(Bitmap image)
        {
            // Create the filters to get our difference from the background and threshold it to enhance
            ThresholdedEuclideanDifference TEDfilter = new ThresholdedEuclideanDifference(ThresVal);
            TEDfilter.OverlayImage = background;        // Sets the background of the difference filter to the global background image (set elsewhere)
            return TEDfilter.Apply(Grayscale.CommonAlgorithms.BT709.Apply(image));
        }

        private void ProcessImage(Bitmap image)
        {
            // Call the function to apply filtering and enhancement to the captured frame before processing for blobs
            Bitmap CombinedImage = FilterImage(image);

            // Create the processed L and R images for blob processing
            Bitmap ProcessedL = filterL.Apply(CombinedImage);
            Bitmap ProcessedR = filterR.Apply(CombinedImage);

            // Free the memory from the filtered combined image
            CombinedImage.Dispose();

            // Create final L and R images based on source
            Bitmap imageL = filterL.Apply(image);
            Bitmap imageR = filterR.Apply(image);

            // Eventually when done testing/tinkering/and generally toying with different techniques we need to eliminate the L and R images and only keep the overlay.

            // Process the disparity map
            StereoBM bm = new StereoBM(Emgu.CV.CvEnum.STEREO_BM_TYPE.BASIC, 0);
            disparity = new Image<Gray, float>(xMax / 2, yMax);
            bm.FindStereoCorrespondence(new Image<Gray, Byte>(ProcessedL), new Image<Gray, Byte>(ProcessedR), disparity);
            //CvInvoke.cvConvertScale(disparity, disparity, 16, 0);
            //CvInvoke.cvNormalize(disparity, disparity, 0, 255, Emgu.CV.CvEnum.NORM_TYPE.CV_MINMAX,IntPtr.Zero);
            pictureBoxD.Image = disparity.ToBitmap(320, 240);
            
            // Process the left
            pictureBoxL.Image = ProcessBlobs(imageL, ProcessedL, Input.Left);

            // Process the right
            pictureBoxR.Image = ProcessBlobs(imageR, ProcessedR, Input.Right);

            // Process the combined data
            pictureBoxC.Image = ProcessFingerData();

            disparity.Dispose();        // Free memory no longer needed
            ProcessedL.Dispose();       // Free memory no longer needed
            ProcessedR.Dispose();       // Free memory no longer needed
            imageL.Dispose();           // Free memory no longer needed
            imageR.Dispose();           // Free memory no longer needed
        }

        private System.Drawing.Image ProcessFingerData()
        {
            Bitmap Overlay = new Bitmap(xMax / 2, yMax);
            hand.CheckAndDiscard();                                                             // Tell our hand model class to do it's magic
            if (hand.FingerCount() > 0)
            {
                Graphics g = Graphics.FromImage(Overlay);
                System.Drawing.Point temp = hand.GetAveragePosition();
                g.DrawEllipse(new Pen(Color.Blue), temp.X / 2, temp.Y / 2, 10, 10);
            }

            return Overlay;
        }

        private void MoveCursor(System.Drawing.Point loc)
        {
            // ***To Do*** 
            // Need to add up down left right and get this to make educated guesses based on current position and target direction.
            Cursor.Position = loc;
        }

        private System.Drawing.Image ProcessBlobs(Bitmap image, Bitmap ProcessedImage, Input input)
        {
            Bitmap ResultImage;                                     // our return image variable
            // Choose which view to overlay our data on
            if (NoFilters)
            {
                ResultImage = (Bitmap)image.Clone();
            }
            else
            {
                ResultImage = new Bitmap(xMax / 2,yMax);
            }

            // Create the blob counter and get the blob info array for further processing
            BlobCounter blobCounter = new BlobCounter();
            // We *COULD* filter blobs here, but as pointed out that blocks the ability to eventually twist the hand/fingers horizontally.
            //blobCounter.FilterBlobs = true;
            //blobCounter.MinHeight = minHeight;
            //blobCounter.MaxWidth = maxWidth;
            blobCounter.ProcessImage(ProcessedImage);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            GrahamConvexHull hullFinder = new GrahamConvexHull();

            // Create graphics control to draw in the picture
            Graphics g = Graphics.FromImage(ResultImage);

            // Label the camfeeds just to prove this works right...
            g.DrawString(input.ToString(), new Font("Arial", 16), new SolidBrush(Color.Blue), new PointF(0, 0));

            // process each blob
            foreach (Blob blob in blobs)
            {
                if (CheckBlob(blob))
                {
                    List<IntPoint> leftPoints, rightPoints;
                    List<IntPoint> edgePoints = new List<IntPoint>();

                    // get blob's edge points
                    blobCounter.GetBlobsLeftAndRightEdges(blob,
                        out leftPoints, out rightPoints);

                    edgePoints.AddRange(leftPoints);
                    edgePoints.AddRange(rightPoints);

                    // calculate the blob's convex hull
                    List<IntPoint> hull = hullFinder.FindHull(edgePoints);

                    // Calculate depth
                    int pix = (int)CvInvoke.cvGet2D(disparity, blob.Rectangle.Top, ((blob.Rectangle.Left + blob.Rectangle.Right) / 2)).v0;

                    // Draw the blob hull and id it with the width/height
                    g.DrawPolygon(new Pen(Color.Blue), IntPointsToPointFs(hull.ToArray()));
                    string coord = "";
                    coord += blob.Rectangle.Width.ToString() + ",";         // X
                    coord += blob.Rectangle.Height.ToString() + ",";        // Y
                    coord += pix.ToString();                              // Z
                    g.DrawString(coord, new Font("Arial", 16), new SolidBrush(Color.Blue), new PointF(hull[0].X, hull[0].Y));

                    // This next line is all we should need once done debugging/designing. Toss the image manipulation.
                    hand.AddFinger(new System.Drawing.Point(((blob.Rectangle.Left + blob.Rectangle.Right) / 2), blob.Rectangle.Top), pix, (Hand.Input)input);
                }
            }

            return ResultImage;
        }

        private bool CheckBlob(Blob blob)
        {
            // The running idea is that we want a specific size blob, about the length and width of a finger. However if our filtering and thresholds are off this fails.
            if (blob.Rectangle.Width < maxWidth)
            {
                if (blob.Rectangle.Height > minHeight)
                {
                    return true;
                }
            }

            if (blob.Rectangle.Height < maxWidth)
            {
                if (blob.Rectangle.Width > minHeight)
                {
                    return true;
                }
            }

            return false;
        }

        private System.Drawing.Point[] IntPointsToPointFs(IntPoint[] intPoint)
        {
            System.Drawing.Point[] result = new System.Drawing.Point[intPoint.GetUpperBound(0)];
            for (int a = 0; a < intPoint.GetUpperBound(0); a++)
            {
                result[a].X = intPoint[a].X;
                result[a].Y = intPoint[a].Y;
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Let's reset the background image...
            background = null;
            FinalVideoSource.NewFrame -= new NewFrameEventHandler(FinalVideoSource_NewFrame);
            FinalVideoSource.NewFrame += new NewFrameEventHandler(FinalVideoSource_NewFrame);
        }

        public void SetThreshold(int val)
        {
            ThresVal = val;
            numericUpDown1.Value = val;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ThresVal = (int)numericUpDown1.Value;
        }

        private void minHeightUD_ValueChanged(object sender, EventArgs e)
        {
            minHeight = (int)minHeightUD.Value;
        }

        private void maxWidthUD_ValueChanged(object sender, EventArgs e)
        {
            maxWidth = (int)maxWidthUD.Value;
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            minHeightUD.Value = minHeight;
            maxWidthUD.Value = maxWidth;
        }

        public void SetFilters(bool value)
        {
            NoFilters = value;
        }
    }
}

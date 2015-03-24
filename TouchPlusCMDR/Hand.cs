using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchPlusCMDR
{
    class Hand
    {
        public enum Input { Left, Right };                                                                                 // Define a type to define left or right input image

        List<System.Drawing.Point> LeftPoints = new List<System.Drawing.Point>();
        List<System.Drawing.Point> RightPoints = new List<System.Drawing.Point>();
        List<int> DepthL = new List<int>();
        List<int> DepthR = new List<int>();
        private Boolean DataChecked = false;

        public void ClearFingers()
        {
            // Each frame we need to re-clear the previous fingers / Maybe eventually keep a couple sets to build a running average of where they "should" be
            // Regardless reset our datachecked flag
            LeftPoints.Clear();
            RightPoints.Clear();
            DepthL.Clear();
            DepthR.Clear();
            DataChecked = false;
        }

        public void AddFinger(System.Drawing.Point point, int depth, Input input)
        {
            if (input == Input.Left) 
            {
                DepthL.Add(depth);
                LeftPoints.Add(point);
            }
            else if (input == Input.Right) 
            {
                DepthR.Add(depth);
                RightPoints.Add(point);
            }
        }

        public void CheckAndDiscard()
        {
            // ***To Do***
            // Needs to compare the left and right input lists and discard garbage that doesn't seem to correlate to the others
            // Then keep only matched sets?
            // For now, I'll just make certain they have the same number of points and drop any unmatched points from the end.
            while (LeftPoints.Count != RightPoints.Count)
            {
                if (LeftPoints.Count > RightPoints.Count)
                {
                    DepthL.RemoveAt(DepthL.Count - 1);
                    LeftPoints.RemoveAt(LeftPoints.Count - 1);
                }
                else if (RightPoints.Count > LeftPoints.Count)
                {
                    DepthR.RemoveAt(DepthR.Count - 1);
                    RightPoints.RemoveAt(RightPoints.Count - 1);
                }
            }
            DataChecked = true;                                                         // Mark that data has been sanitized and can now be pulled
        }

        public int FingerCount()
        {
            if (DataChecked)
            {
                return LeftPoints.Count;
            }
            return 0;
        }

        public System.Drawing.Point GetFinger(int num)
        {
            if (DataChecked)
            {
                if (LeftPoints.Count >= (num + 1))                                      // Check that we have at least that many finger points available
                {
                    // We average the points together (which should be correlated) and return
                    return AveragePoint(LeftPoints[num], RightPoints[num]);
                }
                else
                {
                    return (new System.Drawing.Point(0, 0));                            // Return a 0,0 point indicating we haven't yet checked data
                }
            }
            else
            {
                return (new System.Drawing.Point(0, 0));                                // Return a 0,0 point indicating we haven't yet checked data
            }
        }

        public System.Drawing.Point GetAveragePosition()
        {
            if (DataChecked)
            {
                int tx = 0;
                int ty = 0;
                for (int a = 0; a < LeftPoints.Count; a++)
                {
                    tx = tx + LeftPoints[a].X + RightPoints[a].X;
                    ty = ty + LeftPoints[a].Y + RightPoints[a].Y;
                }
                return (new System.Drawing.Point((tx / (LeftPoints.Count * 2)),(ty / (LeftPoints.Count * 2))));
            }
            else
            {
                return (new System.Drawing.Point(0, 0));                                // Return a 0,0 point indicating we haven't yet checked data
            }
        }

        private System.Drawing.Point AveragePoint(System.Drawing.Point pointL, System.Drawing.Point pointR)
        {
            int x = (pointL.X + pointR.X) / 2;
            int y = (pointL.Y + pointR.Y) / 2;

            return (new System.Drawing.Point(x,y));
        }
    }
}

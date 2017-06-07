using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EmbroidObot.Model;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace EmbroidObot.ViewModel
{
    public class DstDisplayViewModel
    {
        private double dstWindowHeight = 1000;
        private double dstWindowWidth = 1000;
        private double xShift = 0;
        private double yShift = 0;
        private double xScaleFactor = 1;
        private double yScaleFactor = 1;
        private double maxX;
        private double maxY;

        //getter
        public double MaxX
        {
            get
            {
                return (maxX + xShift) * xScaleFactor;
            }
        }

        //getter
        public double MaxY
        {
            get
            {
                return dstWindowHeight - ((maxY + yShift) * yScaleFactor);
            }
        }

        //getter
        public double MinX
        {
            get
            {
                return 0;
            }
        }

        //getter
        public double MinY
        {
            get
            {
                return dstWindowHeight;
            }
        }


        //getter/setter
        public DisplayField DisplayDstWindow
        {
            get;
            set;
        }


        //default contructor
        public void DisplayField()
        {
            DisplayField displayField = new DisplayField();
            displayField.Height = dstWindowHeight;
            displayField.Width = dstWindowWidth;
            displayField.PositionLeft = 200;
            displayField.PositionTop = 10;
            DisplayDstWindow = displayField;
        }

        //getter/setter
        public ObservableCollection<DstLine> DstLines
        {
            get;
            set;
        }

        //loads dst lines and draws them in the display area
        public void LoadDstLines(ObservableCollection<DstLine> lines) {

            DstLines = new ObservableCollection<DstLine>();

            ObservableCollection<DstLine> dstLines = new ObservableCollection<DstLine>();

            FindXYshift(lines);
            FindLineScaleFactor(lines);

            foreach (DstLine line in lines)
            {
                dstLines.Add(new DstLine { StartX = (line.StartX + xShift) * xScaleFactor, StartY = DisplayDstWindow.Height - ((line.StartY + yShift) * yScaleFactor), EndX = (line.EndX + xShift) * xScaleFactor, EndY = DisplayDstWindow.Height - ((line.EndY + yShift) * yScaleFactor) });
            }

            foreach (DstLine line in dstLines)
            {

                line.Colour = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }

            DstLines = dstLines;
        }

        public void UpdateLineColour(SolidColorBrush b)
        {
            foreach (DstLine line in DstLines)
            {
                line.Colour = b;
            }

            
        }

        //aligns the origin of the dst drawing to zero
        private void FindXYshift(ObservableCollection<DstLine> lines)
        {
            double minX = 0;
            if (lines.Count > 0)
            {
                minX = lines[0].StartX;
            }

            foreach (DstLine line in lines)
            {
                if (line.StartX < minX)
                {
                    minX = line.StartX;
                }
                if (line.EndX < minX)
                {
                    minX = line.EndX;
                }
            }

            xShift = 0 - minX;

            double minY = 0;
            if (lines.Count > 0)
            {
                minY = lines[0].StartY;
            }

            foreach (DstLine line in lines)
            {
                if (line.StartY < minY)
                {
                    minY = line.StartY;
                }
                if (line.EndY < minY)
                {
                    minY = line.EndY;
                }
            }

            yShift = 0 - minY;

        }

        //scales dst drawing to fill the display window area
        private void FindLineScaleFactor(ObservableCollection<DstLine> lines)
        {
            
            if (lines.Count > 0)
            {
                maxX = lines[0].StartX;
            }

            foreach (DstLine line in lines)
            {
                if (line.StartX > maxX)
                {
                    maxX = line.StartX;
                }
                if (line.EndX > maxX)
                {
                    maxX = line.EndX;
                }
            }

            xScaleFactor = dstWindowWidth / (maxX + xShift);

            
            if (lines.Count > 0)
            {
                maxY = lines[0].StartY;
            }

            foreach (DstLine line in lines)
            {
                if (line.StartY > maxY)
                {
                    maxY = line.StartY;
                }
                if (line.EndY > maxY)
                {
                    maxY = line.EndY;
                }
            }

            yScaleFactor = dstWindowHeight / (maxY + yShift);

            if (yScaleFactor < xScaleFactor)
            {
                xScaleFactor = yScaleFactor;
            }
            else
            {
                yScaleFactor = xScaleFactor;
            }



        }

    } 
}

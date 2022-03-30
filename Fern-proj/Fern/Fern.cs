/*
 * CS 212 : Project 3 - Fern
 * Seong Chan Cho (sc77)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FernNamespace
{
    /*
     * this class draws a fractal fern when the constructor is called.
     * Written as sample C# code for a CS 212 assignment -- October 2011.
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */   
    class Fern
    {
        private static int BERRYMIN = 2;
        private static int TENDRILMIN = 1;
        private static double DELTATHETA = 0.05;
        private static double SEGLENGTH = 2.0;
        private static int BIAS = 1;
        private static Random r = new Random();
        private string[] bground = new string[] { "C:/Users/sc77/Desktop/Fern-proj/Fern/bin/Debug/image1.jpg", "C:/Users/sc77/Desktop/Fern-proj/Fern/bin/Debug/image2.jpg", "C:/Users/sc77/Desktop/Fern-proj/Fern/bin/Debug/image3.jpg", "C:/Users/sc77/Desktop/Fern-proj/Fern/bin/Debug/image4.jpg", "C/Users/sc77/Desktop/Fern-proj/Fern/bin/Debug/image5.jpg" };
        



        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern is drwan, implemented a random background feature to allow background to change each time it runs. 
         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {

           int randomBackground = r.Next(0, 4);

            ImageBrush myImageBrush = new ImageBrush();
            myImageBrush.ImageSource = new BitmapImage(new Uri(bground[randomBackground], UriKind.Relative));
            canvas.Background = myImageBrush;
         
            canvas.Children.Clear();                                // delete old canvas contents
                                                                    // draw a new fern with given parameters
            tendril((int)(canvas.Width / 2), (int)(canvas.Height), size / 1.5, redux, turnbias, Math.PI, canvas);
        }



        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, double direction, int BIAS, Canvas canvas)
        {
            // compute the angle of the outgoing tendril
            double theta = (Math.PI * 7 * r.NextDouble() / 180) - Math.PI * 80 / 180;
            tendril(x, y, size, redux, turnbias, direction, canvas);
            // draw left or right tendril depending on the parameter "BIAS" value, 
            if (BIAS == 1)
            {
                tendril(x, y, size / 1.3, redux, turnbias, direction - theta, canvas);
            }
 
            else
            {
                tendril(x, y, size / 1.3, redux, turnbias, direction + theta, canvas);
            }
        }

        /*
         * tendril draws a tendril in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         */
        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            double directions = (r.NextDouble() > turnbias) ? -1 * DELTATHETA : DELTATHETA;
            for (int i = 0; i < size; i++)
            {
                direction += directions * DELTATHETA;
                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
                byte red = (byte)(100 + size / 2);
                byte green = (byte)(220 - size / 3);
                //if (size>120) red = 138; green = 108;
                line(x1, y1, x2, y2, red, green, 0, 2 + size / 80, canvas);
            }
            BIAS = BIAS * -1;
            if (size > TENDRILMIN)
                cluster(x2, y2, size / redux, redux, turnbias, direction, BIAS, canvas);
            if (size < BERRYMIN)
                berry(x2, y2, 3, canvas);
        }

        /*
         * draw a  circle centered at (x,y) onto canvas
         * set color of each berries to random color.
         * Making it look realistic as possible having same color, but simillar color to one another.
         */
        private void berry(int x, int y, double radius, Canvas canvas )
        {
            int rand_int_for_color1 = r.Next(300);
            int rand_int_for_color2 = r.Next(200);
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(200, (byte)rand_int_for_color1, 30, (byte)rand_int_for_color2) ; 
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = 2 * (radius);
            myEllipse.Height = 2 * radius;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 100, 255,100);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);
        }
    }
}

        

/*
 * this class is needed to enable us to set the center for an ellipse
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}


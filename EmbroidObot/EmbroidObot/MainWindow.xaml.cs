using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using System.IO.Ports;
using EmbroidObot.Model;
using System.Drawing;
using System.Windows.Forms;




namespace EmbroidObot
{
    public partial class MainWindow : Window
    {

        private EmbroidObot.ViewModel.MainWindowViewModel mainWindowViewModelObject = new ViewModel.MainWindowViewModel();

        public MainWindow()
        {
            this.DataContext = mainWindowViewModelObject;
            InitializeComponent();
        }



        //Loads the field that displays the opened Dst drawing
        private void DisplayViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            EmbroidObot.ViewModel.DstDisplayViewModel dstDisplayViewModelObject = new EmbroidObot.ViewModel.DstDisplayViewModel();
            dstDisplayViewModelObject.DisplayField();
            DstDisplayViewControl.DataContext = dstDisplayViewModelObject;
        }

        public IList<StitchTajima> stitches = new List<StitchTajima>();

        public void OpenDst(string fileName)
        {
            stitches = new List<StitchTajima>();

            byte[] fileBytes = File.ReadAllBytes(fileName);
            StringBuilder sb = new StringBuilder();

            IList<string> bytes = new List<string>();

            foreach (byte b in fileBytes)
            {
                bytes.Add((Convert.ToString(b, 2).PadLeft(8, '0')));
            }

            for (int i = 512; i < bytes.Count(); i = i + 3)
            {
                if (i + 2 <= bytes.Count())
                {
                    stitches.Add(new StitchTajima(bytes[i], bytes[i + 1], bytes[i + 2]));
                }
            }
        }

        //sends the GCode to the printer if a Dst file has been loaded
        public void SendStitches_Click(object sender, RoutedEventArgs e)
        {

            

            if (mainWindowViewModelObject.ActiveDst != null)
            {
                SendStitches();
            }
        }

        public void SendStitches()
        {
            SerialPort port = new SerialPort(
                  mainWindowViewModelObject.SelectedPort, mainWindowViewModelObject.SelectedBaudRate, Parity.None, 8, StopBits.One);


            try
            {
                port.Open();//opens serial port

                int i = 0;
                foreach (StitchTajima stitch in stitches)
                {


                    int x = stitch.XPath + 128;
                    int y = stitch.YPath + 128;


                    byte[] test = new byte[] { (byte)(x), (byte)(y), (byte)stitch.JumpStitch, (byte)stitch.ColourChange };


                    string output = (test[0] -128) + ":" + (test[1]-128) + ":" + test[2] + ":" + test[3];



                    port.Write(test,0,4) ;

                    Console.WriteLine(output + "   " + port.ReadLine());

                }

                    

                bool b = true;

                System.Windows.Forms.MessageBox.Show("success", "success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                //Console.WriteLine(port.ReadLine());


                port.Close();




            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void SendTest()
        {
            SerialPort port = new SerialPort(mainWindowViewModelObject.SelectedPort, mainWindowViewModelObject.SelectedBaudRate, Parity.None, 8, StopBits.One);


            try
            {
                port.Open();//opens serial port

                int i = 0;

                byte[] test = new byte[] { (byte)0, (byte)0, (byte)0, (byte)0 };                    

                port.Write(test, 0, 4);

                Console.WriteLine(port.ReadLine());

                bool b = true;

                System.Windows.Forms.MessageBox.Show("success", "success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                //Console.WriteLine(port.ReadLine());


                port.Close();




            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ViewModel.DstDisplayViewModel dstDisplayViewModelObject;

        private void OpenDst_Click(object sender, RoutedEventArgs e)
        {


            OpenFileDialog openDstDialog = new OpenFileDialog();

            openDstDialog.InitialDirectory = "c:\\";
            openDstDialog.Filter = "DST files (*.dst)|*.dst";
            openDstDialog.FilterIndex = 1;
            openDstDialog.RestoreDirectory = true;

            if (openDstDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OpenDst(openDstDialog.FileName);

                dstDisplayViewModelObject = new ViewModel.DstDisplayViewModel();
                dstDisplayViewModelObject.DisplayField();
                dstDisplayViewModelObject.LoadDstLines(CreateLines(stitches));

                DstDisplayViewControl.DataContext = dstDisplayViewModelObject;

                mainWindowViewModelObject.UpdateDst(openDstDialog.FileName);
                mainWindowViewModelObject.FileName = null;
            }
        }

        private ObservableCollection<DstLine> CreateLines(IList<StitchTajima> stitchLines)
        {
            ObservableCollection<DstLine> lines = new ObservableCollection<DstLine>();

            DstLine line = new DstLine();
            line.StartX = 0;
            line.StartY = 0;
            line.EndX = stitchLines[0].XPath;
            line.EndY = stitchLines[0].YPath;

            lines.Add(line);


            for (int i = 1; i < stitchLines.Count; i++)
            {
                DstLine l = new DstLine();

                l.StartX = lines[i - 1].EndX;
                l.StartY = lines[i - 1].EndY;
                l.EndX = lines[i - 1].EndX + stitchLines[i].XPath;
                l.EndY = lines[i - 1].EndY + stitchLines[i].YPath;

                lines.Add(l);
            }



            return lines;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SendTest();

            //System.Windows.Forms.ColorDialog colorDialog1 = new System.Windows.Forms.ColorDialog();


            //// Show the color dialog.
            //DialogResult result = colorDialog1.ShowDialog();
            //// See if user pressed ok.
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    var wpfColor = System.Windows.Media.Color.FromArgb(colorDialog1.Color.A, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);

            //    var brush = new SolidColorBrush(wpfColor);

            //    dstDisplayViewModelObject.UpdateLineColour(brush);

            //}
        }

    }

    public class StitchTajima
    {
        private int xPath;
        private int yPath;
        private int jumpStitch;
        private int colourChange;

        public StitchTajima(string b1, string b2, string b3)
        {
            yPath = 0;
            yPath += Convert.ToInt32(b1[0]) * 1;
            yPath += Convert.ToInt32(b1[1]) * -1;
            yPath += Convert.ToInt32(b1[2]) * 9;
            yPath += Convert.ToInt32(b1[3]) * -9;
            yPath += Convert.ToInt32(b2[0]) * 3;
            yPath += Convert.ToInt32(b2[1]) * -3;
            yPath += Convert.ToInt32(b2[2]) * 27;
            yPath += Convert.ToInt32(b2[3]) * -27;
            yPath += Convert.ToInt32(b3[2]) * 81;
            yPath += Convert.ToInt32(b3[3]) * -81;

            xPath = 0;
            xPath += Convert.ToInt32(b1[4]) * -9;
            xPath += Convert.ToInt32(b1[5]) * 9;
            xPath += Convert.ToInt32(b1[6]) * -1;
            xPath += Convert.ToInt32(b1[7]) * 1;
            xPath += Convert.ToInt32(b2[4]) * -27;
            xPath += Convert.ToInt32(b2[5]) * 27;
            xPath += Convert.ToInt32(b2[6]) * -3;
            xPath += Convert.ToInt32(b2[7]) * 3;
            xPath += Convert.ToInt32(b3[4]) * -81;
            xPath += Convert.ToInt32(b3[5]) * 81;

            if (Convert.ToInt32(b3[0]) == '1')
            {
                jumpStitch = 1;
            }
            else
            {
                jumpStitch = 0;
            }

            if (Convert.ToInt32(b3[1]) == '1')
            {
                colourChange = 1;
            }
            else
            {
                colourChange = 0;
            }
        }

        public int XPath
        {
            get
            {
                return xPath;
            }
        }
        public int YPath
        {
            get
            {
                return yPath;
            }
        }
        public int JumpStitch
        {
            get
            {
                return jumpStitch;
            }
        }
        public int ColourChange
        {
            get
            {
                return colourChange;
            }
        }


    }




}
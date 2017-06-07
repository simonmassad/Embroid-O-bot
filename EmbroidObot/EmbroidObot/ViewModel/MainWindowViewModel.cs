using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmbroidObot.Model;
using System.ComponentModel;
using System.IO.Ports;

namespace EmbroidObot.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //the dst that the user has opened
         public CurrentDstModel ActiveDst
         {
             get;
             set;
         }

        //placeholder text for the dst filename display
         private string fileName = "Current Dst: ";

        //dst file name
         public string FileName
         {
             get
             {
                 return fileName;
             }
             set
             {
                 RaisePropertyChanged("fileName");
             }
         }

        //alerts the view that specified properties have changed
         public event PropertyChangedEventHandler PropertyChanged;

         private void RaisePropertyChanged(string property)
         {
             if (PropertyChanged != null)
             {
                 PropertyChanged(this, new PropertyChangedEventArgs(property));
             }
         } 

        //draws new dst that the user has selected
         public void UpdateDst(string dstFileName)
         {
             CurrentDstModel activeDst = new CurrentDstModel();


             activeDst.Dst = dstFileName;
             ActiveDst = activeDst;
         }


        //placeholder for user selected printer baud rate
         private int selectedBaudRate = 0;

        //placeholder for user selected com port
         private string selectedPort;

         //SelectedBaudRate getter/setter
         public int SelectedBaudRate
         {
             get 
             { 
                 return selectedBaudRate; 
             }
             set
             {
                 selectedBaudRate = value;
             }
         }
        //selected port getter/setter
         public string SelectedPort
         {
             get
             {
                 return selectedPort;
             }
             set
             {
                 selectedPort = value;
             }
         }

        //possible baud rates
         private int[] baudRateOptions = { 2400, 9600, 19200, 38400, 57600, 115200, 250000 };
        //gets all active serial ports
         private string[] activePorts = SerialPort.GetPortNames();

        // getter/setter
         public int[] BaudRateOptions
         {
             get
             {
                 return baudRateOptions;
             }
             set
             {
                 baudRateOptions = value;
             }
         }

         // getter/setter
         public string[] ActivePorts
         {
             get
             {
                 return activePorts;
             }
             set
             {
                 activePorts = value;
             }
         }
         
    }
}

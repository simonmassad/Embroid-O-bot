using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EmbroidObot.Model
{
    public class CurrentDstModel : INotifyPropertyChanged 
    {

         //selected dst
        private string dst;
        
        //  getter/setter
        public string Dst
        {
            get
            {
                return dst;
            }
            set
            {
                dst = value;
                RaisePropertyChanged("CurrentDst");
                RaisePropertyChanged("CurrentFileName");
            }
        }
        //current file name
        public string currentFileName = "Current Dst: ";
        //getter/setter
        public string CurrentFileName
        {
            get
            {
                return "Current Dst: " + dst + ".dst";//file name formatting with .dst extention
            }
            set
            {
                CurrentFileName = value;
                RaisePropertyChanged("CurrentFileName");
            }
        }
        //alerts view that proprties have changed
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }

}

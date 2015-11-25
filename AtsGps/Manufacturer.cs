using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AtsGps {

 
    public class Manufacturer : INotifyPropertyChanged {
        //----------------------------------------------Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //----------------------------------------------Initialize
        private String name;
        private List<String> devices;
        private String device;
        //----------------------------------------------Getter/Setter
        public String Name {
            get {
                return name;
            }
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public String Device {
            get {
                return device;
            }
            set {
                device = value;
                OnPropertyChanged("Device");
            }
        }
        public List<String> Devices {
            get {
                return devices;
            }
            set {
                devices = value;
                OnPropertyChanged("Devices");
            }
        }
        //----------------------------------------------Function
        public override string ToString () {
            return this.Name + " - " + this.Device;
        }


    }


}

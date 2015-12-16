using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AtsGps {

 
    public class Device : INotifyPropertyChanged {
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
        private String company;
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
        public String Company {
            get {
                return company;
            }
            set {
                company = value;
                OnPropertyChanged("Company");
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
            String stringName = this.Company + "|" + this.Name;
            return stringName;
        }
    }


}

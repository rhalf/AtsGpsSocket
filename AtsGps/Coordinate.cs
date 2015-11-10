using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps {
    public class Coordinate {

        Double Latitude {
            get; set;
        }
        Double Longitude {
            get; set;
        }

        public Coordinate (Double Latitude, Double Longitude) {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

        public override string ToString () {
            return "{" + this.Latitude.ToString() + "," + this.Longitude.ToString() + "}";
        }
    }
}
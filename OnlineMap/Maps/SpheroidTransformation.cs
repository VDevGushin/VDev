using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class SpheroidTransformation : MercatorTransformation
    {
        public override double GetTileX(double longitude)
        {
            return 0.5 + longitude / 360.0;
        }

        public override double GetTileY(double latitude)
        {
            return 0.5 * (1 - ((Math.Log((1 + Math.Sin(latitude * Math.PI / 180.0)) / (1 - Math.Sin(latitude * Math.PI / 180.0)))) / 2.0) / Math.PI);
        }

        public override double GetLongitude(double tileX)
        {
            return (tileX - 0.5) * 360.0;
        }

        public override double GetLatitude(double tileY)
        {
            bool S = false;
            if (tileY > 0.5)
            {
                S = true;
                tileY = 1 - tileY;
            }
            double y1 = (1.0 - (tileY * 2)) * Math.PI;
            double teta0 = 2.0 * Math.Atan(Math.Pow(Math.E, y1)) - Math.PI / 2.0;

            return (S ? -1 : 1) * teta0 * 180.0 / Math.PI;
        }
    }
}

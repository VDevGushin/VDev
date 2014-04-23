using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class EllipsoidTransformation : MercatorTransformation
    {
        /// <summary>
        /// Ellips a-radius
        /// </summary>
        public double RadiusA { get; set; }

        /// <summary>
        /// Ellips b-radius
        /// </summary>
        public double RadiusB { get; set; }

        private const double precision = 0.0000001;

        public EllipsoidTransformation(double radiusA, double radiusB)
        {
            RadiusA = radiusA;
            RadiusB = radiusB;
        }

        /// <summary>
        /// Default constructor using Krassovsky ellipsoid params (a=6378137.0, b=6356752.0)
        /// </summary>
        public EllipsoidTransformation()
            : this(6378137.0, 6356752.0)
        { }

        public override double GetTileX(double longitude)
        {
            return 0.5 + longitude / 360.0;
        }

        public override double GetTileY(double latitude)
        {
            double e = Math.Sqrt(Math.Abs(RadiusA - RadiusB) * (RadiusA + RadiusB)) / Math.Max(RadiusA, RadiusB);
            return 0.5 * (1 - ((Math.Log((1 + Math.Sin(latitude * Math.PI / 180.0)) / (1 - Math.Sin(latitude * Math.PI / 180.0)))) / 2.0 - e * (Math.Log((1 + e * Math.Sin(latitude * Math.PI / 180.0)) / (1 - e * Math.Sin(latitude * Math.PI / 180.0)))) / 2.0) / Math.PI);
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
            double y1 = (1.0 - tileY * 2) * Math.PI;
            double teta0 = 2.0 * Math.Atan(Math.Pow(Math.E, y1)) - Math.PI / 2.0;

            double e = Math.Sqrt(Math.Abs(RadiusA - RadiusB) * (RadiusA + RadiusB)) / Math.Max(RadiusA, RadiusB);
            double teta = CalcTeta(teta0, y1, e);
            while (Math.Abs(teta - teta0) > precision)
            {
                teta0 = teta;
                teta = CalcTeta(teta, y1, e);
            }
            return (S ? -1 : 1) * teta * 180.0 / Math.PI;
        }

        private double CalcTeta(double teta0, double y1, double e)
        {
            return Math.Asin(1.0 - (1.0 + Math.Sin(teta0)) * Math.Pow(1.0 - e * Math.Sin(teta0), e) / (Math.Pow(Math.E, 2.0 * y1 / 1.0) * Math.Pow(1 + e * Math.Sin(teta0), e)));
        }
    }
}

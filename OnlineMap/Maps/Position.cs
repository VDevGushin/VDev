using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    [TypeConverter(typeof(PositionConverter))]
    public struct Position
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Position(double longitude, double latitude)
            : this()
        {
            if (latitude < -90.0 || latitude > 90.0)
            {
                throw new ArgumentOutOfRangeException(ExceptionStrings.Position_LatitudeOutOfRange);
            }
            Longitude = NormalizeLongitude(longitude);
            Latitude = latitude;
        }

        /// <summary>
        /// Returns string representation of longitude
        /// </summary>
        /// <param name="f">Coordinate format</param>
        /// <returns></returns>
        public string LongitudeToString(CoordinateFormat f)
        {
            return LongitudeToString(Longitude, f);
        }

        /// <summary>
        /// Returns string representation of latitude
        /// </summary>
        /// <param name="f">Coordinate format</param>
        /// <returns></returns>
        public string LatitudeToString(CoordinateFormat f)
        {
            return LatitudeToString(Latitude, f);
        }

        /// <summary>
        /// Returns string representation of geografic position
        /// </summary>
        /// <param name="pf">Coordinate order</param>
        /// <param name="cf">Coordinate format</param>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(Longitude, Latitude, PositionFormat.LonLat, CoordinateFormat.SDD_DDDDDD);
        }

        /// <summary>
        /// Returns string representation of geografic position
        /// </summary>
        /// <param name="pf">Coordinate order</param>
        /// <param name="cf">Coordinate format</param>
        /// <returns></returns>
        public string ToString(PositionFormat pf, CoordinateFormat cf)
        {
            return ToString(Longitude, Latitude, pf, cf);
        }

        /// <summary>
        /// Returns string representation of longitude
        /// </summary>
        /// <param name="f">Geo coordinate format</param>
        /// <returns></returns>
        public static string LongitudeToString(double longitude, CoordinateFormat f)
        {
            double l = Math.Abs(longitude);
            switch (f)
            {
                case CoordinateFormat.SDD_DDDDDD:
                    return String.Format("{0}{1}°", ((longitude < 0.0) ? "-" : ""), Math.Round(l, 6).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.SDD_MMMMMM:
                    return String.Format("{0}{1}°{2}'", ((longitude < 0.0) ? "-" : ""), ((int)l).ToString(CultureInfo.CurrentCulture), Math.Round((l - (double)(int)l) * 60, 4).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.SDD_MM_SSSS:
                    return String.Format("{0}{1}°{2}'{3}\"", ((longitude < 0.0) ? "-" : ""), ((int)l).ToString(CultureInfo.CurrentCulture), ((int)((l - (double)(int)l) * 60)).ToString(CultureInfo.CurrentCulture), Math.Round((((l - (double)(int)l) * 60) - (double)(int)((l - (double)(int)l) * 60)) * 60, 2).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_DDDDDD:
                    return String.Format("{0}{1}°", ((longitude < 0.0) ? "W" : "E"), Math.Round(l, 6).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_MMMMMM:
                    return String.Format("{0}{1}°{2}'", ((longitude < 0.0) ? "W" : "E"), ((int)l).ToString(CultureInfo.CurrentCulture), Math.Round((l - (double)(int)l) * 60, 4).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_MM_SSSS:
                    return String.Format("{0}{1}°{2}'{3}\"", ((longitude < 0.0) ? "W" : "E"), ((int)l).ToString(CultureInfo.CurrentCulture), ((int)((l - (double)(int)l) * 60)).ToString(CultureInfo.CurrentCulture), Math.Round((((l - (double)(int)l) * 60) - (double)(int)((l - (double)(int)l) * 60)) * 60, 2).ToString(CultureInfo.CurrentCulture));
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns string representation of latitude
        /// </summary>
        /// <param name="f">Geo coordinate format</param>
        /// <returns></returns>
        public static string LatitudeToString(double latitude, CoordinateFormat f)
        {
            double l = Math.Abs(latitude);
            switch (f)
            {
                case CoordinateFormat.SDD_DDDDDD:
                    return String.Format("{0}{1}°", ((latitude < 0.0) ? "-" : ""), Math.Round(l, 6).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.SDD_MMMMMM:
                    return String.Format("{0}{1}°{2}'", ((latitude < 0.0) ? "-" : ""), ((int)l).ToString(CultureInfo.CurrentCulture), Math.Round((l - (double)(int)l) * 60, 4).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.SDD_MM_SSSS:
                    return String.Format("{0}{1}°{2}'{3}\"", ((latitude < 0.0) ? "-" : ""), ((int)l).ToString(CultureInfo.CurrentCulture), ((int)((l - (double)(int)l) * 60)).ToString(CultureInfo.CurrentCulture), Math.Round((((l - (double)(int)l) * 60) - (double)(int)((l - (double)(int)l) * 60)) * 60, 2).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_DDDDDD:
                    return String.Format("{0}{1}°", ((latitude < 0.0) ? "S" : "N"), Math.Round(l, 6).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_MMMMMM:
                    return String.Format("{0}{1}°{2}'", ((latitude < 0.0) ? "S" : "N"), ((int)l).ToString(CultureInfo.CurrentCulture), Math.Round((l - (double)(int)l) * 60, 4).ToString(CultureInfo.CurrentCulture));
                case CoordinateFormat.LDD_MM_SSSS:
                    return String.Format("{0}{1}°{2}'{3}\"", ((latitude < 0.0) ? "S" : "N"), ((int)l).ToString(CultureInfo.CurrentCulture), ((int)((l - (double)(int)l) * 60)).ToString(CultureInfo.CurrentCulture), Math.Round((((l - (double)(int)l) * 60) - (double)(int)((l - (double)(int)l) * 60)) * 60, 2).ToString(CultureInfo.CurrentCulture));
                default:
                    return "";
            }
        }

        /// <summary>
        /// Returns string representation of geografic position
        /// </summary>
        /// <param name="pf">Coordinate order</param>
        /// <param name="cf">Coordinate format</param>
        /// <returns></returns>
        public static string ToString(double longitude, double latitude, PositionFormat pf, CoordinateFormat cf)
        {
            switch (pf)
            {
                case PositionFormat.LonLat:
                    return String.Format("{0} {1}", LongitudeToString(longitude, cf), LatitudeToString(latitude, cf));
                case PositionFormat.LatLon:
                    return String.Format("{0} {1}", LatitudeToString(latitude, cf), LongitudeToString(longitude, cf));
                default:
                    return "";
            }
        }

        /// <summary>
        /// Normalizes values less than -180.0 and greater then 180.0
        /// </summary>
        /// <param name="longitude"></param>
        /// <returns>Normalized longitude</returns>
        private static double NormalizeLongitude(double longitude)
        {
            if (longitude <= 180.0 && longitude >= -180.0)
            {
                return longitude;
            }
            return longitude - Math.Floor(longitude / 360.0) * 360.0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Position)obj);
        }

        public bool Equals(Position other)
        {
            return Longitude == other.Longitude && Latitude == other.Latitude;
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
   public static class ExceptionStrings
    {
       public static string PositionConverter_InvalidPositionFormat = "Invalid position format. Pair of doubles required, representing longitude and latitude.";
       public static string Position_LatitudeOutOfRange = "Latitude must be between -90.0 and 90.0";
    }
}

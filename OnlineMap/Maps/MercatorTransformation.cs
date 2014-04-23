using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public abstract class MercatorTransformation
    {
        /// <summary>
        /// Transforms longitude into tile's logic x-coordinate (0 &lt;= x &lt;= 1)
        /// </summary>
        /// <param name="longitude">Longitude</param>
        /// <returns>Tile's x-coordinate</returns>
        public abstract double GetTileX(double longitude);

        /// <summary>
        /// Transforms latitude into tile's logic y-coordinate (0 &lt;= y &lt;= 1)
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <returns>Tile's y-coordinate</returns>
        public abstract double GetTileY(double latitude);

        /// <summary>
        /// Transforms tile's logic x-coordinate into longitude
        /// </summary>
        /// <param name="tileX">Logic X-coordinate (0 &lt;= x &lt;= 1)</param>
        /// <returns>Double representing longitude</returns>
        public abstract double GetLongitude(double tileX);

        /// <summary>
        /// Transforms tile's logic y-coordinate into longitude
        /// </summary>
        /// <param name="tileY">Logic Y-coordinate (0 &lt;= x &lt;= 1)</param>
        /// <returns>Double representing latitude</returns>
        public abstract double GetLatitude(double tileY);
    }
}

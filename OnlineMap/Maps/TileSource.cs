using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public abstract class TileSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <param name="zoom"></param>
        /// <returns>Tile's URI</returns>
        public abstract object GetTile(int tileX, int tileY, int zoom);
        /// <summary>
        /// Layer's Mercator transformation
        /// </summary>
        public abstract MercatorTransformation Transformation { get; }
    }
}

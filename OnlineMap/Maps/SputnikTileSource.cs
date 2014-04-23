using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class SputnikTileSource : TileSource
    {
        private readonly MercatorTransformation _transformation;

        public SputnikTileSource()
        {
            _transformation = new SpheroidTransformation();
        }

        public override object GetTile(int tileX, int tileY, int zoom)
        {
            return new Uri(String.Format("http://m.tiles.maps.sputnik.ru/tiles/kmt2/{0}/{1}/{2}.png",
                       zoom, tileX, tileY));
        }

        public override MercatorTransformation Transformation
        {
            get
            {
                return _transformation;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class OsmTileSource : TileSource
    {
        private readonly MercatorTransformation _transformation;
        private readonly Random _rand;
        private readonly string[] _serverPreffix;

        public OsmTileSource()
        {
            _transformation = new SpheroidTransformation();
            _rand = new Random();
            _serverPreffix = new[] { "a", "b", "c" };
        }

        public override object GetTile(int tileX, int tileY, int zoom)
        {
            return new Uri(String.Format("http://{3}.tah.openstreetmap.org/Tiles/tile/{0}/{1}/{2}.png",
                       zoom, tileX, tileY, _serverPreffix[_rand.Next(3)]));
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

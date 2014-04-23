using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class YandexTileSource : TileSource
    {
        private readonly MercatorTransformation _transformation;
        private readonly Random _rand;

        public YandexTileSource()
        {
            _transformation = new EllipsoidTransformation();
            _rand = new Random();
        }

        public override object GetTile(int tileX, int tileY, int zoom)
        {
            return new Uri(string.Format("http://vec0{0}.maps.yandex.net/tiles?l=map&v=2.20.0&x={1}&y={2}&z={3}&g={4}",
                            1 + _rand.Next(3), tileX, tileY, zoom, "Gagarin".Substring(0, _rand.Next(8))));
        }

        public override MercatorTransformation Transformation
        {
            get { return _transformation; }
        }
    }
}

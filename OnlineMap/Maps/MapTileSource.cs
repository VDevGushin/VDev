using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OnlineMap.Maps
{
    internal class MapTileSource : MultiScaleTileSource
    {
        private readonly TileSource _tileSource;
        private readonly int _zoomDelta;

        internal MapTileSource(TileSource tileSource, int tileWidth, int tileHeight)
            : base(0x40000 * tileWidth, 0x40000 * tileHeight, tileWidth, tileHeight, 0)
        {
            _tileSource = tileSource;
            _zoomDelta = (int)Math.Log(tileWidth, 2);
            TileBlendTime = new TimeSpan(0, 0, 0, 0, 250);
        }

        protected override void GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY, System.Collections.Generic.IList<object> tileImageLayerSources)
        {
            int index = tileLevel - _zoomDelta;
            if ((index >= 1) && (index <= 0x16))
            {
                Uri uri = (Uri)_tileSource.GetTile(tilePositionX, tilePositionY, index);
                //save tiles 
                tileImageLayerSources.Add(uri);
            }
        }
    }
}

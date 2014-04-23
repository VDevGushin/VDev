using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OnlineMap.Maps
{
   public class SimplePin
    {//Position pos, Color color, char text
        public Position position { get; set; }
        public Color color { get; set; }
        public char text { get; set; }
        public double  Scale { get; set; }
        public bool IsSimpleImagePin { get; set; }
    }
}

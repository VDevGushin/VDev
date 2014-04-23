using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
    public class PositionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string val = value as string;
            if (val == null)
            {
                throw new NotSupportedException(ExceptionStrings.PositionConverter_InvalidPositionFormat);
            }
            string[] strArray = val.Split(new char[] { ',', ' ' });
            if (strArray.Length != 2)
            {
                throw new NotSupportedException(ExceptionStrings.PositionConverter_InvalidPositionFormat);
            }
            double longitude, latitude;
            if (double.TryParse(strArray[0], NumberStyles.Float, CultureInfo.InvariantCulture, out longitude) &&
                double.TryParse(strArray[1], NumberStyles.Float, CultureInfo.InvariantCulture, out latitude))
            {
                return new Position(longitude, latitude);
            }
            throw new NotSupportedException(ExceptionStrings.PositionConverter_InvalidPositionFormat);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMap.Maps
{
   
        /// <summary>
        /// Geografic coordinate format 
        /// </summary>
        public enum CoordinateFormat
        {
            /// <value>
            /// W12°34'56.78"
            /// </value>
            LDD_MM_SSSS,
            /// <value>
            /// W12°34.5678'
            /// </value>
            LDD_MMMMMM,
            /// <value>
            /// W12.345678°
            /// </value>
            LDD_DDDDDD,
            /// <value>
            /// -12°34'56.78"
            /// </value>
            SDD_MM_SSSS,
            /// <value>
            /// -12°34.5678'
            /// </value>
            SDD_MMMMMM,
            /// <value>
            /// -12.345678°
            /// </value>
            SDD_DDDDDD
        
    }
}

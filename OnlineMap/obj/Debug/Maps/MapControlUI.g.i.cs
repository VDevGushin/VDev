﻿#pragma checksum "D:\Mapcontrol\OnlineMap\OnlineMap\Maps\MapControlUI.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "22644718E0DE2F3706C5AD08D3EA2FEF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace OnlineMap.Maps {
    
    
    public partial class MapControlUI : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.MultiScaleImage _msi;
        
        internal System.Windows.Controls.Grid grdLayer1;
        
        internal System.Windows.Shapes.Ellipse GridCurrentLocation;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/OnlineMap;component/Maps/MapControlUI.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this._msi = ((System.Windows.Controls.MultiScaleImage)(this.FindName("_msi")));
            this.grdLayer1 = ((System.Windows.Controls.Grid)(this.FindName("grdLayer1")));
            this.GridCurrentLocation = ((System.Windows.Shapes.Ellipse)(this.FindName("GridCurrentLocation")));
        }
    }
}

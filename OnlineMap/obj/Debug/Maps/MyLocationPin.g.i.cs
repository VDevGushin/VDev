﻿#pragma checksum "D:\Mapcontrol\OnlineMap\OnlineMap\Maps\MyLocationPin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C74C78B9017D9601E8EF57EC083524B0"
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
    
    
    public partial class MyLocationPin : System.Windows.Controls.UserControl {
        
        internal System.Windows.Shapes.Path shpShadow;
        
        internal System.Windows.Shapes.Path shpPushpin;
        
        internal System.Windows.Controls.TextBlock txtTitle;
        
        internal System.Windows.Shapes.Ellipse shpDot;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/OnlineMap;component/Maps/MyLocationPin.xaml", System.UriKind.Relative));
            this.shpShadow = ((System.Windows.Shapes.Path)(this.FindName("shpShadow")));
            this.shpPushpin = ((System.Windows.Shapes.Path)(this.FindName("shpPushpin")));
            this.txtTitle = ((System.Windows.Controls.TextBlock)(this.FindName("txtTitle")));
            this.shpDot = ((System.Windows.Shapes.Ellipse)(this.FindName("shpDot")));
        }
    }
}


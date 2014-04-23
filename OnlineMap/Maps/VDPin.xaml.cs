using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace OnlineMap.Maps
{
    public partial class VDPin : UserControl
    {
        public VDPin()
        {
            InitializeComponent();
        }

        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(VDPin), new PropertyMetadata(OnPositionChangedCallback));


        private static void OnPositionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VDPin)d).OnAzimuthChanged(e);
        }

        private void OnAzimuthChanged(DependencyPropertyChangedEventArgs e)
        {
            /// 
        }


        private double p_scale = 1.0;
        /// <summary>
        /// Property. Size of pushpin. Default value is 1.0 (Width:35, Height:63).
        /// </summary>
        public double Scale
        {
            get
            {
                return p_scale;
            }
            set
            {
                p_scale = value;
                ScaleTransform matScale = new ScaleTransform() { ScaleX = p_scale, ScaleY = p_scale };
                grdRoot.RenderTransform = matScale;
            }
        }
    }
}

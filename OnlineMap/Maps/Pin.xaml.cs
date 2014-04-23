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
    public partial class Pin : UserControl
    {
        public Pin()
        {
            InitializeComponent();      ShowSimpleImage();     
        }


        void ShowSimpleImage()
        {
            txtTitle.Visibility = System.Windows.Visibility.Collapsed;
            shpShadow.Visibility = System.Windows.Visibility.Collapsed;
            shpDot.Visibility = System.Windows.Visibility.Collapsed;
            shpPushpin.Visibility = System.Windows.Visibility.Collapsed;
            SimpleImage.Visibility = System.Windows.Visibility.Visible;
        }

        void HideImage()
        {
            txtTitle.Visibility = System.Windows.Visibility.Visible;
            shpShadow.Visibility = System.Windows.Visibility.Visible;
            shpDot.Visibility = System.Windows.Visibility.Visible;
            shpPushpin.Visibility = System.Windows.Visibility.Visible;
            SimpleImage.Visibility = System.Windows.Visibility.Collapsed;
        }
        //simple color
        public Pin(Color color)
        {
            InitializeComponent();
            HideImage();
            shpPushpin.Fill = new SolidColorBrush(color);
        }
        //color
        public Pin(Color color, char text)
        {
            InitializeComponent();
            HideImage();
            txtTitle.Text = Convert.ToString(text);

            if (text != char.MinValue && text != ' ')
            {
                txtTitle.Visibility = Visibility.Visible;
                shpDot.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtTitle.Visibility = Visibility.Collapsed;
                shpDot.Visibility = Visibility.Visible;
            }
            shpPushpin.Fill = new SolidColorBrush(color);
         
        }
        //only text
        public Pin(char text)
        {
            InitializeComponent();
            HideImage();
        }

        
        public Position Position
        {
            get { return (Position)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Position), typeof(Pin), new PropertyMetadata(OnPositionChangedCallback));


        private static void OnPositionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Pin)d).OnAzimuthChanged(e);
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

        private void UserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {            
            PushAnimation.Begin();
        }

                     
    }
}

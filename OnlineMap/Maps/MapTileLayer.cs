using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace OnlineMap.Maps
{

    public static class MultiScaleImageConstants
    {
        public static double zoomInFactor = 0.1;

        public static double zoomOutFactor = 0.1;

        public static double zoomInFactorTap = 1;
    }



    public class MapTileLayer : Panel
    {

        //layout of map tiles        
        private readonly MultiScaleImage _msi;
        


        private const int _tileWidth = 0x100;
        private const int _tileHeight = 0x100;
        public TileSource TileSource
        {
            get { return (TileSource)GetValue(TileSourceProperty); }
            set { SetValue(TileSourceProperty, value); }
        }

        public static readonly DependencyProperty TileSourceProperty =
            DependencyProperty.Register("TileSource", typeof(TileSource), typeof(MapTileLayer), new PropertyMetadata(new OsmTileSource(), OnTileSourceChangedCallback));

        private static void OnTileSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapTileLayer)d).OnTileSourceChanged(e);
        }

        private void OnTileSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            _msi.Source = new MapTileSource((TileSource)e.NewValue, _tileWidth, _tileHeight);

            SetCenter(Center, Zoom);
        }

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if (value >= 1.5 && value <= 19.5)
                {
                    Debug.WriteLine(string.Format("Zoom:{0}", Zoom));
                    SetValue(ZoomProperty, value);
                }
            }
        }

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MapTileLayer), new PropertyMetadata(1.0, OnZoomChangedCallback));

        private static void OnZoomChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapTileLayer)d).OnZoomChanged(e);
        }

        private void OnZoomChanged(DependencyPropertyChangedEventArgs e)
        {

            SetCenter(Center, (double)e.NewValue);
        }

        public double Azimuth
        {
            get { return (double)GetValue(AzimuthProperty); }
            set { SetValue(AzimuthProperty, value); }
        }

        public static readonly DependencyProperty AzimuthProperty =
            DependencyProperty.Register("Azimuth", typeof(double), typeof(MapTileLayer), new PropertyMetadata(1.0, OnAzimuthChangedCallback));


        private static void OnAzimuthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapTileLayer)d).OnAzimuthChanged(e);
        }

        private void OnAzimuthChanged(DependencyPropertyChangedEventArgs e)
        {
            RotateTransform transform = _msi.RenderTransform as RotateTransform;
            if (transform != null)
            {
                transform.Angle = 360.0 - (double)e.NewValue;
            }
        }

        public Position Center
        {
            get { return (Position)GetValue(CenterProperty); }
            set
            {
                var _tmp = value;
                if (_tmp.Latitude > 86)
                {
                    _tmp.Latitude = -86;
                }
                else if (_tmp.Latitude < -86)
                {
                    _tmp.Latitude = 86;
                }

                //if (_tmp.Longitude > 180)
                //{
                //    _tmp.Longitude = -180;
                //}
                //else if (_tmp.Longitude < -180)
                //{
                //    _tmp.Longitude = 180;
                //}


                SetValue(CenterProperty, _tmp);
                Debug.WriteLine(string.Format("{0} {1}", Center.Latitude, Center.Longitude));
            }
        }

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Position), typeof(MapTileLayer), new PropertyMetadata(new Position(), OnCenterChangedCallback));

        private static void OnCenterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapTileLayer)d).OnCenterChanged(e);
        }

        private void OnCenterChanged(DependencyPropertyChangedEventArgs e)
        {
            SetCenter((Position)e.NewValue);
        }

        public void SetCenter(Position center, double zoom)
        {
            _msi.ViewportWidth = _msi.ActualWidth / (_tileWidth * Math.Pow(2.0, zoom));
            SetCenter(center);
        }

        public void SetCenter(Position center)
        {
            if (TileSource != null)
            {
                //set center of screen in position type

                _msi.ViewportOrigin = new Point(TileSource.Transformation.GetTileX(center.Longitude) - _msi.ViewportWidth / 2,
                   TileSource.Transformation.GetTileY(center.Latitude) - _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2);
            }
        }

        public MapTileLayer()
            : this(null)
        { }

        internal MapTileLayer(TileSource tileSource)
            : base()
        {
            Clip = new RectangleGeometry();
            _msi = new MultiScaleImage { UseSprings = false, SkipLevels = 20 };
            _msi.RenderTransform = new RotateTransform();
            _msi.RenderTransformOrigin = new Point(0.5, 0.5);
            //_msi.IsHitTestVisible = false;
            _msi.ManipulationStarted += _msi_ManipulationStarted;
            _msi.ManipulationDelta += _msi_ManipulationDelta;
            _msi.DoubleTap += _msi_DoubleTap;
            Children.Add(_msi);
            TileSource = tileSource ?? new OsmTileSource();
            SizeChanged += MapTileLayer_SizeChanged;
        }

        void _msi_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //set zoom
            Zoom += MultiScaleImageConstants.zoomInFactorTap;
            //get center of tap
            var logic = _msi.ElementToLogicalPoint(e.GetPosition(_msi));
            var Position = new Position(TileSource.Transformation.GetLongitude(logic.X), TileSource.Transformation.GetLatitude(logic.Y));
            Center = Position;
        }

        void _msi_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //if move 
            if (e.DeltaManipulation.Scale.X == 0 && e.DeltaManipulation.Scale.Y == 0)
            {
                if ((e.CumulativeManipulation.Translation.X > 15 || e.CumulativeManipulation.Translation.X < -15) || (e.CumulativeManipulation.Translation.Y > 15 || e.CumulativeManipulation.Translation.Y < -15))
                {
                    _msi.ViewportOrigin = new Point
                    {
                        X = multiScaleImageOrigin.X -
                            (e.CumulativeManipulation.Translation.X /
                            _msi.ActualWidth * _msi.ViewportWidth),
                        Y = multiScaleImageOrigin.Y -
                            (e.CumulativeManipulation.Translation.Y /
                            _msi.ActualHeight * _msi.ViewportWidth),
                    };

                    var CenterPoint = new Point(
                        _msi.ViewportOrigin.X + _msi.ViewportWidth / 2,
                        _msi.ViewportOrigin.Y + _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2
                        );

                    //get center position 
                    var Position = new Position(TileSource.Transformation.GetLongitude(CenterPoint.X), TileSource.Transformation.GetLatitude(CenterPoint.Y));
                    Center = Position;
                    //SetCenter(Position,Zoom);               
                }
            }

            //if zoom
            else
            {
                if (e.PinchManipulation != null)
                {
                    if (IsZoomInPinch(e.PinchManipulation))
                    {

                        Zoom += MultiScaleImageConstants.zoomInFactor;
                        // var NewZoom = Zoom + 0.2;                        
                        // var CenterPoint = new Point(
                        //    _msi.ViewportOrigin.X + _msi.ViewportWidth / 2,
                        //    _msi.ViewportOrigin.Y + _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2
                        //    );
                        // var Position = new Position(TileSource.Transformation.GetLongitude(CenterPoint.X), TileSource.Transformation.GetLatitude(CenterPoint.Y));
                        // SetCenter(CenterOfMapInScreen, NewZoom);                                                
                        // //Zoom = NewZoom;                        
                        //// Debug.WriteLine(string.Format("zoom: {0}", Zoom));
                        // if (_msi.ViewportWidth > 1)
                        //     _msi.ViewportWidth = 1;
                    }
                    if (!IsZoomInPinch(e.PinchManipulation))
                    {
                        Zoom -= MultiScaleImageConstants.zoomOutFactor;
                        // var NewZoom = Zoom - 0.2;

                        // var CenterPoint = new Point(
                        //    _msi.ViewportOrigin.X + _msi.ViewportWidth / 2,
                        //    _msi.ViewportOrigin.Y + _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2
                        //    );

                        // var Position = new Position(TileSource.Transformation.GetLongitude(CenterPoint.X), TileSource.Transformation.GetLatitude(CenterPoint.Y));
                        // SetCenter(CenterOfMapInScreen, NewZoom);


                        //// Zoom = NewZoom;
                        //// Debug.WriteLine(string.Format("zoom: {0}", Zoom));
                        // if (_msi.ViewportWidth > 1)
                        //     _msi.ViewportWidth = 1;                     
                    }
                }
            }


            //var Point = new Point(TileSource.Transformation.GetTileX(CenterOfMapInScreen.Longitude),
            //   TileSource.Transformation.GetTileY(CenterOfMapInScreen.Latitude));


            //var logicalPoint = _msi.ElementToLogicalPoint(Point);
            //_msi.ZoomAboutLogicalPoint(zoomScale, logicalPoint.X, logicalPoint.Y);

            //var logicalPoint = _msi.ElementToLogicalPoint(new Point
            //{
            //    X = manipulationOrigin.X - e.CumulativeManipulation.Translation.X,
            //    Y = manipulationOrigin.Y - e.CumulativeManipulation.Translation.Y
            //}
            //);

            //_msi.ZoomAboutLogicalPoint(zoomScale, logicalPoint.X, logicalPoint.Y);
        }


        private bool IsZoomInPinch(PinchManipulation pinchManipulation)
        {
            double originalDistance = Math.Sqrt(
                Math.Pow(pinchManipulation.Original.PrimaryContact.X - pinchManipulation.Original.SecondaryContact.X, 2) +
                Math.Pow(pinchManipulation.Original.PrimaryContact.Y - pinchManipulation.Original.SecondaryContact.Y, 2));
            double currentDistance = Math.Sqrt(
                Math.Pow(pinchManipulation.Current.PrimaryContact.X - pinchManipulation.Current.SecondaryContact.X, 2) +
                Math.Pow(pinchManipulation.Current.PrimaryContact.Y - pinchManipulation.Current.SecondaryContact.Y, 2));
            return currentDistance > originalDistance;
        }

        private Point manipulationOrigin { get; set; }
        private Point multiScaleImageOrigin { get; set; }
        private void _msi_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            multiScaleImageOrigin = new Point(_msi.ViewportOrigin.X, _msi.ViewportOrigin.Y);
            manipulationOrigin = e.ManipulationOrigin;
        }




        protected override Size MeasureOverride(Size availableSize)
        {
            ((RectangleGeometry)Clip).Rect = new Rect(0.0, 0.0, availableSize.Width, availableSize.Height);
            double side = Math.Sqrt(availableSize.Height * availableSize.Height + availableSize.Width * availableSize.Width);
            _msi.Measure(new Size(side, side));
            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double side = Math.Sqrt(finalSize.Height * finalSize.Height + finalSize.Width * finalSize.Width);
            _msi.Arrange(new Rect((finalSize.Width - side) / 2, (finalSize.Height - side) / 2, side, side));
            return base.ArrangeOverride(finalSize);
        }

        void MapTileLayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetCenter(Center, Zoom);
        }



        //Pins
        public ObservableCollection<Pin> PinCollection
        {
            get { return (ObservableCollection<Pin>)GetValue(PinCollectionProperty); }
            set
            {
                SetValue(PinCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty PinCollectionProperty =
            DependencyProperty.Register("PinCollection", typeof(ObservableCollection<Pin>), typeof(MapTileLayer), new PropertyMetadata(OnPinCollectionChangedCallback));

        private static void OnPinCollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapTileLayer)d).OnPinCollectionChanged(e);
        }

        private void OnPinCollectionChanged(DependencyPropertyChangedEventArgs e)
        {
            AddPins((ObservableCollection<Pin>)e.NewValue);
        }

        private void AddPins(ObservableCollection<Pin> MapPinCollection)
        {
            if (MapPinCollection.Count > 0)
            {
                foreach (var pin in MapPinCollection)
                {
                     
                }
            }

            //    foreach (var pin in enumerable)
            //    {                

            //        MultiScaleImage pinab = new MultiScaleImage { UseSprings = false, SkipLevels = 20 };
            //        pinab.RenderTransform = new RotateTransform();
            //        pinab.RenderTransformOrigin = new Point(0.5, 0.5);
            //        Children.Add(pinab);
            //        //pinab.ViewportOrigin = new Point(TileSource.Transformation.GetTileX(pin.PinCoordinate.Longitude) - _msi.ViewportWidth / 2,
            //          // TileSource.Transformation.GetTileY(pin.PinCoordinate.Latitude) - _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2);

            //        var center = new Point(TileSource.Transformation.GetTileX(pin.PinCoordinate.Longitude),
            //           TileSource.Transformation.GetTileY(pin.PinCoordinate.Latitude));

            //        //
            ////         <Canvas Grid.Row="0" Margin="3,3,2,2">
            ////    <Border x:Name="imageBorder" BorderThickness="2" BorderBrush="Black" Opacity="0" />
            ////</Canvas>
            //        //pinab.Source = new Uri("/pin.jpg");

            //        //_msi = new MultiScaleImage { UseSprings = false, SkipLevels = 20 };
            //        //_msi.RenderTransform = new RotateTransform();
            //        //_msi.RenderTransformOrigin = new Point(0.5, 0.5);
            //        ////_msi.IsHitTestVisible = false;
            //        //_msi.ManipulationStarted += _msi_ManipulationStarted;
            //        //_msi.ManipulationDelta += _msi_ManipulationDelta;
            //        //_msi.DoubleTap += _msi_DoubleTap;
            //        //Children.Add(_msi);
            //    }
            //    //Border x:Name="imageBorder" BorderThickness="2" BorderBrush="Black" Opacity="0"
            //    /*
            //     *  _msi.ViewportOrigin = new Point(TileSource.Transformation.GetTileX(center.Longitude) - _msi.ViewportWidth / 2,
            //           TileSource.Transformation.GetTileY(center.Latitude) - _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight / 2);
            //     */
            //}
        }
    }
}

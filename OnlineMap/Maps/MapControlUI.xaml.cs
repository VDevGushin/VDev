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
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Shapes;
using Windows.Devices.Geolocation;

namespace OnlineMap.Maps
{
    public partial class MapControlUI : UserControl
    {

        public Rect ViewRect { get; set; }

        private Dictionary<Pin, Point> _CollectionLocations;

        public Dictionary<Pin, Point> CollectionLocations
        {
            get { return _CollectionLocations; }
            set 
            {
                _CollectionLocations = value; 
            }
        }

      
        public int Sensitivity = 10;

        private const int _tileWidth = 0x100;
        private const int _tileHeight = 0x100;
        

        #region TileSource
        public TileSource TileSource
        {
            get { return (TileSource)GetValue(TileSourceProperty); }
            set { SetValue(TileSourceProperty, value); }
        }

        public static readonly DependencyProperty TileSourceProperty =
            DependencyProperty.Register("TileSource", typeof(TileSource), typeof(MapControlUI), new PropertyMetadata(new OsmTileSource(), OnTileSourceChangedCallback));

        private static void OnTileSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnTileSourceChanged(e);
        }

        private void OnTileSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            _msi.Source = new MapTileSource((TileSource)e.NewValue, _tileWidth, _tileHeight);
            SetCenter(Center, Zoom);
        }
        #endregion

        #region Zoom

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if (value >= 3 && value <= 19.5)
                {
                   // Debug.WriteLine(string.Format("Zoom:{0}", Zoom));
                    SetValue(ZoomProperty, value);
                }
            }
        }

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom", typeof(double), typeof(MapControlUI), new PropertyMetadata(1.0, OnZoomChangedCallback));

        private static void OnZoomChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnZoomChanged(e);
        }

        private void OnZoomChanged(DependencyPropertyChangedEventArgs e)
        {

            SetCenter(Center, (double)e.NewValue);
        }

        #endregion

        #region Azimut
        public double Azimuth
        {
            get { return (double)GetValue(AzimuthProperty); }
            set { SetValue(AzimuthProperty, value); }
        }

        public static readonly DependencyProperty AzimuthProperty =
            DependencyProperty.Register("Azimuth", typeof(double), typeof(MapControlUI), new PropertyMetadata(1.0, OnAzimuthChangedCallback));


        private static void OnAzimuthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnAzimuthChanged(e);
        }

        private void OnAzimuthChanged(DependencyPropertyChangedEventArgs e)
        {
            RotateTransform transform = _msi.RenderTransform as RotateTransform;
            if (transform != null)
            {
                transform.Angle = 360.0 - (double)e.NewValue;
            }
        }
        #endregion

        #region Center
        public Position Center
        {
            get { return (Position)GetValue(CenterProperty); }
            set
            {
                var _tmp = value;
                if (_tmp.Latitude > 85)
                {
                    _tmp.Latitude = 85;
                }
                else if (_tmp.Latitude < -85)
                {
                    _tmp.Latitude = -85;
                }
                if (_tmp.Longitude < -180)
                {
                    _tmp.Longitude = -170;
                }
                else if (_tmp.Longitude > 180)
                {
                    _tmp.Longitude = 170;
                }              
                    SetValue(CenterProperty, _tmp);
                    Debug.WriteLine(string.Format("{0} {1}", Center.Latitude, Center.Longitude));               
            }
        }


        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Position), typeof(MapControlUI), new PropertyMetadata(new Position(), OnCenterChangedCallback));

        private static void OnCenterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnCenterChanged(e);
        }

        private void OnCenterChanged(DependencyPropertyChangedEventArgs e)
        {
            SetCenter((Position)e.NewValue);
        }
        #endregion

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
                
                //get rect of visible points                  
                ViewRect = getRectOfVisible();
                CheckPushPinsForDraw(ViewRect);                
            }
        }

        private void CheckPushPinsForDraw(Rect ViewRect)
        {
            //check push pins state
            if (CollectionLocations != null && CollectionLocations.Count > 0)
                //check visibility for pushpins
                CheckPointInRect(ViewRect, CollectionLocations);
        }

        private void CheckPointInRect(Rect rect, Dictionary<Pin, Point> PushPins)
        {
            foreach (var pin in PushPins)
            {              
                pin.Key.RenderTransform = ProjectedTranslateTransform(CollectionLocations[pin.Key]);
                //  Point p = new Point(TileSource.Transformation.GetTileX(pin.Longitude), TileSource.Transformation.GetTileY(pin.Latitude));
                //  var Position = new Position(TileSource.Transformation.GetLongitude(CenterPoint.X), TileSource.Transformation.GetLatitude(CenterPoint.Y));
                if (rect.Contains(pin.Value))
                {                                    
                    pin.Key.Opacity = 1.0;
                    // Debug.WriteLine("Эта точка в зоне видимости");
                }
                else
                {                   
                    pin.Key.Opacity = 0.0;
                    //Debug.WriteLine("Эта точка не в зоне видимости");
                }
            }
        }

    
        public MapControlUI()
        {
            InitializeComponent();
            Clip = new RectangleGeometry();        
            CollectionLocations = new Dictionary<Pin, Point>();
            ViewRect = new Rect();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _msi.Width = this.ActualWidth;
            _msi.Height = this.ActualHeight;
            grdLayer1.Height = _msi.ActualHeight;
            grdLayer1.Width = _msi.ActualWidth;
            grdLayer1.ManipulationStarted += grdLayer1_ManipulationStarted;
           
            _msi.UseSprings = false;
            _msi.SkipLevels = 10;
            _msi.RenderTransform = new RotateTransform();
            _msi.RenderTransformOrigin = new Point(0.5, 0.5);
            _msi.BlurFactor = 1.0;
            _msi.UseLayoutRounding = false;
            _msi.UseOptimizedManipulationRouting = false;                                         
            _msi.ManipulationStarted += _msi_ManipulationStarted;
            _msi.ManipulationDelta += _msi_ManipulationDelta;
            _msi.DoubleTap += _msi_DoubleTap;
            TileSource = new SputnikTileSource();
            SizeChanged += MapTileLayer_SizeChanged;

            
        }      
             
        void grdLayer1_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            e.Complete();
        }
      
        #region Get Rect of visibility points
        private Rect getRectOfVisible()
        {
            try
            {                
               return new Rect(_msi.ViewportOrigin.X, _msi.ViewportOrigin.Y, _msi.ViewportWidth, _msi.ViewportWidth / _msi.ActualWidth * _msi.ActualHeight);            
            }
            catch
            {
                return new Rect(0,0,0,0);
            }
        }
        #endregion

        #region Manipulation with map

        void _msi_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //set zoom
            //Zoom += MultiScaleImageConstants.zoomInFactorTap;
            Zoom += 1;
            //get center of tap            
            var Position = new Position(TileSource.Transformation.GetLongitude(_msi.ElementToLogicalPoint(e.GetPosition(_msi)).X), TileSource.Transformation.GetLatitude(_msi.ElementToLogicalPoint(e.GetPosition(_msi)).Y));
            Center = Position;
            GridCurrentLocation.Visibility = System.Windows.Visibility.Collapsed;
        }

        void _msi_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //if move           
            if (e.DeltaManipulation.Scale.X == 0 && e.DeltaManipulation.Scale.Y == 0)
            {
                if ((e.CumulativeManipulation.Translation.X > Sensitivity || e.CumulativeManipulation.Translation.X < -Sensitivity) || (e.CumulativeManipulation.Translation.Y > Sensitivity || e.CumulativeManipulation.Translation.Y < -Sensitivity))
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

                       // Zoom += MultiScaleImageConstants.zoomInFactor;
                        Zoom += (Zoom / 4.4) / Zoom;
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
                        //Zoom -= MultiScaleImageConstants.zoomOutFactor;
                        Zoom -= (Zoom /4.4) / Zoom;
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
            GridCurrentLocation.Visibility = System.Windows.Visibility.Collapsed;
            multiScaleImageOrigin = new Point(_msi.ViewportOrigin.X, _msi.ViewportOrigin.Y);
            manipulationOrigin = e.ManipulationOrigin;
        }
      
        #endregion


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

        #region Add Pins
        public Pin addSimplePin(Position pos, Color color, char text)
        {
            Pin newSimplePin = new Pin(color, text);
            //for gestures
            //newSimplePin.IsEnabled = false;
            newSimplePin.Position = pos;
            newSimplePin.HorizontalAlignment = HorizontalAlignment.Left;
            newSimplePin.VerticalAlignment = VerticalAlignment.Top;            
            newSimplePin.DoubleTap += newSimplePin_DoubleTap;           
            SimpleAdd(new Point(TileSource.Transformation.GetTileX(pos.Longitude),TileSource.Transformation.GetTileY(pos.Latitude)), newSimplePin);
            return newSimplePin;                      
        }

        void newSimplePin_DoubleTap(object sender, GestureEventArgs e)
        {
            var _sen = sender as Pin;
          // MessageBox.Show(_sen.Position.ToString());
            this.Center = _sen.Position;
        }


        public void SimpleAdd(Point posPoint, Pin elementUI)
        {
            CollectionLocations.Add(elementUI, posPoint);
            try
            {                
                grdLayer1.Children.Add(elementUI);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }                       
        }
        #endregion

        private Transform ProjectedTranslateTransform(Point value)
        {
            try
            {              
                return new TranslateTransform() { X = _msi.LogicalToElementPoint(value).X, Y = _msi.LogicalToElementPoint(value).Y };
            }
            catch
            {
                return null;
            }
        }


        #region Add PinsSource

        public ObservableCollection<SimplePin> PinCollectionSource
        {
            get { return (ObservableCollection<SimplePin>)GetValue(PinCollectionProperty); }
            set { SetValue(PinCollectionProperty, value); }
        }

        public static readonly DependencyProperty PinCollectionProperty =
            DependencyProperty.Register("PinCollectionSource", typeof(ObservableCollection<SimplePin>), typeof(MapControlUI), new PropertyMetadata(OnPinCollectionChangedCallback));


        private static void OnPinCollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnPinCollectionChanged(e);
        }

        private void OnPinCollectionChanged(DependencyPropertyChangedEventArgs e)
        {
            foreach (var _pin in (ObservableCollection<SimplePin>)e.NewValue)
            {
                Pin newSimplePin = null;
                if (!_pin.IsSimpleImagePin)
                {
                     newSimplePin = new Pin(_pin.color, _pin.text);
                }
                else
                {
                     newSimplePin = new Pin();
                }
                newSimplePin.Scale = _pin.Scale;
                newSimplePin.Position = _pin.position;
                newSimplePin.DoubleTap += newSimplePin_DoubleTap;
                newSimplePin.HorizontalAlignment = HorizontalAlignment.Left;
                newSimplePin.VerticalAlignment = VerticalAlignment.Top;
                SimpleAdd(new Point(TileSource.Transformation.GetTileX(_pin.position.Longitude), TileSource.Transformation.GetTileY(_pin.position.Latitude)), newSimplePin);                       
            }
        }

        #endregion

        #region CurrentLocation
      
        public bool UseUserLocation
        {
            get { return (bool)GetValue(UseUserLocationProperty); }
            set { SetValue(UseUserLocationProperty, value); }
        }

        public static readonly DependencyProperty UseUserLocationProperty =
            DependencyProperty.Register("UseUserLocation", typeof(bool), typeof(MapControlUI), new PropertyMetadata(OnUseUserLocationChangedCallback));


        private static void OnUseUserLocationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MapControlUI)d).OnUseUserLocationChanged(e);
        }

        private void OnUseUserLocationChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                    GetCurrentLocation();
            }
            else
            {
                StopGetCurrentLocation();
            }
        }

        //CurrentLocation Prop
      
        Geolocator geolocator;
        private void StopGetCurrentLocation()
        {
            if (geolocator != null)
            {
                geolocator.PositionChanged -= geolocator_PositionChanged;
                geolocator = null;
                GridCurrentLocation.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void GetCurrentLocation()
        {
            if (geolocator == null)
                geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.Default;
            geolocator.MovementThreshold =10;
            geolocator.ReportInterval = 500;
            geolocator.PositionChanged += geolocator_PositionChanged;
        }


        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (args != null)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    Center = new Maps.Position(args.Position.Coordinate.Longitude, args.Position.Coordinate.Latitude);
                    GridCurrentLocation.Visibility = System.Windows.Visibility.Visible;
                });
            }
        }

        #endregion

    }
}


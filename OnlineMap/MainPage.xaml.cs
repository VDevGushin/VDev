using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OnlineMap.Resources;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Windows.Devices.Geolocation;

namespace OnlineMap
{
    public partial class MainPage : PhoneApplicationPage
    {      
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            //add simple pin
            ////a.AddPin(0.4, 0.5, Colors.DarkGray, 'D');
            //a.addSimplePin(new Maps.Position(37.594998, 55.889217), Colors.Green, char.MinValue).Scale = 0.9;
            ////55.813979, 37.307494
            //a.addSimplePin(new Maps.Position(37.307494, 55.81397), Colors.Red, 'K').Scale = 0.7;
            ////55.888416, 37.596523
            //a.addSimplePin(new Maps.Position(37.596523, 55.888416), Colors.Orange, 'L').Scale = 0.7;
            ////55.889481, 37.597038
            //a.addSimplePin(new Maps.Position(37.597038, 55.889481), Colors.Magenta, 'D').Scale = 0.7;

            //add pin collection
            ObservableCollection<Maps.SimplePin> _pinsList = new ObservableCollection<Maps.SimplePin>();
            _pinsList.Add(new Maps.SimplePin() { IsSimpleImagePin = true, position = new Maps.Position(37.594855, 55.889082), Scale = 0.9 });//55.756427, 37.627071
            _pinsList.Add(new Maps.SimplePin() { IsSimpleImagePin = false, color = Colors.Magenta, text = char.MinValue, position = new Maps.Position(37.308094, 55.814205), Scale = 0.7 });//55.814205,37.308094
            a.PinCollectionSource = _pinsList;
        }
    
        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            a.UseUserLocation = false;
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            a.UseUserLocation = true;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
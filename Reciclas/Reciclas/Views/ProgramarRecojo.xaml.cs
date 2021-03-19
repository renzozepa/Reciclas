using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProgramarRecojo : ContentPage
	{
        public ObservableCollection<RecojosPoint> SubLayer1
        {
            get; set;
        }
        public ProgramarRecojo ()
		{
			InitializeComponent ();

            SubLayer1 = new ObservableCollection<RecojosPoint>()
            {
                new RecojosPoint(){Id=1,Name="Casa 1",Description="Calle 1",Latitude = -12.008008, Longitude = -77.036608},
                new RecojosPoint(){Id=2,Name="Casa 2",Description="Calle 2",Latitude = -12.013027,Longitude = -77.031592},
                new RecojosPoint(){Id=3,Name="Casa 3",Description="Calle 3",Latitude = -12.008352,Longitude = -77.035803},
                new RecojosPoint(){Id=4,Name="Casa 4",Description="Calle 4",Latitude = -12.009369,Longitude = -77.034924},
                new RecojosPoint(){Id=5,Name="Casa 5",Description="Calle 5",Latitude = -12.013027,Longitude = -77.031592},
                new RecojosPoint(){Id=6,Name="Casa 6",Description="Calle 6",Latitude = -12.012283,Longitude = -77.032061},
                new RecojosPoint(){Id=7,Name="Casa 7",Description="Calle 7",Latitude = -12.013771,Longitude = -77.031294},
                new RecojosPoint(){Id=8,Name="Casa 8",Description="Calle 8",Latitude = -12.015807,Longitude = -77.030366},
                new RecojosPoint(){Id=9,Name="Casa 9",Description="Calle 9",Latitude = -12.015917,Longitude = -77.028491},
                new RecojosPoint(){Id=10,Name="Casa 10",Description="Calle 10",Latitude = -12.017234,Longitude = -77.027212},
                new RecojosPoint(){Id=11,Name="Casa 11",Description="Calle 11",Latitude = -12.020083,Longitude = -77.028636},
                new RecojosPoint(){Id=12,Name="Casa 12",Description="Calle 12",Latitude = -12.023433,Longitude = -77.031699},
                new RecojosPoint(){Id=13,Name="Casa 13",Description="Calle 13",Latitude = -12.026122,Longitude = -77.034124}
            };

            MyMap.ItemsSource = SubLayer1;

            foreach (var item in SubLayer1)
            {
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(item.Latitude, item.Longitude),
                    Label = item.Name,
                    Address = item.Description
                };
                MyMap.Pins.Add(pin);
            }
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var gps = Geolocation.GetLocationAsync(request);

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(-12.008008f, -77.036608f), Distance.FromKilometers(1)));

        }
	}
    

    public class RecojosPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public Posicion Posi { get; set; }

    }
}
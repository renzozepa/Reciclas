using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Reciclas.Tablas;
using Reciclas.ViewModels;
using Xamarin.Forms.Maps;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetalleRecojo : ContentPage
	{
		public DetalleRecojo ()
		{
			InitializeComponent ();
            
        }
        public DetalleRecojo(double Latitud , double Longitud , string Descripcion , string Direccion)
        {
            InitializeComponent();
            Pin pin = new Pin
            {
                Label = Descripcion,
                Address = Direccion,
                Type = PinType.Place,
                Position = new Position(Latitud, Longitud)
            };
            Mapa.Pins.Add(pin);
            Mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Latitud, Longitud), Distance.FromKilometers(1)));
        }
    }
}
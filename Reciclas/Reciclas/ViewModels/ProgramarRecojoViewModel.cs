using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Reciclas.ViewModels
{
    public class ProgramarRecojoViewModel : BaseViewModel
    {
        public ObservableCollection<RecojosPoint> SubLayer1
        {
            get; set;
        }

        public ProgramarRecojoViewModel()
        {
            SubLayer1 = new ObservableCollection<RecojosPoint>()
            {
                new RecojosPoint(){Id=1,Name="Casa 1",Description="Calle 1",Posi = new Posicion(-12.008008,-77.036608)},
                new RecojosPoint(){Id=2,Name="Casa 2",Description="Calle 2",Posi = new Posicion(-12.013027,-77.031592)},
                new RecojosPoint(){Id=3,Name="Casa 3",Description="Calle 3",Posi = new Posicion(-12.008352,-77.035803)},
                new RecojosPoint(){Id=4,Name="Casa 4",Description="Calle 4",Posi = new Posicion(-12.009369,-77.034924)},
                new RecojosPoint(){Id=5,Name="Casa 5",Description="Calle 5",Posi = new Posicion(-12.013027,-77.031592)},
                new RecojosPoint(){Id=6,Name="Casa 6",Description="Calle 6",Posi = new Posicion(-12.012283,-77.032061)},
                new RecojosPoint(){Id=7,Name="Casa 7",Description="Calle 7",Posi = new Posicion(-12.013771,-77.031294)},
                new RecojosPoint(){Id=8,Name="Casa 8",Description="Calle 8",Posi = new Posicion(-12.015807,-77.030366)},
                new RecojosPoint(){Id=9,Name="Casa 9",Description="Calle 9",Posi = new Posicion(-12.015917,-77.028491)},
                new RecojosPoint(){Id=10,Name="Casa 10",Description="Calle 10",Posi = new Posicion(-12.017234,-77.027212)},
                new RecojosPoint(){Id=11,Name="Casa 11",Description="Calle 11",Posi = new Posicion(-12.020083,-77.028636)},
                new RecojosPoint(){Id=12,Name="Casa 12",Description="Calle 12",Posi = new Posicion(-12.023433,-77.031699)},
                new RecojosPoint(){Id=13,Name="Casa 13",Description="Calle 13",Posi = new Posicion(-12.026122,-77.034124)}
            };
        }
    }

    public class RecojosPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Posicion Posi { get; set; }
        
    }

    public class Posicion
    {
        public Posicion(double Lat,double Lon) {
            Latitude = Lat;
            Longitude = Lon;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}

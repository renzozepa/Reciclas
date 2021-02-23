using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Reciclas.ViewModels
{
    public class ProgramarRecojoViewModel : BaseViewModel
    {
        public ObservableCollection<Point> SubLayer1
        {
            get; set;
        }

        public ProgramarRecojoViewModel()
        {
            SubLayer1 = new ObservableCollection<Point>()
            {
                new Point(-12.008008, -77.036608),
                new Point(-12.013027, -77.031592),
                new Point(-12.008352, -77.035803),
                new Point(-12.009369, -77.034924),
                new Point(-12.013027, -77.031592),
                new Point(-12.012283, -77.032061),
                new Point(-12.013771, -77.031294),
                new Point(-12.015807, -77.030366),
                new Point(-12.015917, -77.028491),
                new Point(-12.017234, -77.027212),
                new Point(-12.020083, -77.028636),
                new Point(-12.023433, -77.031699),
                new Point(-12.026122, -77.034124)
            };
        }
    }
}

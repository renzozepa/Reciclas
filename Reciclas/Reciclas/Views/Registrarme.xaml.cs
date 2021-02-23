using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Reciclas.ViewModels;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registrarme : ContentPage
	{
		public Registrarme ()
		{
			InitializeComponent ();
            BindingContext = new RegistrarmeViewModel(this.Navigation);
        }
	}
}
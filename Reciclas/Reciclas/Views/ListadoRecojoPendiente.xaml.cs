using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Reciclas.Tablas;
using Reciclas.ViewModels;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListadoRecojoPendiente : ContentPage
	{
        ListarRecojoPenViewModel vm;
        public ListadoRecojoPendiente ()
		{
			InitializeComponent ();
            vm = new ListarRecojoPenViewModel();
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            if (lstRecojoPen.SelectedItem != null)
                lstRecojoPen.SelectedItem = null;
        }
        private void lstRecojoPen_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (lstRecojoPen.SelectedItem != null)
                {
                    Usuario usuarios = (Usuario)e.SelectedItem;

                    if (usuarios != null)
                    {
                        Navigation.PushAsync(new DetalleRecojo
                        {
                            BindingContext = e.SelectedItem as Usuario
                        }
                            );
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
﻿using System;
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
	public partial class ListadoRecojo : ContentPage
	{
        ListarRecojoViewModel vm;
        public ListadoRecojo ()
		{
			InitializeComponent ();
            vm = new ListarRecojoViewModel();
            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            if (lstRecojo.SelectedItem != null)
                lstRecojo.SelectedItem = null;
        }
        private void lstRecojo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (lstRecojo.SelectedItem != null)
                {
                    Usuario usuarios = (Usuario)e.SelectedItem;
                    
                    if (usuarios != null)
                    {                        
                        Navigation.PushAsync(new DetalleRecojo
                            {
                                BindingContext = e.SelectedItem as Usuario }
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
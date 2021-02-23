using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Reciclas.Tablas;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultaRegistro : ContentPage
	{
		public ConsultaRegistro ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Usuario>();
                var contacts = conn.Table<Usuario>().ToList();
                contactsListView.ItemsSource = contacts;
            }
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
        //    {
        //        var Usuario = conn.Table<Usuario>().FirstOrDefault(j => j.ID == 3);
        //        if (Usuario == null)
        //            App.Current.MainPage.DisplayAlert("Información", "Usuario no encontrado.!", "Ok");

        //        conn.Delete(Usuario);

        //        conn.CreateTable<Usuario>();
        //        var contacts = conn.Table<Usuario>().ToList();
        //        contactsListView.ItemsSource = contacts;
        //    }
        //}
    }
}
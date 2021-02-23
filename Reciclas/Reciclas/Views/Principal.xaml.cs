using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reciclas.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Principal : MasterDetailPage
    {
		public Principal ()
		{
			InitializeComponent ();
            MyMenu();

        }
        public void MyMenu()
        {
            Detail = new NavigationPage(new Feed());
            List<Menu> menu = new List<Menu>
            {
                new Menu{ Page= new MiPerfil(),MenuTitle="Mi perfil",  MenuDetail="Mi perfil",icon="user.png",Id=1},
                new Menu{ Page= new RegistrarReciclador(),MenuTitle="Registrar reciclador",  MenuDetail="Registrar reciclador",icon="user.png",Id=2},
                new Menu{ Page= new RegistrarTipoResiduo(),MenuTitle="Registrar tipo residuo",  MenuDetail="Registrar tipo residuo",icon="actualizar.png",Id=3},
                new Menu{ Page= new ProgramarRecojo(),MenuTitle="Programar recojo",  MenuDetail="Programar recojo",icon="settings.png",Id=4},
                new Menu{ Page= new RegistrarRecojo(),MenuTitle="Registrar recojo",  MenuDetail="Registrar recojo",icon="settings.png",Id=5},
                new Menu{ Page= new Login(),MenuTitle="Salir",  MenuDetail="Salir",icon="salir.png",Id=6},
            };
            ListMenu.ItemsSource = menu;

        }

        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var menu = e.SelectedItem as Menu;
            IsPresented = false;
            Detail = new NavigationPage(menu.Page);

            //if (menu != null)
            //{
            //    if (menu.Id == 8)
            //    {
            //        App.Token = null;
            //    }
            //    if (App.Token != null)
            //    {
            //        IsPresented = false;
            //        Detail = new NavigationPage(menu.Page);
            //    }
            //    else
            //    {
            //        if (menu.Id == 1 || menu.Id == 2 || menu.Id == 8)
            //        {
            //            IsPresented = false;
            //            Detail = new NavigationPage(menu.Page);
            //        }
            //    }
            //}
        }

        public class Menu
        {
            public int Id
            {
                get;
                set;
            }
            public string MenuTitle
            {
                get;
                set;
            }
            public string MenuDetail
            {
                get;
                set;
            }

            public ImageSource icon
            {
                get;
                set;
            }

            public Page Page
            {
                get;
                set;
            }
        }
    }
}
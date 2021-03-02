using Reciclas.Tablas;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Reciclas.ViewModels
{
    public class ListarRecojoViewModel : BaseViewModel
    {
        

        private List<Usuario> usuarioCollection;
        public List<Usuario> UsuarioCollection
        {
            get { return usuarioCollection; }
            set { usuarioCollection = value; OnPropertyChanged(); }
        }

        private List<Usuario> selectedusuario;
        public List<Usuario> Selectedusuario
        {
            get { return selectedusuario; }
            set { selectedusuario = value; OnPropertyChanged(); }
        }

        public ListarRecojoViewModel()
        {
            ListarUsuario();
        }

        void ListarUsuario()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Usuario> resultado = ConsultarUsuario(db);

                if (resultado.Count() > 0)
                {
                    UsuarioCollection = new List<Usuario>();
                    UsuarioCollection = (List<Usuario>)resultado;                    
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static IEnumerable<Usuario> ConsultarUsuario(SQLiteConnection db)
        {
            db.CreateTable<Usuario>();
            return db.Query<Usuario>("Select * From Usuario ");
        }
    }
}

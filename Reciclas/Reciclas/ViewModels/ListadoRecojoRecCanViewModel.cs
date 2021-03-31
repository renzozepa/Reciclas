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
    public class ListadoRecojoRecCanViewModel : BaseViewModel
    {
        public ICommand ActualizarCommand { get; set; }
        private List<Recojo> recojoCollection;
        public List<Recojo> RecojoCollection
        {
            get { return recojoCollection; }
            set { recojoCollection = value; OnPropertyChanged(); }
        }

        private List<Recojo> selectedrecojo;
        public List<Recojo> Selectedrecojo
        {
            get { return selectedrecojo; }
            set
            {
                selectedrecojo = value;
                OnPropertyChanged();
            }
        }

        public ListadoRecojoRecCanViewModel()
        {
            ListarRecojo();
            ActualizarCommand = new Command(() => ListarRecojoRefresh());
        }
        public void ListarRecojo()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Recojo> resultado = ConsultarRecojo(db);

                if (resultado.Count() > 0)
                {
                    RecojoCollection = new List<Recojo>();
                    RecojoCollection = (List<Recojo>)resultado;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ListarRecojoRefresh()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Recojo> resultado = ConsultarRecojo(db);

                if (resultado.Count() > 0)
                {
                    RecojoCollection = new List<Recojo>();
                    RecojoCollection = (List<Recojo>)resultado;
                }
                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                throw;
            }
        }

        public static IEnumerable<Recojo> ConsultarRecojo(SQLiteConnection db)
        {
            db.CreateTable<Recojo>();
            return db.Query<Recojo>("Select * From Recojo Where ID_ESTADO > 2");
        }

    }
}

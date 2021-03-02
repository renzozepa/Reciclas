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
    public class ListarRecojoPenViewModel : BaseViewModel
    {
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

        public ListarRecojoPenViewModel()
        {
            ListarRecojoPen();
        }

        public void ListarRecojoPen()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Recojo> resultado = ConsultarRecojoPen(db);

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

        public static IEnumerable<Recojo> ConsultarRecojoPen(SQLiteConnection db)
        {
            db.CreateTable<Recojo>();
            return db.Query<Recojo>("Select * From Recojo ");
        }

    }
}

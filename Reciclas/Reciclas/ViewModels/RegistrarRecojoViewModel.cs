using Reciclas.Models;
using Reciclas.ServicioApi;
using Plugin.Connectivity;
using Reciclas.Tablas;
using Reciclas.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Reciclas.ViewModels
{
    public class RegistrarRecojoViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand MapsCommand { get; }
        public ICommand GuardarRecojoCommand { get; }

        List<RecojoApi> LstRecojoApi = null;

        private string _codigo;
        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; OnPropertyChanged(); }
        }

        private string _direccion;
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; OnPropertyChanged(); }
        }

        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; OnPropertyChanged(); }
        }

        private string _selectedIndexHorario;
        public string SelectedIndexHorario
        {
            get { return _selectedIndexHorario; }
            set {
                SetProperty(ref _selectedIndexHorario , value);
                SelectedDescripcionHorario = _lstHorariodisponible[Convert.ToInt32(_selectedIndexHorario)].DESCRIPCION.ToString();
                OnPropertyChanged();
            }
        }

        private string _selectedDescripcionHorario;
        public string SelectedDescripcionHorario
        {
            get { return _selectedDescripcionHorario; }
            set
            {
                SetProperty(ref _selectedDescripcionHorario, value);
                OnPropertyChanged();
            }
        }


        public List<HorarioApi> _lstHorariodisponible = new List<HorarioApi>();
        public List<HorarioApi> LstHorariodisponible
        {
            get { return _lstHorariodisponible; }
            set
            {
                if (_lstHorariodisponible != value)
                {
                    _lstHorariodisponible = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<HorarioApi> _lstIdHorariodisponible = new List<HorarioApi>();
        public List<HorarioApi> LstIdHorariodisponible
        {
            get { return _lstIdHorariodisponible; }
            set
            {
                //_myregion = _myregion ?? new List<string>();
                if (_lstIdHorariodisponible != value)
                {
                    _lstIdHorariodisponible = value;
                    OnPropertyChanged();
                }
            }
        }

        public RegistrarRecojoViewModel(INavigation navigation)
        {            
            Navigation = navigation;
            NroPedido();
            HorarioDisponible();
            MapsCommand = new Command(async () => await OpenLocation());
            GuardarRecojoCommand = new Command(() => GuardarRecojo());
        }
        async Task OpenLocation()
        {
            await Map.OpenAsync(double.Parse("-12.0135681"), double.Parse("-77.031494"), new MapLaunchOptions
            {
                Name = "Casa",
                NavigationMode = NavigationMode.Default
            });
        }
        public static IEnumerable<Recojo> ConsultarIdMaxRecojo(SQLiteConnection db)
        {
            db.CreateTable<Recojo>();
            return db.Query<Recojo>("Select ( MAX(IFNULL(ID,0)) + 1 ) as ID From Recojo ");
        }

        void NroPedido()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                db.CreateTable<Recojo>();
                IEnumerable<Recojo> resultado = ConsultarIdMaxRecojo(db);
                if (resultado.Count() > 0)
                {
                    List<Recojo> listll = (List<Recojo>)resultado;
                    foreach (Recojo itemRecojo in listll)
                    {
                        Codigo = itemRecojo.ID.ToString() == "0" ? "1" : itemRecojo.ID.ToString();
                        
                    }
                }
                else
                {
                    Codigo = "1";
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        void HorarioDisponible()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Usuario> resultado = ConsultarUsuarioLogeado(db);

                if (resultado.Count() > 0)
                {
                    List<Usuario> listll = (List<Usuario>)resultado;

                    foreach (Usuario itemLoginLocal in listll)
                    {
                        LstHorariodisponible = new List<HorarioApi>();
                        string var_zipcode = string.IsNullOrEmpty(itemLoginLocal.ZIPCODE.ToString()) ? "15096" : itemLoginLocal.ZIPCODE.ToString();
                        var t = Task.Run(async () => LstHorariodisponible = await HaugApi.Metodo.GetHorarioDisponible(var_zipcode));
                        t.Wait();
                        Direccion = itemLoginLocal.DIRECCION.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
                    
        }

        public static IEnumerable<Usuario> ConsultarUsuarioLogeado(SQLiteConnection db)
        {
            db.CreateTable<Usuario>();
            return db.Query<Usuario>("Select * From Usuario where ID = ?", App.Id_Usuario);
        }

        void GuardarRecojo()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Usuario> resultado = ConsultarUsuarioLogeado(db);
                if (resultado.Count() > 0)
                {
                    List<Usuario> listll = (List<Usuario>)resultado;

                    foreach (Usuario itemLoginLocal in listll)
                    {
                        var horario_seleccionado = SelectedDescripcionHorario;
                        var t = Task.Run(async () => LstIdHorariodisponible = await HaugApi.Metodo.GetIdHorarioDisponible(horario_seleccionado.ToString()));
                        t.Wait();

                        string var_token = Convert.ToString(Guid.NewGuid().ToString());

                        var DatosAdd = new Recojo
                        {
                            ID = 0,
                            DESCRIPCION = Descripcion,
                            ID_HORARIO = LstIdHorariodisponible[0].ID,
                            TOKEN_RECOJO = var_token,
                            ID_USUARIO = App.Id_Usuario,
                            FECHA_TRANSACCION = DateTime.Now.Date,
                            ID_ESTADO = 1,
                            ENVIADO = 0,
                            FECHA_ENVIADO = DateTime.Now.Date
                        };
                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            conn.CreateTable<Recojo>();
                            int rpt = conn.Insert(DatosAdd);
                            GuardarRecojoWebApi(rpt, Descripcion, LstIdHorariodisponible[0].ID, Direccion, var_token);
                            App.Current.MainPage.DisplayAlert("Agregar", "Datos registrados correctamente.", "Aceptar");
                            Limpiar();
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        void GuardarRecojoWebApi(int recoj,string descripcion, int horario, string direccion, string token_recojo)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstRecojoApi = new List<RecojoApi>();
                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<Usuario> resultado = ConsultarUsuarioLogeado(db);

                    foreach (Usuario UsuarioItem in resultado)
                    {
                        var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClientRecojo(descripcion,horario,direccion,token_recojo,UsuarioItem.TOKEN));
                        t.Wait();
                    }

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var Var_Recojo = conn.Table<Recojo>().FirstOrDefault(j => j.ID == recoj);
                        Var_Recojo.ENVIADO = 1;
                        Var_Recojo.FECHA_ENVIADO = DateTime.Now.Date;
                        conn.Update(Var_Recojo);
                    }
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Error", "Verifique su conexion a internet", "Ok");
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", "Error : " + ex.InnerException.ToString(), "Ok");
            }
        }
        void Limpiar()
        {
            NroPedido();
            Direccion = string.Empty;
        }


    }
}

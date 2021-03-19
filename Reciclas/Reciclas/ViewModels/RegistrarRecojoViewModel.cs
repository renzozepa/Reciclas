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
        public ICommand GetUbiActCommand { get; }
        public ICommand GetUbiSavCommand { get; }

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

        private string _latitud;
        public string Latitud
        {
            get { return _latitud; }
            set { _latitud = value; OnPropertyChanged(); }
        }
        private string _longitud;
        public string Longitud
        {
            get { return _longitud; }
            set { _longitud = value; OnPropertyChanged(); }
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
            set
            {
                SetProperty(ref _selectedIndexHorario, value);
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
            GetUbiActCommand = new Command(async () => await OpenLocation());
            GetUbiSavCommand = new Command(() => DireccionLogin());
        }
        async Task OpenLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var gps = await Geolocation.GetLocationAsync(request);
            var placemarks = await Geocoding.GetPlacemarksAsync(gps.Latitude, gps.Longitude);
            var placemark = placemarks?.FirstOrDefault();

            Latitud = gps.Latitude.ToString();
            Longitud = gps.Longitude.ToString();
            Direccion = placemark.FeatureName.ToString();

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

        void DireccionLogin()
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
                        Latitud = itemLoginLocal.LATITUD.ToString();
                        Longitud = itemLoginLocal.LONGITUD.ToString();
                        Direccion = itemLoginLocal.DIRECCION.ToString();
                    }
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
                        Latitud = itemLoginLocal.LATITUD.ToString();
                        Longitud = itemLoginLocal.LONGITUD.ToString();
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
                            HORARIO = horario_seleccionado.ToString(),
                            TOKEN_RECOJO = var_token,
                            ID_USUARIO = App.Id_Usuario,
                            FECHA_TRANSACCION = DateTime.Now.Date,
                            ID_ESTADO = 1,
                            ENVIADO = 0,
                            FECHA_ENVIADO = DateTime.Now.Date,
                            LATITUD = Latitud,
                            LONGITUD = Longitud
                        };
                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            conn.CreateTable<Recojo>();
                            int rpt = conn.Insert(DatosAdd);
                            GuardarRecojoWebApi(rpt, Descripcion, LstIdHorariodisponible[0].ID, Direccion, var_token, Latitud, Longitud);
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

        void GuardarRecojoWebApi(int recoj, string descripcion, int horario, string direccion, string token_recojo, string latitud, string longitud)
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
                        var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClientRecojo(descripcion, horario, direccion, token_recojo, UsuarioItem.TOKEN, latitud, longitud));
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

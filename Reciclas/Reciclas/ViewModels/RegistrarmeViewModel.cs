using Plugin.Connectivity;
using Reciclas.Models;
using Reciclas.ServicioApi;
using Reciclas.Tablas;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Reciclas.ViewModels
{
    public class RegistrarmeViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand RegresarCommand { get; set; }
        public ICommand RegistrarmeCommand { get; set; }

        public static List<UsuarioApi> LstUsuarioApi
        {
            get;
            set;
        }
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }
        private string _usuario;
        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; OnPropertyChanged(); }
        }
        private string _clave;
        public string Clave
        {
            get { return _clave; }
            set { _clave = value; OnPropertyChanged(); }
        }
        private string _confirm_clave;
        public string Confirm_clave
        {
            get { return _confirm_clave; }
            set { _confirm_clave = value; OnPropertyChanged(); }
        }
        private string _direccion;
        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; OnPropertyChanged(); }
        }
        private string _celular;
        public string Celular
        {
            get { return _celular; }
            set { _celular = value; OnPropertyChanged(); }
        }
        private string _zipcode;
        public string ZipCode
        {
            get { return _zipcode; }
            set { _zipcode = value; OnPropertyChanged(); }
        }

        public RegistrarmeViewModel(INavigation navigation)
        {
            Navigation = navigation;
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Usuario>();
            }
            RegresarCommand = new Command(async () => await Regresar());
            RegistrarmeCommand = new Command(async () => await Registrarme());
        }
        async Task Regresar()
        {
            await Navigation.PopAsync();
        }
        async Task Registrarme()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Usuario>();
                
                IEnumerable<Usuario> resultado = ValidarUsuarioExistente(conn, Usuario);
                if (resultado.Count() >= 1)
                {
                    Limpiar();
                    await App.Current.MainPage.DisplayAlert("Registrar usuario", "Usuario existente.", "Aceptar");
                }
                else
                {
                    if (string.Equals(Clave.ToString().Trim(), Confirm_clave.ToString().Trim()))
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                        var gps = await Geolocation.GetLocationAsync(request);
                        var placemarks = await Geocoding.GetPlacemarksAsync(gps.Latitude, gps.Longitude);
                        var placemark = placemarks?.FirstOrDefault();

                        var DatosRegistro = new Usuario
                        {
                            NOMBRE = Nombre,
                            USUARIO = Usuario,
                            CONTRASENIA = Clave,
                            DIRECCION = Direccion,
                            CELULAR = Celular,
                            TOKEN = Convert.ToString(Guid.NewGuid().ToString()),
                            ID_PERFIL = "3",
                            LATITUD = gps.Latitude.ToString(),
                            LONGITUD = gps.Longitude.ToString(),
                            FECHA_REGISTRO = DateTime.Now,
                            ALTA = 0,
                            ENVIADO_ALTA = false,
                            ZIPCODE = placemark.PostalCode.ToString() ?? ""
                        };

                        
                        conn.Insert(DatosRegistro);
                        await App.Current.MainPage.DisplayAlert("Registrar usuario", "Datos registrados correctamente.", "Aceptar");
                        RegistrarWebApi(Usuario);
                        Limpiar();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Registrar usuario", "Contraseñas ingresadas no coinciden. Revisar.", "Aceptar");
                    }
                }
            }
        }
        public static IEnumerable<Usuario> ValidarUsuarioExistente(SQLiteConnection db, string usuario)
        {
            return db.Query<Usuario>("Select * From Usuario where USUARIO = ? ", usuario);
        }
        void RegistrarWebApi(string ParUsuario)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstUsuarioApi = new List<UsuarioApi>();
                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<Usuario> resultado = ValidarUsuarioExistente(db,Usuario);

                    foreach (Usuario UsuarioItem in resultado)
                    {
                        var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClient(
                            UsuarioItem.NOMBRE, UsuarioItem.USUARIO, UsuarioItem.CONTRASENIA,
                            UsuarioItem.DIRECCION, UsuarioItem.LATITUD, UsuarioItem.LONGITUD,
                            UsuarioItem.ID_PERFIL, UsuarioItem.TOKEN, UsuarioItem.FECHA_REGISTRO,
                            UsuarioItem.ALTA, UsuarioItem.FECHA_ALTA, UsuarioItem.CELULAR,UsuarioItem.ZIPCODE));
                        t.Wait();
                    }

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var Usuario = conn.Table<Usuario>().FirstOrDefault(j => j.USUARIO == ParUsuario);
                        Usuario.ENVIADO_ALTA = true;
                        conn.Update(Usuario);
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
            Nombre = string.Empty;
            Usuario = string.Empty;
            Clave = string.Empty;
            Confirm_clave = string.Empty;
            Direccion = string.Empty;
            Celular = string.Empty;
        }        
    }
}

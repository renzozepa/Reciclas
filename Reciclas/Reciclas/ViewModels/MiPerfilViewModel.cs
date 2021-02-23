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
    public class MiPerfilViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand ActualizarCommand { get; set; }


        private ObservableCollection<Perfil> perfilCollection;
        public ObservableCollection<Perfil> PerfilCollection
        {
            get { return perfilCollection; }
            set { perfilCollection = value; }
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

        private string _perfil;
        public string Perfil
        {
            get { return _perfil; }
            set { _perfil = value; OnPropertyChanged(); }
        }

        private string _alta;
        public string Alta
        {
            get { return _alta; }
            set { _alta = value; OnPropertyChanged(); }
        }

        private string _fecha_alta;
        public string Fecha_Alta
        {
            get { return _fecha_alta; }
            set { _fecha_alta = value; OnPropertyChanged(); }
        }

        private string _token;
        public string Token
        {
            get { return _token; }
            set { _token = value; OnPropertyChanged(); }
        }

        private string _zipcode;
        public string ZipCode
        {
            get { return _zipcode; }
            set { _zipcode = value; OnPropertyChanged(); }
        }

        public MiPerfilViewModel(INavigation navigation)
        {
            Navigation = navigation;
            perfilCollection = new ObservableCollection<Perfil>
            {
                new Perfil() { DESCRIPCION = "Admin" },
                new Perfil() { DESCRIPCION = "Recolector" },
                new Perfil() { DESCRIPCION = "Usuario" }
            };

            CargarDatosUsuario();
            ActualizarCommand = new Command(() => actualizarZipCode());
            //ActualizarCommand = new Command(async () => await actualizarZipCode2());
        }

        async Task actualizarZipCode2()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var gps = await Geolocation.GetLocationAsync(request);

            var placemarks = await Geocoding.GetPlacemarksAsync(gps.Latitude, gps.Longitude);
            var placemark = placemarks?.FirstOrDefault();
            string rpt = "Hola";

        }
        void actualizarZipCode()
        {
            try
            {                
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var perfil = conn.Table<Usuario>().FirstOrDefault(j => j.ID == App.Id_Usuario);
                    if (perfil == null)
                    {
                        throw new Exception("Usuario no encontrado en la base de datos!");
                    }

                    //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    //var gps = await Geolocation.GetLocationAsync(request);

                    //var placemarks = await Geocoding.GetPlacemarksAsync(gps.Latitude, gps.Longitude);
                    //var placemark = placemarks?.FirstOrDefault();
                    //if (placemark != null)
                    //{
                    //    perfil.TOKEN = Convert.ToString(Guid.NewGuid().ToString());
                    //    perfil.LATITUD = gps.Latitude.ToString();
                    //    perfil.LONGITUD = gps.Longitude.ToString();
                    //    perfil.ZIPCODE = placemark.PostalCode.ToString() ?? "";
                    //    conn.Update(perfil);
                    //}

                    perfil.NOMBRE = Nombre;
                    perfil.USUARIO = Usuario;
                    perfil.DIRECCION = Direccion;
                    perfil.CELULAR = Celular;
                    perfil.TOKEN = "57411cd1-0ba6-47b4-a8b1-c0d78537d674";
                    conn.Update(perfil);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error : " + ex.InnerException.ToString());
            }
            
        }
        void CargarDatosUsuario()
        {
            var db = new SQLiteConnection(App.FilePath);
            IEnumerable<Usuario> resultado = ConsultarUsuario(db);
            if (resultado.Count() > 0)
            {
                List<Usuario> listll = (List<Usuario>)resultado;
                foreach (Usuario itemLoginLocal in listll)
                {
                    Nombre = itemLoginLocal.NOMBRE;
                    Usuario = itemLoginLocal.USUARIO;
                    Direccion = itemLoginLocal.DIRECCION;
                    Latitud = itemLoginLocal.LATITUD;
                    Longitud = itemLoginLocal.LONGITUD;
                    Celular = itemLoginLocal.CELULAR;
                    Perfil = itemLoginLocal.ID_PERFIL == "1" ? "Admin" : itemLoginLocal.ID_PERFIL == "2" ? "Recolector" : "Usuario";
                    Alta = itemLoginLocal.ALTA.ToString();
                    Fecha_Alta = itemLoginLocal.FECHA_ALTA.ToShortDateString();
                    Token = itemLoginLocal.TOKEN;
                    ZipCode = itemLoginLocal.ZIPCODE;
                }
            }
        }
        public static IEnumerable<Usuario> ConsultarUsuario(SQLiteConnection db)
        {
            db.CreateTable<Usuario>();
            return db.Query<Usuario>("Select * From Usuario where ID = ? ", App.Id_Usuario);
        }
    }
}

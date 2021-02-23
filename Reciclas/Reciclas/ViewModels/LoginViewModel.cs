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


namespace Reciclas.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegristrarCommand { get; set; }
        public ICommand UsuariosCommand { get; set; }

        public static List<UsuarioApi> LstUsuarioApi { get; set; }

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

        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginCommand = new Command(async () => await ValidarUsuario());
            RegristrarCommand = new Command(async () => await Registrarme());
            UsuariosCommand = new Command(async () => await ListarUsuarios());
        }
        async Task ValidarUsuario()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<Usuario> resultado = ConsultarUsuario(db, Usuario, Clave);
                if (resultado.Count() > 0)
                {
                    List<Usuario> listll = (List<Usuario>)resultado;
                    foreach (Usuario itemLoginLocal in listll)
                    {
                        App.Id_Usuario = Convert.ToInt32(itemLoginLocal.ID.ToString());
                        App.Para_alta = itemLoginLocal.ALTA;
                        if (!string.IsNullOrEmpty(itemLoginLocal.TOKEN))
                        {
                            App.Token = itemLoginLocal.TOKEN.ToString();
                        }
                        else {
                            App.Token = Convert.ToString(Guid.NewGuid().ToString());
                        }
                    }
                    if(App.Para_alta == 0)
                        ValidarAltaUsuario();
                    
                    Limpiar();
                    await App.Current.MainPage.Navigation.PushAsync(new Principal());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Inicio sesión", "Verifique su usuario/contraseña", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error : " + ex.Message.ToString(), "Aceptar");
            }
        }
        void Limpiar()
        {
            Usuario = string.Empty;
            Clave = string.Empty;
        }
        async Task Registrarme()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Registrarme());
        }
        async Task ListarUsuarios()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ConsultaRegistro());
        }
        public static IEnumerable<Usuario> ConsultarUsuario(SQLiteConnection db, string usuario, string contra)
        {
            db.CreateTable<Usuario>();
            return db.Query<Usuario>("Select * From Usuario where USUARIO = ? and CONTRASENIA = ?", usuario, contra);
        }
        
        void ValidarAltaUsuario()
        {            
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstUsuarioApi = new List<UsuarioApi>();
                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<Usuario> resultado = ListarUsuarioPorSincronizarToken(db);

                    foreach (Usuario usuarioitem in resultado)
                    {
                        var t = Task.Run(async () => LstUsuarioApi = await HaugApi.Metodo.GetUsuarioxToken(usuarioitem.TOKEN));
                        t.Wait();
                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            var Usuario = conn.Table<Usuario>().FirstOrDefault(j => j.ID == App.Id_Usuario);
                            if (Usuario == null)
                                App.Current.MainPage.DisplayAlert("Información","Usuario no encontrado.!","Ok");
                            
                            if (LstUsuarioApi != null)
                            {
                                if (LstUsuarioApi.Count > 0)
                                {
                                    Usuario.ALTA = LstUsuarioApi[0].ALTA;
                                    Usuario.FECHA_ALTA = LstUsuarioApi[0].FECHA_ALTA;
                                    Usuario.ID_PERFIL = LstUsuarioApi[0].ID_PERFIL.ToString();
                                    conn.Update(Usuario);
                                }
                            }
                        }
                    }
                }
                else {
                    App.Current.MainPage.DisplayAlert("Error", "Verifique su conexion a internet.", "Ok");
                }
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error","Error : " +  ex.InnerException.ToString(), "Ok");
            }
        }

        public static IEnumerable<Usuario> ListarUsuarioPorSincronizarToken(SQLiteConnection db)
        {
            return db.Query<Usuario>("Select * From Usuario Where ID = ?", App.Id_Usuario);
        }

    }
}

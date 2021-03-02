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
    public class DetalleRecojoViewModel : BaseViewModel
    {
        private Usuario _usuario;

        public Usuario Usuarios
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }

        public DetalleRecojoViewModel(Usuario selectedUsuario)
        {
            Usuarios = selectedUsuario;
        }
    }
}

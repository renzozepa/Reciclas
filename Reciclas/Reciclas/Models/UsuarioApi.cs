using System;
using System.Collections.Generic;
using System.Text;

namespace Reciclas.Models
{
    public class UsuarioApi
    {        
        public int ID { get; set; }        
        public string NOMBRE { get; set; }        
        public string USUARIO1 { get; set; }        
        public string CLAVE { get; set; }        
        public string DIRECCION { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
        public int ID_PERFIL { get; set; }        
        public string TOKEN { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int ALTA { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public string CELULAR { get; set; }
        public string ZIPCODE { get; set; }

    }
}

using SQLite;
using System;

namespace Reciclas.Tablas
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(255)]
        public string NOMBRE { get; set; }
        [MaxLength(255)]
        public string USUARIO { get; set; }
        [MaxLength(255)]
        public string CONTRASENIA { get; set; }
        [MaxLength(255)]
        public string DIRECCION { get; set; }
        [MaxLength(15)]
        public string CELULAR { get; set; }
        public string TOKEN { get; set; }
        public string ID_PERFIL { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int ALTA { get; set; }
        public DateTime FECHA_ALTA { get; set; }
        public bool ENVIADO_ALTA { get; set; }
        public string ZIPCODE { get; set; }
    }
}

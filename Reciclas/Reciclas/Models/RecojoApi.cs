namespace Reciclas.Models
{
    using System;
    using System.Collections.Generic;

    public class RecojoApi
    {
        public int ID { get; set; }        
        public string DESCRIPCION { get; set; }
        public int? ID_HORARIO { get; set; }
        public string DIRECCION { get; set; }
        public string TOKEN_RECOJO { get; set; }
        public string TOKEN_USUARIO { get; set; }
        public DateTime? FECHA_TRANSACCION { get; set; }
        public int? ID_ESTADO { get; set; }
    }
}

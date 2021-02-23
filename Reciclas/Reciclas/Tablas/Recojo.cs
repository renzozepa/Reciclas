﻿using SQLite;
using System;

namespace Reciclas.Tablas
{
    public class Recojo
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [MaxLength(100)]
        public string DESCRIPCION { get; set; }

        public int ID_HORARIO { get; set; }
        public string TOKEN_RECOJO { get; set; }
        public int ID_USUARIO { get; set; }
        public DateTime FECHA_TRANSACCION { get; set; }
        public int ID_ESTADO { get; set; }
        public int ENVIADO { get; set; }
        public DateTime FECHA_ENVIADO { get; set; }


    }
}

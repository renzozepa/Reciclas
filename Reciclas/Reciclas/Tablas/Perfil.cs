using SQLite;

namespace Reciclas.Tablas
{
    public class Perfil
    {
        [PrimaryKey]
        public int ID { get; set; }
        [MaxLength(255)]
        public string DESCRIPCION { get; set; }
    }
}

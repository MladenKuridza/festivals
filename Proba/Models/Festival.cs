using System.ComponentModel.DataAnnotations;

namespace Mladen_Kuridza.Models
{
    public class Festival
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Minimalna cena je 0")]
        public double Cena { get; set; }
        [Range(1950, 2018, ErrorMessage = "Opseg godina je od 1950-2018")]
        public int Godina { get; set; }

        public Mesto Mesto { get; set; }
        public int MestoId { get; set; }
    }
}

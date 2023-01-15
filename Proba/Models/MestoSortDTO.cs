using System.ComponentModel.DataAnnotations;

namespace Mladen_Kuridza.Models
{
    public class MestoSortDTO
    {
        [MaxLength(5, ErrorMessage = "Maksimalna duzina koda je 5 cifara")]
        public int Kod { get; set; }
    }
}

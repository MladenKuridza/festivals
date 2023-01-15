using System;

namespace Mladen_Kuridza.Models
{
    public class FestivalDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string MestoNaziv { get; set; }
        public int MestoId { get; set; }
        public int Godina { get; set; }
        public double Cena { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FestivalDTO dTO &&
                   Id == dTO.Id &&
                   Naziv == dTO.Naziv &&
                   MestoNaziv == dTO.MestoNaziv &&
                   Godina == dTO.Godina &&
                   Cena == dTO.Cena;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Naziv, MestoNaziv, Godina, Cena);
        }
    }
}

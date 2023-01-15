using Mladen_Kuridza.Models;
using System.Linq;

namespace Mladen_Kuridza.Interfaces
{
    public interface IFestivalRepository
    {
        IQueryable<Festival> GetAll();
        Festival GetById(int id);
        void Add(Festival festival);
        void Update(Festival festival);
        void Delete(Festival festival);
        IQueryable<Festival> GetAllByParameters(int start, int kraj);
        IQueryable<Festival> GetAllPrice(double cena);
        IQueryable<Festival> GetAllByName(string naziv);
        StatisticsDTO GetStatistics();
    }
}

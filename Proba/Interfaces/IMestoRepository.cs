using Mladen_Kuridza.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mladen_Kuridza.Interfaces
{
    public interface IMestoRepository
    {
        IQueryable<Mesto> GetAll();
        Mesto GetById(int id);
        IQueryable<Mesto> SearchMesto(int kod);
    }
}

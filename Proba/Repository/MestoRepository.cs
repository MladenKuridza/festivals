using Mladen_Kuridza.Interfaces;
using Mladen_Kuridza.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mladen_Kuridza.Repository
{
    public class MestoRepository : IMestoRepository
    {
        private readonly AppDbContext _context;

        public MestoRepository(AppDbContext context)
        {
            this._context = context;
        }

        public IQueryable<Mesto> GetAll()
        {
            return _context.Mesta;
        }
        public Mesto GetById(int id)
        {
            return _context.Mesta.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Mesto> SearchMesto(int kod)
        {
            return _context.Mesta.OrderBy(m => m.Kod).Where(e => e.Kod < kod);
        }
    }
}

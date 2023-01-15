using Microsoft.EntityFrameworkCore;
using Mladen_Kuridza.Interfaces;
using Mladen_Kuridza.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mladen_Kuridza.Repository
{
    public class FestivalRepository : IFestivalRepository
    {
        private readonly AppDbContext _context;

        public FestivalRepository(AppDbContext context)
        {
            this._context = context;
        }
        public void Add(Festival festival)
        {
            {
                _context.Festivali.Add(festival);
                _context.SaveChanges();
            }
        }

        public void Delete(Festival festival)
        {
            _context.Festivali.Remove(festival);
            _context.SaveChanges();
        }

        public IQueryable<Festival> GetAll()
        {
            return _context.Festivali.OrderByDescending(c => c.Cena).Include(m => m.Mesto);
        }

        public Festival GetById(int id)
        {
            return _context.Festivali.Include(x => x.Mesto).FirstOrDefault(p => p.Id == id);
        }

        public void Update(Festival festival)
        {
            _context.Entry(festival).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public IQueryable<Festival> GetAllByParameters(int start, int kraj)
        {
            return _context.Festivali.OrderBy(c => c.Godina).Include(x => x.Mesto).Where(c => c.Godina >= start && c.Godina <= kraj);
        }

        public IQueryable<Festival> GetAllByName(string naziv)
        {
            return _context.Festivali.Where(c => c.Naziv.Contains(naziv));
        }

        public IQueryable<Festival> GetAllPrice(double cena)
        {
            return _context.Festivali.OrderByDescending(n => n.Naziv).Include(x => x.Mesto).Where(c => c.Cena <= cena);
        }

        public List<FestivalCenaProsekDTO> GetAllProsek()
        {
            return _context.Festivali.GroupBy(p => p.MestoId).Select(s =>
                new FestivalCenaProsekDTO()
                {
                    Mesto = _context.Mesta.Where(place => place.Id == s.Key).Select(k => k.Naziv).Single(),
                    ProsecnaCena = s.Average(x => x.Cena)
                }
            ).OrderByDescending(x => x.ProsecnaCena).ToList();
        }

        public StatisticsDTO GetStatistics()
        {
            return new StatisticsDTO()
            {
                Pregled = GetAllProsek()
            };
        }
    }
}

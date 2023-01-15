using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mladen_Kuridza.Interfaces;
using Mladen_Kuridza.Models;
using System.Linq;

namespace Mladen_Kuridza.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FestivaliController : ControllerBase
    {
        private readonly IFestivalRepository _festivalRepository;
        private readonly IMestoRepository _mestoRepository;
        private readonly IMapper _mapper;

        public FestivaliController(IFestivalRepository festivalRepository, IMapper mapper, IMestoRepository mestoRepository)
        {
            _festivalRepository = festivalRepository;
            _mapper = mapper;
            _mestoRepository = mestoRepository;
        }

        //Prikazuje sve festivale poredjane po ceni opadajuce
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SviFestivali()
        {
            return Ok(_festivalRepository.GetAll().ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());

        }

        //Prikazuje festival ciji Id odgovara prosledjenom Id-u
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Festival(int id)
        {
            var festival = _festivalRepository.GetById(id);

            if (festival == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FestivalDTO>(festival));
        }

        //Edituje festival ciji Id odgovara prosledjenom Id-u
        [HttpPut("{id}")]
        public IActionResult PutFestival(int id, Festival festival)
        {
            if (id != festival.Id)
            {
                return BadRequest();
            }

            try
            {
                _festivalRepository.Update(festival);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(festival);
        }

        //Dodavanje festivala, ukoliko Id vec postoji, umesto dodavanja novog festivala radi se edit postojeceg
        [HttpPost]
        public IActionResult PostFestival(Festival festival)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _festivalRepository.Add(festival);
            return CreatedAtAction("Festival", new { id = festival.Id }, festival);
        }

        //Brisanje festivala ciji Id smo prosledili
        [HttpDelete("{id}")]
        public IActionResult DeleteFestival(int id)
        {
            var festival = _festivalRepository.GetById(id);
            if (festival == null)
            {
                return NotFound();
            }

            _festivalRepository.Delete(festival);
            return NoContent();
        }

        //Prikazuje festivale cija godina pocetka i kraja odrzavanja odgovara opsegu godina koji je prosledjen(start i kraj). Poredjano po godini odrzavanja rastuce
        [Route("/api/Festivali/pretraga")]
        [HttpPost]
        public IActionResult SearchRange(FestivalSortDTO filter)
        {
            if (filter.Start < 1950 || filter.Kraj > 2018 || filter.Start > filter.Kraj)
            {
                return BadRequest();
            }
            return Ok(_festivalRepository.GetAllByParameters(filter.Start, filter.Kraj).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());

        }

        //Prikazuje festivale cija cena je manja ili jednaka u odnosu na vrednost koja je prosledjena. Poredjano po nazivu opadajuce
        [Route("/api/Festivali/cena")]
        [HttpGet]
        public IActionResult SearchCena(double cena)
        {
            if (cena < 0)
            {
                return BadRequest();
            }
            return Ok(_festivalRepository.GetAllPrice(cena).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());

        }

        //Prikazuje festivale cije ime sadrzi tekst(string) koji je prosledjen
        [Route("/api/Festivali/imena")]
        [HttpGet]
        public IActionResult SearchNames(string naziv)
        {
            if (string.IsNullOrEmpty(naziv))
            {
                return BadRequest();
            }
            return Ok(_festivalRepository.GetAllByName(naziv).ProjectTo<FestivalDTO>(_mapper.ConfigurationProvider).ToList());

        }

        //Prikazuje prosecnu cenu festivala u nekom mestu. Poredjano po prosecnoj ceni opadajuce
        [Route("/api/Prosek")]
        [HttpGet]
        public IActionResult GetStatistics()
        {
            return Ok(_festivalRepository.GetStatistics());
        }
    }
}

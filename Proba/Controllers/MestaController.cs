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
    public class MestaController : ControllerBase
    {
        private readonly IMestoRepository _mestoRepository;

        private readonly IMapper _mapper;


        public MestaController(IMestoRepository mestoRepository, IMapper mapper)
        {
            _mestoRepository = mestoRepository;
            _mapper = mapper;
        }

        //Prikazuje sva mesta 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Mesta()
        {

            return Ok(_mestoRepository.GetAll().ToList());
        }

        //Prikazuje mesto ciji Id odgovara prosledjenom Id-u
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public IActionResult Mesta(int id)
        {
            var mesto = _mestoRepository.GetById(id);
            if (mesto == null)
            {
                return NotFound();
            }

            return Ok(mesto);
        }

        //Prikazuje postanske kodove koji su manji u odnosu na vrednost koda koja je prosledjena. Kodovi su poredjani po postanskom kodu rastuce
        [Route("/api/Mesta/zipKod")]
        [HttpGet]
        public IActionResult Search(int kod)
        {

            if (kod.ToString().Length < 5)
            {
                return BadRequest();
            }
            return Ok(_mestoRepository.SearchMesto(kod).ToList());
        }

    }
}

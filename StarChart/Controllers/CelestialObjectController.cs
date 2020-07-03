using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestial = _context.CelestialObjects.Find(id);
            if (celestial == null)
                return NotFound();

            celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();

            return Ok(celestial);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(c => c.Name.Equals(name)).ToList();

            if (!celestialObjects.Any())
                return NotFound();

            foreach (var celestial in celestialObjects)
            {
                celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();
            }

            return Ok(celestialObjects.ToList());
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestial in celestialObjects)
            {
                celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}

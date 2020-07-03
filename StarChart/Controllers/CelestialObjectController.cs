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
            CelestialObject celestial = _context.CelestialObjects.Where(c => c.Id == id).First();
            if (celestial == null)
                return NotFound();

            celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();

            return Ok(celestial);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            CelestialObject celestial = _context.CelestialObjects.Where(c => c.Name.Equals(name)).First();
            if (celestial == null)
                return NotFound();

            celestial.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();

            return Ok(celestial);
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

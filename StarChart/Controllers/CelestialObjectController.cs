using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Edm;
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
            var celestial = _context.CelestialObjects.Find(id);
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestial)
        {
            _context.CelestialObjects.Add(celestial);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestial.Id }, celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]CelestialObject celestialObject)
        {
            var celestial = _context.CelestialObjects.Find(id);
            if (celestial == null)
                return NotFound();

            celestial.Name = celestialObject.Name;
            celestial.OrbitalPeriod = celestialObject.OrbitalPeriod;
            celestial.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestial = _context.CelestialObjects.Find(id);
            if (celestial == null)
                return NotFound();

            celestial.Name = name;

            _context.CelestialObjects.Update(celestial);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestials = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if (!celestials.Any())
                return NotFound();

            _context.CelestialObjects.RemoveRange(celestials);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

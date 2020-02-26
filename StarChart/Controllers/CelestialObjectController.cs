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

        //        [ ] Create all `CelestialObjectController`'s Get actions
        //  - [ ] Create a new method `GetById` 
        //    - This method should have a return type of `IActionResult` 
        //    - This method should accept a parameter of type `int` named `id`. 
        //    - This method should have an `HttpGet` attribute with an value of `"{id:int}"` and the `Name` property set to `"GetById"`. 
        //    - This method should return `NotFound` there is no `CelestialObject` with an `Id` property that matches the parameter.
        //    - This method should also set the `Satellites` property to any `CelestialObjects` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
        //    - This method should return an `Ok` with a value of the `CelestialObject` who's `Id` property matches the `id` parameter.
        //  - [ ] Create the `GetByName` method
        //    - This method should have a return type of `IActionResult` 
        //    - This method should accept a parameter of type `string` named `name`. 
        //    - This method should have an `HttpGet` attribute with a value of `"{name}"`.
        //    - This method should return `NotFound` there is no `CelestialObject` with an `Name` property that matches the `name` parameter.
        //    - This method should also set the `Satellites` property for each `CelestialObject` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
        //    - This method should return an `Ok` with a value of the list of `CelestialObject` who's `Name` property matches the `name` parameter.
        //  - [ ] Create the `GetAll` method
        //    - This method should have a return type of `IActionResult`.
        //    - This method should also set the `Satellites` property for each of the `CelestialObject`s returned.
        //    - This method should have an `HttpGet` attribute.
        //    - This method should return `Ok` with a value of all `CelestialObjects`s.
        //- [ ] Create `CelestialObjectControllers`'s Post, Put, Patch, and Delete actions.

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (celestialObject == null) return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (celestialObjects.Count == 0) return NotFound();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }
        [HttpPost]

        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute(
                "GetById",
                new
                {
                    id = celestialObject.Id
                },
                celestialObject
                );
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject newCelestialObject)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null) return NotFound();

            celestialObject.Name = newCelestialObject.Name;
            celestialObject.OrbitalPeriod = newCelestialObject.OrbitalPeriod;
            celestialObject.OrbitedObjectId = newCelestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (celestialObject == null) return NotFound();

            celestialObject.Name = name;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Id == id);
            if (celestialObjects.Count() == 0) return NotFound();

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();

        }
    }
}

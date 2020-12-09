using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using DirectorioAPI.Controllers.FilterModels;
using DirectorioAPI.DAOs;
using DirectorioAPI.NeoDB.Exceptions;
using DirectorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DirectorioAPI.Controllers
{
    [Route("api/personas")]
    [ApiController]
    public class PersonasController : ControllerBase
    {

        // GET: api/personas/
        // GET: api/personas?conocimiento=Java
        [HttpGet]
        public IActionResult Get([FromQuery] PersonaFilter filter)
        {

            var listaPersonas = filter.exec();
            return Content(JsonConvert.SerializeObject(listaPersonas), "application/json");

        }

        // GET: api/personas/1999...
        [HttpGet("{uid}")]
        public IActionResult Get([FromRoute] string uid)
        {

            try
            {
                var persona = PersonasDAO.getInstance().findByUid(uid);
                return Content(JsonConvert.SerializeObject(persona), "application/json");
            }
            catch (PersonaNotFoundException) {
                return NotFound();
            }

        }

        // POST: api/personas
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement json)
        {

            try
            {
                var nombre = json.GetProperty("nombre").GetString();
                var dni = json.GetProperty("dni").GetString();
                var email = json.GetProperty("email").GetString();

                Persona persona = PersonasDAO.getInstance().insert(nombre, dni, email);
                return Content(JsonConvert.SerializeObject(persona), "application/json");

            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

        }

        // PUT api/personas/1999...
        /*[HttpPut("{uid}")]
        public IActionResult Put(string uid, [FromBody] JsonElement json)
        {

            try
            {
                var name = json.GetProperty("nombre").GetString();
                var dni = json.GetProperty("dni").GetString();
                var email = json.GetProperty("email").GetString();

                Person person = PersonDAO.getInstance().insert(name, dni, email);
                return Content(JsonConvert.SerializeObject(person), "application/json");

            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

        }*/

        // DELETE api/personas/1999...
        [HttpDelete("{uid}")]
        public IActionResult Delete(string uid)
        {

            try {

                var persona = PersonasDAO.getInstance().deleteByUid(uid);
                return Content(JsonConvert.SerializeObject(persona), "application/json");

            }
            catch (PersonaNotFoundException) {
                return NotFound();
            }


        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DirectorioAPI.DAOs;
using DirectorioAPI.Models;
using DirectorioAPI.NeoDB.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace DirectorioAPI.Controllers
{
    [Route("api/conocimientos")]
    [ApiController]
    public class ConocimientosController : ControllerBase
    {
        // GET: api/conocimientos
        [HttpGet]
        public IActionResult Get()
        {

            var listaConocimientos = ConocimientosDAO.getInstance().listAll();
            return Content(JsonConvert.SerializeObject(listaConocimientos), "application/json");

        }

        // GET: api/conocimientos/1999...
        [HttpGet("{uid}")]
        public IActionResult Get(string uid)
        {

            try
            {
                var conocimiento = ConocimientosDAO.getInstance().findByUid(uid);
                return Content(JsonConvert.SerializeObject(conocimiento), "application/json");
            }
            catch (ConocimientoNotFoundException)
            {
                return NotFound();
            }

        }

        // POST: api/conocimientos
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement json)
        {

            try
            {
                var nombre = json.GetProperty("nombre").GetString();

                Conocimiento conocimiento = ConocimientosDAO.getInstance().insert(nombre);
                return Content(JsonConvert.SerializeObject(conocimiento), "application/json");

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

        // PUT: api/conocimientos/1999...
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/conocimientos/1999...
        [HttpDelete("{uid}")]
        public IActionResult Delete(string uid)
        {

            try
            {

                var conocimiento = PersonasDAO.getInstance().deleteByUid(uid);
                return Content(JsonConvert.SerializeObject(conocimiento), "application/json");

            }
            catch (ConocimientoNotFoundException)
            {
                return NotFound();
            }

        }
    }
}

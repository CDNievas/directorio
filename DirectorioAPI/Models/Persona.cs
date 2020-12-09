using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioAPI.Models
{
    public class Persona
    {

        public int id { get; set; }

        public string uid { get; set; }
        public string nombre { get; set; }
        public string dni { get; set; }
        public string email { get; set; }

        public List<Conocimiento> conocimientos = new List<Conocimiento>();

        public Persona(int id, string uid, string nombre, string dni, string email)
        {
            this.id = id;
            this.uid = uid;
            this.nombre = nombre;
            this.dni = dni;
            this.email = email;
        }

        public void addConocimiento(Conocimiento conocimiento)
        {
            this.conocimientos.Add(conocimiento);
        }

    }

}

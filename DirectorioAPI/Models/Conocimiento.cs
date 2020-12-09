using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioAPI.Models
{
    public class Conocimiento
    {

        public int id {get;set;}
        public string uid { get; set; }
        public string nombre { get; set; }

        public List<Conocimiento> subconocimientos = new List<Conocimiento>();

        public Conocimiento(int id, string uid, string nombre)
        {
            this.id = id;
            this.uid = uid;
            this.nombre = nombre;
        }

        public void addSubconocimiento(Conocimiento conocimiento)
        {
            this.subconocimientos.Add(conocimiento);
        }

    }
}

using DirectorioAPI.DAOs;
using DirectorioAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioAPI.Controllers.FilterModels
{
    public class PersonaFilter
    {

        public String conocimiento { get; set; }
        
        
        public List<Persona> exec()
        {
            if(conocimiento != null) {
                return PersonasDAO.getInstance().findByConocimiento(conocimiento);
            } else {
                return PersonasDAO.getInstance().listAll();
            }

        }


    }
}

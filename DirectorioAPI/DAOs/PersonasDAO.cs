using DirectorioAPI.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectorioAPI.NeoDB;

namespace DirectorioAPI.DAOs
{
    public class PersonasDAO
    {

        public static PersonasDAO instance = new PersonasDAO();

        private PersonasDAO() { }

        public static PersonasDAO getInstance() { return PersonasDAO.instance; }

        public Persona insert(string nombre, string dni, string email)
        {
            string uid = NewId.Next().ToString("D").ToUpperInvariant();
            var id = NeoDBConn.getConn().insertPersona(uid, nombre, dni, email);
            return new Persona(id, uid, nombre, dni, email);
        }

        public Persona findByUid(string uid)
        {

            var resultado = NeoDBConn.getConn().findPersonaByUid(uid);
            return this.getPersona(resultado);

        }


        public List<Persona> findByConocimiento(string conocimiento)
        {

            var jobjResultados = NeoDBConn.getConn().findPersonaByConocimiento(conocimiento);

            var resultados = new List<Persona>();
            foreach (var jobjResultado in jobjResultados)
            {
                resultados.Add(this.getPersona(jobjResultado));
            }

            return resultados;

        }

        public List<Persona> listAll()
        {

            var jobjResultados = NeoDBConn.getConn().listAllPersonas();

            var resultados = new List<Persona>();
            foreach (var jobjResultado in jobjResultados)
            {
                resultados.Add(this.getPersona(jobjResultado));
            }

            return resultados;

        }

        public void update(string uid, string nombre, string dni, string email)
        {

            //var result = NeoDBConn.getConn().updatePerson(uid, name, dni, email);


        }

        public Persona deleteByUid(string uid)
        {

            return NeoDBConn.getConn().deletePersonaByUid(uid);

        }

        private Persona getPersona(dynamic obj)
        {

            int id = obj.id;
            string uid = obj.uid;
            string nombre = obj.nombre;
            string dni = obj.dni;
            string email = obj.email;

            var persona = new Persona(id, uid, nombre, dni, email);

            foreach (var objCono in obj.conocimientos)
            {

                int idCono = objCono.id;
                string uidCono = objCono.uid;
                string nombreCono = objCono.nombre;

                var know = new Conocimiento(idCono, uidCono, nombreCono);
                persona.addConocimiento(know);

            }

            return persona;

        }

    }

}
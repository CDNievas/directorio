using DirectorioAPI.NeoDB.Exceptions;
using DirectorioAPI.Models;
using DirectorioAPI.NeoDB;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DirectorioAPI.DAOs
{
    public class ConocimientosDAO
    {

        public static ConocimientosDAO instance = new ConocimientosDAO();

        private ConocimientosDAO() { }

        public static ConocimientosDAO getInstance() { return ConocimientosDAO.instance; }

        public Conocimiento insert(string nombre)
        {

            string uid = NewId.Next().ToString("D").ToUpperInvariant();
            var id = NeoDBConn.getConn().insertConocimiento(uid, nombre);
            return new Conocimiento(id, uid, nombre);
        }

        public Conocimiento findByUid(string uid)
        {

            var resultado = NeoDBConn.getConn().findConocimientoByUid(uid);
            return this.getConocimiento(resultado);

        }

        public List<Conocimiento> listAll()
        {

            var jobjResultados = NeoDBConn.getConn().listAllConocimientos();

            var resultados = new List<Conocimiento>();
            foreach (var jobjResultado in jobjResultados)
            {

                resultados.Add(this.getConocimiento(jobjResultado));

            }

            return resultados;

        }

        public Conocimiento deleteByUid(string uid)
        {

            return NeoDBConn.getConn().deletePersonaByUid(uid);

        }

        private Conocimiento getConocimiento(dynamic obj)
        {

            int id = obj.id;
            string uid = obj.uid;
            string nombre = obj.nombre;

            var conocimiento = new Conocimiento(id, uid, nombre);

            foreach (var objCono in obj.subconocimientos)
            {

                int idCono = objCono.id;
                string uidCono = objCono.uid;
                string nombreCono = objCono.nombre;

                var know = new Conocimiento(idCono, uidCono, nombreCono);
                conocimiento.addSubconocimiento(know);

            }

            return conocimiento;

        }

    }
}

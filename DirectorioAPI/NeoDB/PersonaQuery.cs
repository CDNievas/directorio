using DirectorioAPI.NeoDB.Exceptions;
using Neo4j.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectorioAPI.NeoDB
{
    public partial class NeoDBConn
    {

        // CREATE
        public int insertPersona(string uid, string nombre, string dni, string email)
        {

            using (var session = driver.Session())
            {

                var resultado = session.WriteTransaction(tx =>
                {

                    var resultado = tx.Run(@"CREATE (p:Persona {uid:$uid,nombre:$nombre,dni:$dni,email:$email})
                    RETURN ID(p)", new { uid, nombre, dni, email });

                    var record = resultado.Single();
                    var id = record["ID(p)"].As<int>();

                    return id;

                });

                return resultado;

            }

        }

        // READ
        public dynamic findPersonaByUid(string uid)
        {

            using (var session = driver.Session())
            {

                var resultado = session.ReadTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(p:Persona)
                    OPTIONAL MATCH (p)-[:CONOCE]->(c:Conocimiento)
                    WITH p,{id: ID(c), uid:c.uid, nombre:c.nombre} as conocimiento
                    WHERE p.uid = $uid
                    RETURN {id: ID(p), uid:p.uid, nombre:p.nombre, email:p.email, dni:p.dni, conocimientos:collect(conocimiento)} as obj", new { uid });

                    var record = resultado.SingleOrDefault();
                
                    if (record == null)
                    {
                        throw new PersonaNotFoundException();
                    }
                    else
                    {

                        var obj = record["obj"].As<IDictionary<string, object>>();
                        return this.getJObjPersona(obj);

                    }


                });

                return resultado;

            }

        }

        public List<dynamic> findPersonaByConocimiento(string conocimiento)
        {

            using (var session = driver.Session())
            {

                var resultado = session.ReadTransaction(tx =>
                {

                    var resultado = tx.Run(@"
                        MATCH(p:Persona) -[:CONOCE | CONTIENE * 1..]->(c: Conocimiento { nombre: $conocimiento})
                        MATCH(p) -[:CONOCE]->(c2: Conocimiento)
                        WITH p, { id: ID(c2), uid:c.uid, nombre: c2.nombre} as conocimiento
                        RETURN { id: ID(p), uid:p.uid, nombre:p.nombre, email:p.email, dni:p.dni ,conocimientos: collect(conocimiento)} as obj", new { conocimiento });

                    var jobjResultados = new List<dynamic>();

                    foreach (var record in resultado)
                    {

                        var obj = record["obj"].As<IDictionary<string, object>>();
                        jobjResultados.Add(this.getJObjPersona(obj));

                    }

                    return jobjResultados;

                });

                return resultado;

            }

        }

        public List<dynamic> listAllPersonas()
        {

            using (var session = driver.Session())
            {

                var resultado = session.ReadTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(p:Persona)
                    OPTIONAL MATCH (p)-[:CONOCE]->(c:Conocimiento)
                    WITH p,{id: ID(c), uid:c.uid, nombre:c.nombre} as conocimiento
                    RETURN {id: ID(p), uid:p.uid, nombre:p.nombre, email:p.email, dni:p.dni, conocimientos:collect(conocimiento)} as obj");

                    var jobjResultados = new List<dynamic>();

                    foreach (var record in resultado)
                    {

                        var obj = record["obj"].As<IDictionary<string, object>>();
                        jobjResultados.Add(this.getJObjPersona(obj));

                    }

                    return jobjResultados;

                });

                return resultado;

            }

        }

        // DELETE
        public dynamic deletePersonaByUid(string uid)
        {

            using (var session = driver.Session())
            {

                var resultado = session.WriteTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(p:Persona)
                    WITH p, properties(p) AS r
                    WHERE p.uid=$uid
                    DETACH DELETE p
                    RETURN r", new { uid });

                    return resultado;

                });


                var record = resultado.SingleOrDefault();

                if (record == null) {

                    throw new PersonaNotFoundException();

                } else {

                    var obj = record["obj"].As<IDictionary<string, object>>();
                    return this.getJObjPersona(obj);

                }

            }

        }
        
        // UPDATE


        // Private
        private dynamic getJObjPersona(IDictionary <string,object> dict)
        {

            var conoList = dict["conocimientos"].As<IList<object>>();

            dynamic jObjPer = new JObject();

            jObjPer.id = dict["id"];
            jObjPer.uid = dict["uid"];
            jObjPer.nombre = dict["nombre"];
            jObjPer.dni = dict["dni"];
            jObjPer.email = dict["email"];

            var jobjConocimientos = new List<JObject>();
            foreach (var cono in conoList)
            {

                dynamic jObjCono = new JObject();

                var conoDict = cono.As<IDictionary<string, object>>();

                jObjCono.id = conoDict["id"];
                jObjCono.uid = conoDict["uid"];
                jObjCono.nombre = conoDict["nombre"];
                jobjConocimientos.Add(jObjCono);

            }

            jObjPer.conocimientos = JArray.FromObject(jobjConocimientos);

            return jObjPer;

        }
    
    }

}

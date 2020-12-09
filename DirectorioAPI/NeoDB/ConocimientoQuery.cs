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
        public int insertConocimiento(string uid, string nombre)
        {

            using (var session = driver.Session())
            {

                var resultado = session.WriteTransaction(tx =>
                {

                    var resultado = tx.Run(@"CREATE (c:Conocimiento {uid:$uid, nombre:$nombre})
                    RETURN ID(c)", new { uid, nombre });

                    var record = resultado.Single();
                    var id = record["ID(c)"].As<int>();

                    return id;

                });

                return resultado;

            }

        }

        // READ
        public dynamic findConocimientoByUid(string uid)
        {

            using (var session = driver.Session())
            {

                var resultado = session.ReadTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(c:Conocimiento)
                    OPTIONAL MATCH (c)-[:CONTIENE]->(c2:Conocimiento)
                    WITH c,{id: ID(c2), uid:c2.uid, nombre:c2.nombre} as conocimiento
                    WHERE c.uid=$uid
                    RETURN {id: ID(c), uid:c.uid, nombre:c.nombre, subconocimientos:collect(conocimiento)} as obj", new { uid });


                    var record = resultado.SingleOrDefault();

                    if (record == null)
                    {
                        throw new ConocimientoNotFoundException();
                    }
                    else
                    {

                        var obj = record["obj"].As<IDictionary<string, object>>();
                        return this.getJObjConocimiento(obj);

                    }

                });

                return resultado;

            }

        }

        public List<dynamic> listAllConocimientos()
        {

            using (var session = driver.Session())
            {

                var resultado = session.ReadTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(c:Conocimiento)
                    OPTIONAL MATCH(c)-[:CONTIENE]->(c2:Conocimiento)
                    WITH c, {id: ID(c2), uid:c2.uid, nombre:c2.nombre} as subconocimiento
                    RETURN {id: ID(c), uid:c.uid, nombre:c.nombre, subconocimientos:collect(subconocimiento)} as obj");


                    var jobjResultados = new List<dynamic>();

                    foreach (var record in resultado)
                    {

                        var obj = record["obj"].As<IDictionary<string, object>>();
                        jobjResultados.Add(this.getJObjConocimiento(obj));

                    }

                    return jobjResultados;

                });

                return resultado;

            }

        }

        // DELETE
        public dynamic deleteConocimientoByUid(string uid)
        {

            using (var session = driver.Session())
            {

                var resultado = session.WriteTransaction(tx =>
                {

                    var resultado = tx.Run(@"MATCH(c:Conocimiento)
                    WITH c, properties(c) AS r
                    WHERE c.uid=$uid
                    DETACH DELETE p
                    RETURN r", new { uid });

                    return resultado;

                });

                var record = resultado.SingleOrDefault();

                if (record == null)
                {

                    throw new ConocimientoNotFoundException();

                }
                else
                {

                    var obj = record["obj"].As<IDictionary<string, object>>();
                    return this.getJObjConocimiento(obj);

                }

            }

        }

        // UPDATE

        // Private
        private dynamic getJObjConocimiento(IDictionary<string, object> dict)
        {

            var subconocimientosList = dict["subconocimientos"].As<IList<object>>();

            dynamic jobj = new JObject();

            jobj.id = dict["id"];
            jobj.uid = dict["uid"];
            jobj.nombre = dict["nombre"];

            var jobjConocimientos = new List<JObject>();
            foreach (var subconocimiento in subconocimientosList)
            {

                dynamic jobj2 = new JObject();

                var subconDict = subconocimiento.As<IDictionary<string, object>>();

                if (subconDict["id"] != null)
                {
                    jobj2.id = subconDict["id"];
                    jobj2.uid = subconDict["uid"];
                    jobj2.nombre = subconDict["nombre"];
                    jobjConocimientos.Add(jobj2);
                }
                else
                {
                    break;
                }

            }

            jobj.subconocimientos = JArray.FromObject(jobjConocimientos);

            return jobj;

        }

    }
}

using Neo4j.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DirectorioAPI.NeoDB
{
    public partial class NeoDBConn
    {

        private static NeoDBConn instance;
        private IDriver driver;

        private NeoDBConn()
        {

            var driver = GraphDatabase.Driver("bolt://localhost:7687/", AuthTokens.Basic("neo4j", "asd"));
            this.driver = driver;
            NeoDBConn.instance = this;

        }

        public static NeoDBConn getConn()
        {

            if (NeoDBConn.instance == null)
            {
                NeoDBConn.instance = new NeoDBConn();
            }

            return NeoDBConn.instance;
        }

    }
}

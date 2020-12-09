using DirectorioAPI.DAOs;
using DirectorioAPI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Type = DirectorioAPI.Models.Type;

namespace UnitTest
{
    class Relations_Resolution_Test
    {

        [SetUp]
        public void Setup()
        {

            // Characteristics
            Characteristic char1 = new Characteristic("Java", Type.Tipo1);
            Characteristic char2 = new Characteristic("Spring", Type.Tipo2);
            Characteristic char3 = new Characteristic("Swing", Type.Tipo2);
            Characteristic char4 = new Characteristic("JWT", Type.Tipo3);

            char2.addSubCharac(char4); // JWT -> Spring
            
            char1.addSubCharac(char2); // Spring -> Java
            char1.addSubCharac(char3); // Swing -> Java

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            characDAO.add(char1);
            characDAO.add(char2);
            characDAO.add(char3);
            characDAO.add(char4);

            // Persons
            Persona person1 = new Persona("Juan", "00000001", "juan@hotmail.com");
            Persona person2 = new Persona("Alberto", "00000002", "alberto@hotmail.com");

            person1.addCharac(char1);
            person2.addCharac(char4);

            PersonasDAO perDAO = PersonasDAO.getInstance();
            perDAO.clear();

            perDAO.add(person1);
            perDAO.add(person2);

        }

        [Test]
        public void FindPersonByCharName_ExistName1_GetPerson()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            var charac = characDAO.findByName("Java");
            var listPersons = perDAO.findByCharac(charac);

            Assert.AreEqual(2, listPersons.Count);
            //Assert.That(listPersons.TrueForAll(x => x.containCharac(charac)));

        }

        [Test]
        public void FindPersonByCharName_ExistName2_GetPerson()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            var charac = characDAO.findByName("JWT");
            var listPersons = perDAO.findByCharac(charac);

            Assert.AreEqual(1, listPersons.Count);
            //Assert.That(listPersons.TrueForAll(x => x.containCharac(charac)));

        }

        [Test]
        public void FindPersonByCharName_ExistName3_GetPerson()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            var charac = characDAO.findByName("Spring");
            var listPersons = perDAO.findByCharac(charac);

            Assert.AreEqual(1, listPersons.Count);
            //Assert.That(listPersons.TrueForAll(x => x.containCharac(charac)));

        }

    }
}

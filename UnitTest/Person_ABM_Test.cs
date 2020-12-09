using DirectorioAPI.DAOs;
using DirectorioAPI.Models;

using NUnit.Framework;
using System.Numerics;

namespace UnitTest
{

    public class Person_ABM_Test
    {

        [SetUp]
        public void Setup()
        {

            Persona person1 = new Persona("Juan", "00000001", "juan@hotmail.com");
            Persona person2 = new Persona("Alberto", "00000002", "alberto@hotmail.com");
            Persona person3 = new Persona("Hernesto", "0000003", "hernesto@hotmail.com");

            PersonasDAO perDAO = PersonasDAO.getInstance();
            perDAO.clear();

            perDAO.add(person1);
            perDAO.add(person2);
            perDAO.add(person3);

        }

        [Test]
        public void New_Person_AddToCollection()
        {
            PersonasDAO perDAO = PersonasDAO.getInstance();
            Assert.AreEqual(3, perDAO.size());
        }

        [Test]
        public void FindByEmail_ExistEmail_GetPersonFromCollection()
        {
            PersonasDAO perDAO = PersonasDAO.getInstance();
            var person = perDAO.findByEmail("juan@hotmail.com");
            Assert.IsNotNull(person);
        }

        [Test]
        public void FindByEmail_InexistEmail_GetNullFromCollection()
        { 
            PersonasDAO perDAO = PersonasDAO.getInstance();
            var person = perDAO.findByEmail("asd@hotmail.com");
            Assert.IsNull(person);
        }

        [Test]
        public void FindByDni_ExistDni_GetPersonFromCollection()
        {
            PersonasDAO perDAO = PersonasDAO.getInstance();
            var person = perDAO.findByDni("00000001");
            Assert.IsNotNull(person);
        }

        [Test]
        public void FindByDni_InexistDni_GetNullFromCollection()
        {
            PersonasDAO perDAO = PersonasDAO.getInstance();
            var person = perDAO.findByDni("asd");
            Assert.IsNull(person);
        }

        [Test]
        public void RemoveByDni_ExistDni_DeleteFromCollection()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            var quantBef = perDAO.size();
            perDAO.findRemoveByDni("00000001");
            var quantAft = perDAO.size();
            Assert.AreEqual(quantBef, quantAft+1);

        }

        [Test]
        public void RemoveByDni_InexistDni_NotDeleteFromCollection()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            var quantBef = perDAO.size();
            perDAO.findRemoveByDni("asd");
            var quantAft = perDAO.size();
            Assert.AreEqual(quantBef, quantAft);

        }

        [Test]
        public void RemoveByEmail_ExistEmail_DeleteFromCollection()
        {

            PersonasDAO perDAO = PersonasDAO.getInstance();
            var quantBef = perDAO.size();
            perDAO.findRemoveByEmail("juan@hotmail.com");
            var quantAft = perDAO.size();
            Assert.AreEqual(quantBef, quantAft + 1);

        }

        [Test]
        public void RemoveByEmail_InexistEmail_NotDeleteFromCollection()
        {
            PersonasDAO perDAO = PersonasDAO.getInstance();
            var quantBef = perDAO.size();
            perDAO.findRemoveByEmail("asd@hotmail.com");
            var quantAft = perDAO.size();
            Assert.AreEqual(quantBef, quantAft);
        }

    }

}
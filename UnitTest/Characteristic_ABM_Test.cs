using DirectorioAPI.DAOs;
using DirectorioAPI.DAOs.Exceptions;
using DirectorioAPI.Models;

using NUnit.Framework;
using System;
using Type = DirectorioAPI.Models.Type;

namespace UnitTest
{
    public class Characteristic_ABM_Test
    {

        [SetUp]
        public void Setup()
        {

            Characteristic char1 = new Characteristic("Java", Type.Tipo1);
            Characteristic char2 = new Characteristic("Spring", Type.Tipo2);

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            characDAO.clear();

            characDAO.add(char1);
            characDAO.add(char2);

        }

        [Test]
        public void New_Characteristic_AddToCollection()
        {
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            Assert.AreEqual(2, characDAO.size());
        }

        [Test]
        public void FindByName_ExistName_GetCharacFromCollection()
        {
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            var charac = characDAO.findByName("Java");
            Assert.IsNotNull(charac);
        }

        [Test]
        public void FindByName_InexistName_GetNullFromCollection()
        {
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            var charac = characDAO.findByName("Asd");
            Assert.IsNull(charac);
        }

        [Test]
        public void FilterByType_ExistType_GetCharacListFromCollection()
        {
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            var listChar = characDAO.filterByType(Type.Tipo1);
            Assert.That(listChar.TrueForAll(x => x.type == Type.Tipo1));
        }

        [Test]
        public void RemoveByName_ExistName_DeleteFromCollection()
        {
            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            var quantBef = characDAO.size();
            characDAO.findRemoveByName("Java");
            var quantAft = characDAO.size();
            Assert.AreEqual(quantBef, quantAft+1);

        }

        [Test]
        public void RemoveByName_InexistName_NotDeleteFromCollection()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();
            var quantBef = characDAO.size();
            characDAO.findRemoveByName("asd");
            var quantAft = characDAO.size();
            Assert.AreEqual(quantBef, quantAft);

        }

        [Test]
        public void AddCharacRelationByName_ExistNames_AddRelation()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.addCharacRelationByName("Spring", "Java");

                var subCharac = characDAO.findByName("Spring");
                var charac = characDAO.findByName("Java");

                if (charac.containsSubCharac(subCharac))
                {
                    Assert.Pass();
                } else
                {
                    Assert.Fail();

                }

            } catch (CharacteristicNotFoundException)
            {
                Assert.Fail();
            }    

        }

        [Test]
        public void AddCharacRelationByName_InexistFstName_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.addCharacRelationByName("asd", "Java");

                var subCharac = characDAO.findByName("asd");
                var charac = characDAO.findByName("Java");

                if (charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }


        }

        [Test]
        public void AddCharacRelationByName_InexistSndName_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.addCharacRelationByName("Spring", "asd");

                var subCharac = characDAO.findByName("Spring");
                var charac = characDAO.findByName("asd");

                if (charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void AddCharacRelationByName_InexistNames_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.addCharacRelationByName("asd", "asd");

                var subCharac = characDAO.findByName("asd");
                var charac = characDAO.findByName("asd");

                if (charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void RemoveCharacRelationByName_ExistNames_DeleteRelation()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {

                characDAO.removeCharacRelationByName("Spring", "Java");

                var subCharac = characDAO.findByName("Spring");
                var charac = characDAO.findByName("Java");

                if (!charac.containsSubCharac(subCharac))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Fail();
            }

        }

        [Test]
        public void RemoveCharacRelationByName_InexistFstName_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.removeCharacRelationByName("asd", "Java");

                var subCharac = characDAO.findByName("Spring");
                var charac = characDAO.findByName("Java");

                if (!charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void RemoveCharacRelationByName_InexistSndName_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.removeCharacRelationByName("Spring", "asd");

                var subCharac = characDAO.findByName("Spring");
                var charac = characDAO.findByName("asd");

                if (!charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void RemoveCharacRelationByName_InexistNames_ThrowsException()
        {

            CharacteristicDAO characDAO = CharacteristicDAO.getInstance();

            try
            {
                characDAO.removeCharacRelationByName("asd", "asd");

                var subCharac = characDAO.findByName("asd");
                var charac = characDAO.findByName("asd");

                if (!charac.containsSubCharac(subCharac))
                {
                    Assert.Fail();
                }
                else
                {
                    Assert.Fail();

                }

            }
            catch (CharacteristicNotFoundException)
            {
                Assert.Pass();
            }

        }

    }

}
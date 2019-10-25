using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{

    public class TestSuite
    {
        //basic test for example of testing functionality
        [UnityTest]
        public IEnumerator TestCardUserSetter()
        {
            BaseCard card = new BaseCard();
            card.setUserName("TestName");
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
            Assert.True(card.User == "TestName");
        }

        //instantiate GameObjects from Prefab
        [UnityTest]
        public IEnumerator InstantiateFromPrefab()
        {
            var PlayerCharacter = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Tests/TestPrefabs/Player1.prefab", typeof(GameObject));
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
            Assert.IsNotNull(PlayerCharacter);
        }

        //tests ability to populate a card from HandleTurn information
        [UnityTest]
        public IEnumerator HandleTurnPopulateCard()
        {
            HandleTurn turn = new HandleTurn();
            turn.Attacker = "Player1";
            turn.Type = "Attack";
            turn.cardToUse = new CardAttack1();
            turn.populateCard();
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
            Assert.True(turn.cardToUse.Type == "Attack");
            Assert.True(turn.cardToUse.User == "Player1");
        }





        /*
        // A Test behaves as an ordinary method
        [Test]
        public void TestSuiteSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        */
    }
}

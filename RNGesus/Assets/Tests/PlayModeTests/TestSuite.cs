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

        //instantiate GameObjects from Prefab
        [UnityTest]
        public IEnumerator LoadBattleScene()
        {
            string newGameScene = "BattleSceneTemplate";
            List<string> playerStrings = new List<string>();
            List<string> enemyStrings = new List<string>();
            playerStrings.Add("Assets/Prefabs/Characters/Jesus.prefab");
            playerStrings.Add("Assets/Prefabs/Characters/Mary.prefab");
            enemyStrings.Add("Assets/Prefabs/Enemies/Pontius Pilates.prefab");
            enemyStrings.Add("Assets/Prefabs/Enemies/PontiusGoon.prefab");

            CharacterManager.Load(newGameScene, playerStrings, enemyStrings);
            yield return new WaitForSeconds(1.0f);
        }

        /*
        //test attacking
        [UnityTest]
        public IEnumerator TestAttacking()
        {
            string newGameScene = "BattleSceneTemplate";
            List<string> playerStrings = new List<string>();
            List<string> enemyStrings = new List<string>();
            playerStrings.Add("Assets/Prefabs/Characters/Jesus.prefab");
            playerStrings.Add("Assets/Prefabs/Characters/Mary.prefab");
            enemyStrings.Add("Assets/Prefabs/Enemies/Pontius Pilates.prefab");
            enemyStrings.Add("Assets/Prefabs/Enemies/PontiusGoon.prefab");

            CharacterManager.Load(newGameScene, playerStrings, enemyStrings);

            BattleStateMachine BSM = GameObject.FindObjectOfType<BattleStateMachine>();
            
            yield return new WaitForSeconds(1.0f);

            //select an attacking type card
            for (int i = 0; i < BSM.selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards.Count; i++){
                if (BSM.selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards[i].Type == "Attack"){
                    BSM.selectCard(i);
                }
                break;
            }

            //select the enemy
            BSM.selectEnemy(0);
            yield return new WaitForSeconds(7.0f);

            //assert that the enemy got damaged
            Assert.True(BSM.EnemyCharacters[0].GetComponent<EnemyStateMachine>().enemy.currentHP != 100);

        }
        */

        //tests ability to populate a card from HandleTurn information
        [UnityTest]
        public IEnumerator HandleTurnPopulateCard()
        {
            HandleTurn turn = new HandleTurn();
            turn.Attacker = "Player1";
            turn.Type = "Attack";
            turn.cardToUse = new CardAttack1();
            turn.populateCard();
            yield return null;
            Assert.True(turn.cardToUse.Type == "Attack");
            Assert.True(turn.cardToUse.User == "Player1");
        }
    }
}

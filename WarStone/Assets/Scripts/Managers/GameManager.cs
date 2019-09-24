﻿#pragma warning disable 0618

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SA.GameStates;


namespace SA
{
    public class GameManager : NetworkBehaviour
    {
        public State currentState;
        public GameObject CardPrefab;
        public Player currentPlayer;


        private void Start()
        {
            Resources.Load<Player>(@"Data/Variables/Player1").StartingCardID = 0;
            Resources.Load<Player>(@"Data/Variables/Player2").StartingCardID = 100;
            Settings.gameManager = this;


            if (!isServer) {
                Settings.ChangeStateToOpponentControlState();
                currentPlayer = Resources.Load<Player>(@"Data/Variables/Player2");
                


            } else {
                Settings.ChangeStateToPlayerControlState();
                currentPlayer = Resources.Load<Player>(@"Data/Variables/Player1");

            }
        }

        private void Update()
        {
            currentState.Tick(Time.deltaTime);
        }

        public void SetState(State state)
        {
            currentState = state;
        }

        public void CreateHandCard()
        {            
            GameObject obj = Instantiate(CardPrefab) as GameObject;
            currentPlayer.CreateHandCard(obj);
            //NetworkServer.Spawn(obj);
            var playerConObj = GameObject.Find("LocalPlayer").GetComponent<PlayerConnectionScript>();
            playerConObj.CmdSpawnCard(obj);
        }


    }
}

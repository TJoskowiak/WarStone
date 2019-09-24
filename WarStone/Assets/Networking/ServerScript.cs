﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerScript : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void RegisterMove(int FirstCard, int SecondCard) {
        if (!isServer) {
            return;
        }

        //server should calculate the damage and update syncvars here


        Debug.Log("Server: Registered the move " + FirstCard.ToString() + " " + SecondCard.ToString() + " , new turn");
        RpcSwitchRounds();
    }

    [ClientRpc]
    public void RpcSwitchRounds() {
        SA.Settings.SwapState();
    }

    [ClientRpc]
    public void RpcPlayer1CardDeployed(GameObject card)
    {
        if (card == null) return;

        //typ kartu
        //if(card.instance.cardViz.cardType == MinionCard)
        {
            var grid = GameObject.Find("Player1CardsDown");
            card.transform.SetParent(grid.transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;
            if (!isServer){
                SA.CardInstance cardInstance = card.AddComponent<SA.CardInstance>();
            }
            card.GetComponentInParent<SA.CardInstance>().currentLogic = Resources.Load<SA.GameElements.CardElementLogic>(@"Data/Game Elements/My Desk Card");
            if (card.GetComponentInParent<SA.CardInstance>() == null)
                Debug.Log("CardInstanceNull");
        }
    }

    [ClientRpc]
    public void RpcPlayer2CardDeployed(GameObject card) {
        if (card == null) return;

        {
            var grid = GameObject.Find("Player2CardsDown");
            card.transform.SetParent(grid.transform);
            card.transform.localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;
            if (!isServer){
                SA.CardInstance cardInstance = card.AddComponent<SA.CardInstance>();
            }
            card.GetComponentInParent<SA.CardInstance>().currentLogic = Resources.Load<SA.GameElements.CardElementLogic>(@"Data/Game Elements/Opponent Desk Card");
            if (card.GetComponentInParent<SA.CardInstance>() == null)
                Debug.Log("CardInstanceNull");
        }
    }

}

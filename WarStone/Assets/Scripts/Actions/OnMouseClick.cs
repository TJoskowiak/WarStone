﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SA.GameStates
{
    [CreateAssetMenu(menuName = "Action/OnMouseClick")]
    public class OnMouseClick : Action
    {
        public override void Execute(float d) {
            if (Input.GetMouseButtonDown(0)) {
                List<RaycastResult> results = Settings.GetObjectUnderMouse();

                foreach (RaycastResult res in results) {
                    IClicable obj = res.gameObject.GetComponentInParent<IClicable>();
                    if (obj != null) {
                        obj.onClick();
                        break;
                    }
                }
            } else if (Input.GetMouseButtonDown(1)) {
                var Player = GameObject.Find("LocalPlayer");
                var PlayerComp = Player.GetComponent<PlayerConnectionScript>();
                PlayerComp.CmdSendMovement(PlayerComp.firstCard, PlayerComp.secondCard);
                PlayerComp.firstCard = 0;
                PlayerComp.secondCard = 0;

                var firstViz = GameObject.Find(PlayerComp.firstCard.ToString()).GetComponent<SA.CardViz>();
                var secondViz = GameObject.Find(PlayerComp.secondCard.ToString()).GetComponent<SA.CardViz>();
                firstViz._highlight = false;
                secondViz._highlight = false;

            }
        }
    }
}

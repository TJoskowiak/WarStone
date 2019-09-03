﻿//Unity inludes
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//System includes
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

namespace SA {
    public class CardViz : NetworkBehaviour {
        //To be deleted. Or left. Not sure though what will call desrialize on card. Probably some battlefield controller that will also update the values on the cards.
        public int card_json_id = 1;
        public int card_object_id = 0;

        public TextMeshProUGUI cardName;
        public TextMeshProUGUI HP;
        public TextMeshProUGUI strength;
        public TextMeshProUGUI weaponSkill;

        public Image image;
        [SyncVar]
        private int _healthStat;
        [SyncVar]
        private int _specialStat;
        [SyncVar]
        private int _strengthStat;

        public int healthStat { get => _healthStat; set => _healthStat = value; }
        public int specialStat { get => _specialStat; set => _specialStat = value; }
        public int strengthStat { get => _strengthStat; set => _strengthStat = value; }

        private void Start() {
            if (!isServer) {
                transform.Rotate(new Vector3(0, 0, 180));
                if (card_object_id < 100) {
                    //ustaw authority dla mnie
                    CmdSetAuthority(this.GetComponent<NetworkIdentity>());
                }

            } else {
                if (card_object_id >= 100) {
                    // ustaw authority dla mnie
                    CmdSetAuthority(this.GetComponent<NetworkIdentity>());

                }

            }
            //To be deleted. The Deserialie will be called from the game controller during the bginning of the game. Can' pass any parameters through Start, Awake or Update
            DeserializeCard(card_json_id);
            this.name = card_object_id.ToString();
        }

        private void Update()
        {
            if (hasAuthority)
            {
                // Debug.Log(this.GetComponent<NetworkIdentity>().netId.ToString() + "I am yours");
            }
            this.HP.text = this.healthStat.ToString();
            this.strength.text = this.strengthStat.ToString();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                
                if (hit.collider != null || card_object_id == 4)
                {
                    //hit.transform.name == this.name  <= original condiiton
                    if ( card_object_id == 4) // <== debug condition
                    {
                        var Player = GameObject.Find("LocalPlayer");
                        var PlayerComp = Player.GetComponent<PlayerConnectionScript>();
                       
                        if (PlayerComp.firstCard == 0)
                        {
                            PlayerComp.firstCard = card_object_id;
                        }

                        else if(PlayerComp.firstCard == this.card_object_id)
                        {
                            PlayerComp.firstCard = 0;
                        }

                        else if(PlayerComp.secondCard == 0)
                        {
                            PlayerComp.secondCard = card_object_id;
                        }

                        else if(PlayerComp.secondCard == card_object_id)
                        {
                            PlayerComp.secondCard = 0;
                        }
                    }
                }
            }

        }
        //TODO: Implementation function that retrieves data from JSON file//
        //Add JSON files in folder \WarStone\Assets\Data//
        public void DeserializeCard(int CardID) {
            var CardData = new Card();

            var jsonString = File.ReadAllText(@"Assets/Cards.json");
            var allCards = JsonUtility.FromJson<CardContainer>(jsonString);

            foreach (Card c in allCards.cards) {
                if (c.id == CardID) {
                    CardData = c;
                }
            }
            //Initialize each field
            this.cardName.text = CardData.name;
            //Initialize ints for each of the card fields
            this.healthStat = CardData.health;
            this.specialStat = CardData.special;
            this.strengthStat = CardData.strength;

            //Initialize corresponding TextMeshes
            this.HP.text = this.healthStat.ToString();
            this.strength.text = this.strengthStat.ToString();
            //this.weaponSkill.text = this.WeaponSkillNum.ToString();

            //Initialize the sprite for the card
            this.image.sprite = Resources.Load<Sprite>(CardData.gfx_path);

        }

        [Command]
        void CmdSetAuthority(NetworkIdentity grabID) {
  

            grabID.AssignClientAuthority(connectionToClient);
        }

    }
}

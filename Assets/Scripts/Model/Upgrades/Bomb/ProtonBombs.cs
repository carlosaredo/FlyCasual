﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrade;
using Ship;
using Bombs;

namespace UpgradesList
{

    public class ProtonBombs : GenericTimedBomb
    {
        GenericShip _ship = null;

        public ProtonBombs() : base()
        {
            Type = UpgradeType.Bomb;
            Name = "Proton Bombs";
            Cost = 5;

            bombPrefabPath = "Prefabs/Bombs/ProtonBomb";

            IsDiscardedAfterDropped = true;
        }

        public override void ExplosionEffect(GenericShip ship, Action callBack)
        {
            _ship = ship;

            Triggers.RegisterTrigger(new Trigger()
            {
                Name = "Suffer damage from bomb",
                TriggerType = TriggerTypes.OnDamageIsDealt,
                TriggerOwner = ship.Owner.PlayerNo,
                EventHandler = SufferProtonBombDamage,
                EventArgs = new DamageSourceEventArgs()
                {
                    Source = this,
                    DamageType = DamageTypes.BombDetonation
                }
            });

            Triggers.ResolveTriggers(TriggerTypes.OnDamageIsDealt, callBack);
        }

        private void SufferProtonBombDamage(object sender, EventArgs e)
        {
            Messages.ShowInfoToHuman(string.Format("{0}: Dealt faceup card to {1}", Name, _ship.PilotName));
            _ship.SufferHullDamage(true, e);            
        }

        public override void PlayDetonationAnimSound(GameObject bombObject, Action callBack)
        {
            BombsManager.CurrentBomb = this;

            Sounds.PlayBombSound(bombObject, "Explosion-7");
            bombObject.transform.Find("Explosion/Explosion").GetComponent<ParticleSystem>().Play();
            bombObject.transform.Find("Explosion/Ring").GetComponent<ParticleSystem>().Play();

            GameManagerScript Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            Game.Wait(1.4f, delegate { callBack(); });
        }
    }

}
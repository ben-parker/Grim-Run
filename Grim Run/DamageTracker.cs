using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Grim_Run
{

    internal class DamageTracker
    {
        private static class EntityName
        {
            public const string Nemesis = "/nemesis/";
            public const string Hero = "/hero/";
            public const string Boss = "/boss&quest/";
            public const string Bounties = "/bounties/";
            public const string Player = "/pc/";
            public const string Pet0 = "/playerclass";
            public const string Pet1 = "/pets/";
        }

        private Dictionary<string, string> entities;
        private Dictionary<string, EntityDealtDamage> damageByEntity;
        private List<DamageDealt> damageToPlayer;
        
        private float _physical;
        private float _piercing;
        private float _fire;
        private float _cold;
        private float _lightning;
        private float _bleeding;
        private float _acid;
        private float _vitality;
        private float _aether;
        private float _chaos;
        private float _percentLife;
        private float _totalDamage;
        private float _unknown;
        
        private IProgress<(float, float, DamageType)> progress;
        private IProgress<DamageDealt> dmgToPlayerProgress;
        //private PlayerDamageTracker playerDamage;
        //private EnemyDamageTracker damageToPlayer;

        public DamageTracker(
            IProgress<(float total, float damage, DamageType type)> progress,
            IProgress<DamageDealt> dmgToPlayerProgress)
        {
            this.progress = progress;
            this.dmgToPlayerProgress = dmgToPlayerProgress;

            entities = new Dictionary<string, string>();
            damageToPlayer = new List<DamageDealt>();
        }

        public void Reset()
        {
            _physical = 0;
            _piercing = 0;
            _fire = 0;
            _cold = 0;
            _lightning = 0;
            _bleeding = 0;
            _acid = 0;
            _vitality = 0;
            _aether = 0;
            _chaos = 0;
            _percentLife = 0;
            _totalDamage = 0;
        }

        public void UpdateDamage(DamageDealt dmg)
        {
            string entry = String.Empty;
            string defenderName = String.Empty;

            if (dmg.AttackerId != null &&
                 dmg.AttackerName != null &&
                !entities.TryGetValue(dmg.AttackerId, out entry))
            {
                entities.Add(dmg.AttackerId, dmg.AttackerName);
            }

            // first check if we can get an existing defender name
            // or if we need to add a new one
            if (dmg.DefenderId != null &&
                 dmg.DefenderName != null &&
                !entities.TryGetValue(dmg.DefenderId, out defenderName))
            {
                entities.Add(dmg.DefenderId, dmg.DefenderName);
                defenderName = dmg.DefenderName;
            }
            // if we only got a defender ID (environmental, DOT)
            // see if it's an existing entity
            else if (dmg.DefenderId != null &&
                entities.TryGetValue(dmg.DefenderId, out defenderName))
            {
                
            }

            
            // need to do something here for situations where damage is done
            // to an entity that hasn't had a name sent yet, like dot damage without targeting yet
            // maybe a separate dictionary with a list of damage
            // in situations where there is a defender id with no name

            if (String.IsNullOrEmpty(defenderName))
            {
                return;
            }

            if (IsPlayerControlled(dmg.AttackerName) || !IsPlayerControlled(defenderName))
            {
                UpdatePlayerDamage(dmg);
            }
            else
            {
                UpdateDamageToPlayer(dmg, defenderName);
            }

        }

        private bool IsPlayerControlled(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }

            return (name.Contains(EntityName.Player) ||
                name.Contains(EntityName.Pet0) ||
                name.Contains(EntityName.Pet1));
        }

        private void UpdatePlayerDamage(DamageDealt dmg)
        {
            if (dmg.Type != DamageType.Unknown)
            {
                float newDmgTotal = AddToTotalDamage(dmg);
                _totalDamage += dmg.Damage;

                progress.Report((_totalDamage, newDmgTotal, dmg.Type));
            }
        }

        private void UpdateDamageToPlayer(DamageDealt dmg, string defenderName)
        {
            if (defenderName.Contains(EntityName.Player))
            {
                damageToPlayer.Add(dmg);
                dmgToPlayerProgress.Report(dmg);
            }
        }

        private bool AttackerIsBoss(string attackerName)
        {
            return (attackerName.Contains(EntityName.Boss) ||
                attackerName.Contains(EntityName.Bounties) ||
                attackerName.Contains(EntityName.Hero) ||
                attackerName.Contains(EntityName.Nemesis));
        }

        private float AddToTotalDamage(DamageDealt dmg)
        {
            float returnDmg = 0;

            if (dmg.Type == DamageType.Physical)
            {
                _physical += dmg.Damage;
                returnDmg = _physical;
            }
            else if (dmg.Type == DamageType.Piercing)
            {
                _piercing += dmg.Damage;
                returnDmg = _piercing;
            }
            else if (dmg.Type == DamageType.Fire)
            {
                _fire += dmg.Damage;
                returnDmg = _fire;
            }
            else if (dmg.Type == DamageType.Cold)
            {
                _cold += dmg.Damage;
                returnDmg = _cold;
            }
            else if (dmg.Type == DamageType.Lightning)
            {
                _lightning += dmg.Damage;
                returnDmg = _lightning;
            }
            else if (dmg.Type == DamageType.Bleeding)
            {
                _bleeding += dmg.Damage;
                returnDmg = _bleeding;
            }
            else if (dmg.Type == DamageType.Acid)
            {
                _acid += dmg.Damage;
                returnDmg = _acid;
            }
            else if (dmg.Type == DamageType.Vitality)
            {
                _vitality += dmg.Damage;
                returnDmg = _vitality;
            }
            else if (dmg.Type == DamageType.Aether)
            {
                _aether += dmg.Damage;
                returnDmg = _aether;
            }
            else if (dmg.Type == DamageType.Chaos)
            {
                _chaos += dmg.Damage;
                returnDmg = _chaos;
            }
            else if (dmg.Type == DamageType.PercentCurrentLife)
            {
                _percentLife += dmg.Damage;
                returnDmg = _percentLife;
            }
            else
            {
                _unknown += dmg.Damage;
            }

            return returnDmg;
        }
    }
}
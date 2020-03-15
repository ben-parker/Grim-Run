using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Grim_Run
{

    internal class DamageTracker
    {
        private static class Pattern
        {
            public const string Nemesis = "/nemesis/";
            public const string Hero = "/hero/";
            public const string Boss = "/boss&quest/";
            public const string Bounties = "/bounties/";
            public const string Player = "/pc/";
            public const string Pet0 = "/playerclass";
            public const string Pet1 = "/pets/";
        }

        private Dictionary<string, string> damage;
        
        private float _physical;
        private float _piercing;
        private float _fire;
        private float _cold;
        private float _lightning;
        private float _acid;
        private float _vitality;
        private float _aether;
        private float _chaos;
        private float _totalDamage;
        private float _unknown;
        
        private IProgress<(float, float, DamageType)> progress;

        public DamageTracker(IProgress<(float total, float damage, DamageType type)> progress)
        {
            this.progress = progress;
            damage = new Dictionary<string, string>();
        }

        public void Reset()
        {
            _physical = 0;
            _piercing = 0;
            _fire = 0;
            _cold = 0;
            _lightning = 0;
            _acid = 0;
            _vitality = 0;
            _aether = 0;
            _chaos = 0;
            _totalDamage = 0;
        }

        public void UpdateDamage(DamageDealt dmg)
        {
            string entry = String.Empty;

            if (dmg.AttackerId != null &&
                 dmg.AttackerName != null &&
                !damage.TryGetValue(dmg.AttackerId, out entry))
            {
                damage.Add(dmg.AttackerId, dmg.AttackerName);
                entry = dmg.AttackerName;
            }

            if (dmg.DefenderId != null &&
                 dmg.DefenderName != null &&
                !damage.TryGetValue(dmg.DefenderId, out entry))
            {
                damage.Add(dmg.DefenderId, dmg.DefenderName);
                entry = dmg.DefenderName;
            }

            // when dot or environmental, only get defender ID
            // if ID is in dictionary and is not player controlled, track damage

            // need to do something here for situations where damage is done
            // to an entity that hasn't had a name sent yet, like dot damage without targeting yet
            // maybe a separate dictionary with a list of damage
            // in situations where there is a defender id with no name

            // break out separate class for
            // player damage tracking
            // damage done by player-controlled entity
            // damage done to player by bosses

            if (String.IsNullOrEmpty(entry))
            {
                return;
            }

            bool trackDamage = false;
            if (IsPlayerControlled(dmg.AttackerName))
            {
                trackDamage = true;
            }
            else
            {
                // lookup defender ID in dictionary
                // if there, look at value (defender name) to see if it's player controlled
                trackDamage = !IsPlayerControlled(entry);
            }

            if (trackDamage && dmg.Type != DamageType.Unknown)
            {
                float newDmgTotal = AddToTotalDamage(dmg);
                _totalDamage += dmg.Damage;

                progress.Report((_totalDamage, newDmgTotal, dmg.Type));
            }
        }

        private bool IsPlayerControlled(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }

            return (name.Contains(Pattern.Player) ||
                name.Contains(Pattern.Pet0) ||
                name.Contains(Pattern.Pet1));
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
            else
            {
                _unknown += dmg.Damage;
            }

            return returnDmg;
        }
    }
}
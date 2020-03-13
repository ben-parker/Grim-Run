using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Grim_Run
{
    internal class DamageTracker
    {
        private Dictionary<string, DamageDealt> damage;
        
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
        
        private IProgress<(float, float, DamageType)> progress;

        public DamageTracker(IProgress<(float total, float damage, DamageType type)> progress)
        {
            this.progress = progress;
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
            //DamageDealt entry;

            //if (damage.TryGetValue(dmg.AttackerName, out entry))
            //{
            //    entry.Add((msg.Damage, msg.DamageType));
            //}
            //else
            //{
            //    entry = new List<(float damage, string type)>
            //    {
            //        (msg.Damage, msg.DamageType)
            //    };

            //    damageDone.Add(msg.AttackerName, entry);
            //}

            if (dmg.Type == DamageType.Physical)
                _physical += dmg.Damage;
            else if (dmg.Type == DamageType.Piercing)
                _piercing += dmg.Damage;
            else if (dmg.Type == DamageType.Fire)
                _fire += dmg.Damage;
            else if (dmg.Type == DamageType.Cold)
                _cold += dmg.Damage;
            else if (dmg.Type == DamageType.Lightning)
                _lightning += dmg.Damage;
            else if (dmg.Type == DamageType.Acid)
                _acid += dmg.Damage;
            else if (dmg.Type == DamageType.Vitality)
                _vitality += dmg.Damage;
            else if (dmg.Type == DamageType.Aether)
                _aether += dmg.Damage;
            else if (dmg.Type == DamageType.Chaos)
                _chaos += dmg.Damage;

            var returnDmg = dmg.Type switch
            {
                DamageType.Physical => _physical,
                DamageType.Piercing => _piercing,
                DamageType.Fire => _fire,
                DamageType.Cold => _cold,
                DamageType.Lightning => _lightning,
                DamageType.Acid => _acid,
                DamageType.Vitality => _vitality,
                DamageType.Aether => _aether,
                DamageType.Chaos => _chaos,
            };
            
            if (dmg.Type != DamageType.Unknown)
            {
                _totalDamage += dmg.Damage;
                progress.Report((_totalDamage, returnDmg, dmg.Type));
            }
        }
    }
}
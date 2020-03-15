using System;
using System.Collections.Generic;
using System.Linq;

namespace Grim_Run
{
    class GameEventParser
    {
        private DamageTracker tracker;

        private bool fullMessage = false;
        private float damage;
        private DamageType damageType;
        private string attackerName;
        private string attackerId;
        private string defenderId;
        private string defenderName;
        private CombatType combatType;
        private List<(float damage, DamageType damageType)> damageList;

        public GameEventParser(DamageTracker tracker)
        {
            this.tracker = tracker;
            damageList = new List<(float damage, DamageType damageType)>();
        }

        public void Parse(GrimRunMessage msg)
        {
            switch (msg.MessageType)
            {
                case MessageType.AttackerName:
                    fullMessage = true;
                    attackerName = msg.Data.Substring(0, msg.DataLen);
                    break;
                case MessageType.DefenderName:
                    defenderName = msg.Data;
                    break;
                case MessageType.AttackerId:
                    attackerId = BitConverter.ToString(msg.bytes);
                    break;
                case MessageType.ApplyDamage:
                    damage = msg.Damage;
                    break;
                case MessageType.DamageToDefender:
                    ParseDamageToDefender(msg);
                    break;
                //case MessageType.CombatType:
                //    combatType = CombatTypeFromString(msg.Data.Substring(0, msg.DataLen));
                //    break;
                case MessageType.EndCombat:
                    UpdateDamage();
                    fullMessage = false;
                    break;
                //default:
                //    throw new ArgumentException(
                //        message: "Unknown game event", paramName: nameof(msg.MessageType));
            }
        }

        private void ParseDamageToDefender(GrimRunMessage msg)
        {
            damageType = DamageTypeFromString(msg.Data2);
            defenderId = BitConverter.ToString(msg.bytes);

            if (damageType == DamageType.Unknown) return;

            if (fullMessage && damage > 0)
            {
                damageList.Add((damage, damageType));
                return;
            }

            if (!fullMessage && damage > 0)
            {
                UpdateDamage();
            }
        }

        private void UpdateDamage()
        {
            // full combat message may have multiple damage sources in it
            // has full info about attacker and defender
            if (fullMessage && damageList.Any())
            {
                foreach (var dmg in damageList)
                {
                    tracker.UpdateDamage(new DamageDealt
                    {
                        Damage = dmg.damage,
                        Type = dmg.damageType,
                        //CombatType = combatType,
                        AttackerId = attackerId,
                        AttackerName = attackerName,
                        DefenderId = defenderId,
                        DefenderName = defenderName
                    });
                }
                
                damageList.Clear();
            }
            // dot and environmental damage only has damage and defender
            else if (damage > 0)
            {
                tracker.UpdateDamage(new DamageDealt
                {
                    Damage = damage,
                    Type = damageType,
                    DefenderId = defenderId,
                    CombatType = CombatType.Dot
                });
            }

            
        }

        private DamageType DamageTypeFromString(string damageType)
        {
            var result = damageType switch
            {
                "Physical" => DamageType.Physical,
                "Pierce" => DamageType.Piercing,
                "Fire" => DamageType.Fire,
                "Cold" => DamageType.Cold,
                "Lightning" => DamageType.Lightning,
                "Acid" => DamageType.Acid,
                "Vitality" => DamageType.Vitality,
                "Aether" => DamageType.Aether,
                "Chaos" => DamageType.Chaos,
                _ => UnrecognizedDamageType(damageType)
            };

            return result;
        }

        private DamageType UnrecognizedDamageType(string damageType)
        {
            Console.Error.WriteLine($"Unrecognized Damage Type {damageType}");
            return DamageType.Unknown;
        }

        private CombatType CombatTypeFromString(string combatType)
        {
            var result = combatType switch
            {
                "Melee Attack" => CombatType.Melee,
                "Debuff Attack" => CombatType.Debuff,
                "Retaliation" => CombatType.Retaliation,
                _ => throw new ArgumentException(
                    message: "Did not recognize combat type", paramName: nameof(combatType))
            };

            return result;
        }
    }
}
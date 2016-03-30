﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Mario_s_Template
{
    public static class SpellsManager
    {
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Targeted R;

        public static void InitializeSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 350);
            W = new Spell.Active(SpellSlot.W, 200);
            E = new Spell.Active(SpellSlot.E, 300);
            R = new Spell.Targeted(SpellSlot.R, 400);
        }

        #region Damages
        public static float GetDamage(this Obj_AI_Base target, SpellSlot slot)
        {
            var damageType = DamageType.Magical;
            var AD = Player.Instance.FlatPhysicalDamageMod;
            var AP = Player.Instance.FlatMagicDamageMod;
            var sLevel = Player.GetSpell(slot).Level - 1;

            //You can get the damage information easily on wikia

            var dmg = 0f;

            switch (slot)
            {
                case SpellSlot.Q:
                    if (Q.IsReady())
                    {
                        dmg += new float[] { 20, 45, 70, 95, 120 }[sLevel] + 1f * AD;
                    }
                    break;
                case SpellSlot.W:
                    if (W.IsReady())
                    {
                        dmg += new float[] { 0, 0, 0, 0, 0 }[sLevel] + 1f * AD;
                    }
                    break;
                case SpellSlot.E:
                    if (E.IsReady())
                    {
                        dmg += new float[] { 80, 110, 140, 170, 200 }[sLevel];
                    }
                    break;
                case SpellSlot.R:
                    if (R.IsReady())
                    {
                        dmg += new float[] { 600, 840, 1080 }[sLevel] * 0.6f + 1.2f * AP;
                    }
                    break;
            }
            return Player.Instance.CalculateDamageOnUnit(target, damageType, dmg - 10);
        }

        public static float GetTotalDamage(this Obj_AI_Base target)
        {
            var dmg =
                Player.Spells.Where(
                    s => (s.Slot == SpellSlot.Q) || (s.Slot == SpellSlot.W) || (s.Slot == SpellSlot.E) || (s.Slot == SpellSlot.R) && s.IsReady)
                    .Sum(s => target.GetDamage(s.Slot));

            return dmg + Player.Instance.GetAutoAttackDamage(target);
        }

        #endregion Damages
    }
}
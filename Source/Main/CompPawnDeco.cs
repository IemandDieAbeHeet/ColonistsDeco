using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.AI;

namespace Main
{
    class CompPawnDeco : ThingComp
    {
        public int DecoCooldown {
            get { return decoCooldown; }
        }

        public bool CanDecorate {
            get {
                return decoCooldown == 0 ? true : false;
            }
        }

        private int decoCooldown = ColonistsDecoMain.Settings.defaultDecoCooldown;

        public void ResetDecoCooldown()
        {
            Random random = new Random();
            if (ColonistsDecoMain.Settings.defaultDecoCooldown != 0)
            {
                decoCooldown = ColonistsDecoMain.Settings.defaultDecoCooldown + random.Next(30000);
            } else
            {
                decoCooldown = 0;
            }
        }

        public void RemoveDecoCooldown()
        {
            decoCooldown = 0;
        }
        
        public override void CompTick()
        {
            base.CompTick();

            if (decoCooldown != 0)
            {
                decoCooldown--;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref decoCooldown, "decoCooldown", ColonistsDecoMain.Settings.defaultDecoCooldown);
        }
    }
}
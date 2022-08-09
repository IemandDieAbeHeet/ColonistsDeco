using System;
using Verse;

namespace ColonistsDeco
{
    class CompPawnDeco : ThingComp
    {
        public bool CanDecorate => _decoCooldown == 0;

        private int _decoCooldown = ColonistsDecoMain.settings.defaultDecoCooldown;

        public void ResetDecoCooldown()
        {
            Random random = new Random();
            if (ColonistsDecoMain.settings.defaultDecoCooldown != 0)
            {
                _decoCooldown = ColonistsDecoMain.settings.defaultDecoCooldown + random.Next(30000);
            } else
            {
                _decoCooldown = 0;
            }
        }

        public override void CompTick()
        {
            base.CompTick();

            if (_decoCooldown != 0)
            {
                _decoCooldown--;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Values.Look(ref _decoCooldown, "decoCooldown", ColonistsDecoMain.settings.defaultDecoCooldown);
        }
    }
}
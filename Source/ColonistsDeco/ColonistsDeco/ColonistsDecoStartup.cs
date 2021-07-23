using Verse;
using RimWorld;
using System;
using HarmonyLib;

namespace ColonistsDeco
{
    [StaticConstructorOnStartup]
    static class ColonistsDecoStartup
    {
        static ColonistsDecoStartup()
        {
            Utility.LoadDefs();
        }
    }
}
using Verse;
using RimWorld;
using System;
using HarmonyLib;

namespace Main
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
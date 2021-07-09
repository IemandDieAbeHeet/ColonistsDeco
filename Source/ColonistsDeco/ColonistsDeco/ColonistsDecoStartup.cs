using Verse;
using System.Reflection;

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
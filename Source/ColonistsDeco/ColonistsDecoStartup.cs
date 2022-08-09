using Verse;

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
using Verse;

namespace Main
{
    public class CompProperties_Decoration : CompProperties
    {
        public string decorationName;

        public CompProperties_Decoration()
        {
            compClass = typeof(CompDecoration);
        }
    }
}

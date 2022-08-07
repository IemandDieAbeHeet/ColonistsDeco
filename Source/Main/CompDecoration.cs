using Verse;

namespace Main
{
    public class CompDecoration : ThingComp
    {
        public CompProperties_Decoration Props => (CompProperties_Decoration)props;

        public string decorationName;

        public string decorationCreator;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);

            decorationName = Props.decorationName;
            decorationCreator = "Unknown";
        }
    }
}

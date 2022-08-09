using Verse;

namespace ColonistsDeco
{
    public class CompDecoration : ThingComp
    {
        public CompProperties_Decoration Props => (CompProperties_Decoration)props;

        public string decorationName;

        public string decorationCreator;

        public override void Initialize(CompProperties compProps)
        {
            base.Initialize(compProps);

            decorationName = Props.decorationName;
            decorationCreator = "Unknown";
        }
    }
}

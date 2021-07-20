using Verse;
using RimWorld;

namespace ColonistsDeco
{
    enum DecoLocationType
    {
        Wall,
        Ceiling,
        Bedside
    }

    class DecoModExtension : DefModExtension
    {
        public TechLevel decoTechLevel;
        public DecoLocationType decoLocationType;
    }
}

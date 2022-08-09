using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ColonistsDeco
{
    public enum DecoLocationType
    {
        Wall,
        Ceiling,
        Bedside
    }

    class DecoModExtension : DefModExtension
    {
        public readonly List<TechLevel> decoTechLevels = new List<TechLevel>();
        public DecoLocationType decoLocationType;
    }
}

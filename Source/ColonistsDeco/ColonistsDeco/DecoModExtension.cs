using Verse;
using RimWorld;
using System.Collections.Generic;

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
        public List<TechLevel> decoTechLevels = new List<TechLevel>();
        public DecoLocationType decoLocationType;
    }
}

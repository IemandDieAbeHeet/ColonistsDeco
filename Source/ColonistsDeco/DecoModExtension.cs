using System.Collections.Generic;
using RimWorld;
using Verse;

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

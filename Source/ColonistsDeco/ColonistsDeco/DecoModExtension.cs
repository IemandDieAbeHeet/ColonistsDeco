using Verse;

namespace ColonistsDeco
{
    enum DecoTechProgression
    {
        Neolithic,
        Medieval,
        Industrial,
        Spacer,
        Ultra
    }

    enum DecoLocationType
    {
        Wall,
        Ceiling,
        Bedside
    }

    class DecoModExtension : DefModExtension
    {
        public DecoTechProgression decoTechProgression;
        public DecoLocationType decoLocationType;
    }
}

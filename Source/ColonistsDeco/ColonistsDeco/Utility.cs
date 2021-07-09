using System.Collections.Generic;
using Verse;

namespace ColonistsDeco
{
	class Utility
	{
		public static ThingDef wallDef;

		public static ThingDef tornDef;

		public static List<ThingDef> posterDefs = new List<ThingDef>();

		public static List<ThingDef> bedsideDefs = new List<ThingDef>();

		public static int wallHash;

		public static List<int> posterHashes = new List<int>();

		public static List<int> bedsideHashes = new List<int>();

		public static void LoadDefs()
		{
			CompProperties_AttachableThing attachableThingComp = new CompProperties_AttachableThing();
			CompProperties_PawnDeco pawnDecoThingComp = new CompProperties_PawnDeco();

			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.defName.Contains("DECOPoster"))
				{
					if(allDef.defName == "DECOPosterTorn")
                    {
						tornDef = allDef;
                    } else
                    {
						posterDefs.Add(allDef);
						posterHashes.Add(allDef.GetHashCode());
					}
				}
				else if (allDef.defName.Contains("DECOBedside"))
                {
					bedsideDefs.Add(allDef);
					bedsideHashes.Add(allDef.GetHashCode());
                }

				switch(allDef.defName)
                {
					case "Wall":
						wallDef = allDef;
						allDef.comps.Add(attachableThingComp);
						wallHash = allDef.GetHashCode();
						break;
					case "EndTable":
						allDef.comps.Add(attachableThingComp);
						break;
					case "Human":
						allDef.comps.Add(pawnDecoThingComp);
						break;
					default:
						break;
				}
			}
		}

		public static bool IsPoster(Thing thing)
		{
			if (posterHashes.Contains(thing.def.GetHashCode()))
			{
				return true;
			}
			return false;
		}

		public static bool IsBedside(Thing thing)
		{
			if (bedsideHashes.Contains(thing.def.GetHashCode()))
			{
				return true;
			}
			return false;
		}

		public static bool IsWall(Thing thing)
		{
			if (wallHash == thing.def.GetHashCode())
			{
				return true;
			}
			return false;
		}
	}
}
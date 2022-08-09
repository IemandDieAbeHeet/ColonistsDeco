using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld;
using Verse;

namespace ColonistsDeco
{
	public static class Utility
	{
		public static Dictionary<ThingDef, List<TechLevel>> thingTechProgression =
			new Dictionary<ThingDef, List<TechLevel>>();

		public static Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)> decoDictionary =
			new Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)>();

		public static List<ThingDef> ceilingDecoDefs = new List<ThingDef>();

		public static List<ThingDef> wallDecoDefs = new List<ThingDef>();

		public static List<ThingDef> bedsideDecoDefs = new List<ThingDef>();

		public static List<int> wallHashes = new List<int>();

		public static List<int> ceilingDecoHashes = new List<int>();

		public static List<int> wallDecoHashes = new List<int>();

		public static List<int> bedsideDecoHashes = new List<int>();

		public static List<ResearchProjectDef> researchProjectDefs = new List<ResearchProjectDef>();

		public static void LoadDefs()
		{
			CompProperties_AttachableThing attachableThingComp = new CompProperties_AttachableThing();
			CompProperties_PawnDeco pawnDecoThingComp = new CompProperties_PawnDeco();

			foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (allDef.HasModExtension<DecoModExtension>())
				{
					thingTechProgression.Add(allDef, allDef.GetModExtension<DecoModExtension>().decoTechLevels);
					decoDictionary.Add(allDef,
						(allDef.GetModExtension<DecoModExtension>().decoTechLevels,
							allDef.GetModExtension<DecoModExtension>().decoLocationType));

					switch (allDef.GetModExtension<DecoModExtension>().decoLocationType)
                    {
						case DecoLocationType.Wall:
							wallDecoDefs.Add(allDef);
							wallDecoHashes.Add(allDef.GetHashCode());
							break;
						case DecoLocationType.Bedside:
							bedsideDecoDefs.Add(allDef);
							bedsideDecoHashes.Add(allDef.GetHashCode());
							break;
						case DecoLocationType.Ceiling:
							ceilingDecoDefs.Add(allDef);
							ceilingDecoHashes.Add(allDef.GetHashCode());
							break;
						default:
							throw new ArgumentOutOfRangeException();
                    }
				}

				switch(allDef.defName)
                {
					case "Wall":
						allDef.comps.Add(attachableThingComp);
						wallHashes.Add(allDef.GetHashCode());
						break;
					case var val when new Regex("(Smoothed)+").IsMatch(val):
						allDef.comps.Add(attachableThingComp);
						wallHashes.Add(allDef.GetHashCode());
						break;
					case "EndTable":
						allDef.comps.Add(attachableThingComp);
						break;
					case "Human":
						allDef.comps.Add(pawnDecoThingComp);
						break;
                }
			}

			foreach(var researchDef in DefDatabase<ResearchProjectDef>.AllDefs)
            {
				if(researchDef.tab == DefDatabase<ResearchTabDef>.GetNamed("Decorations"))
                {
					researchProjectDefs.Add(researchDef);
                }
            }
		}

		public static bool IsCeilingDeco(Thing thing)
		{
			return ceilingDecoHashes.Contains(thing.def.GetHashCode());
		}

		public static bool IsWallDeco(Thing thing)
		{
			return wallDecoHashes.Contains(thing.def.GetHashCode());
		}

		public static bool IsBedsideDeco(Thing thing)
		{
			return bedsideDecoHashes.Contains(thing.def.GetHashCode());
		}

		public static bool IsWall(Thing thing)
		{
			return wallHashes.Any(h => h == thing.def.GetHashCode());
		}

		public static List<ThingDef> GetDecoList(DecoLocationType decoLocationType)
		{
			var decoList = new List<ThingDef>();
			var locationDecoList = new List<ThingDef>();
			var maxTechLevel = TechLevel.Neolithic;

			foreach (var researchProjectDef in researchProjectDefs.Where(researchProjectDef => researchProjectDef.IsFinished))
			{
				maxTechLevel = researchProjectDef.techLevel;
			}

			foreach (var deco in decoDictionary.Keys)
			{
				if (!decoDictionary.TryGetValue(deco, out var decoTuple)) continue;
				if (decoTuple.Item1.Any(t => t == maxTechLevel) && decoTuple.Item2 == decoLocationType)
				{
					decoList.Add(deco);
				}
				else if (decoTuple.Item2 == decoLocationType)
				{
					locationDecoList.Add(deco);
				}
			}

			return decoList.Count > 0 ? decoList : locationDecoList;
		}
		
		public static bool ResearchLevelHasDecos(ResearchProjectDef researchLevel, DecoLocationType decoLocationType)
        {
			var count = 0;

			foreach(var deco in decoDictionary.Keys)
			{
				if (!decoDictionary.TryGetValue(deco, out var decoTuple)) continue;
				if (decoTuple.Item1.Any(t => t == researchLevel.techLevel) && decoTuple.Item2 == decoLocationType)
				{
					count++;
				}
			}

			return count > 0;
        }

		public static ResearchProjectDef GetHighestResearchedLevel()
        {
	        var rd = new ResearchProjectDef();

			foreach (var researchProjectDef in researchProjectDefs.Where(researchProjectDef => researchProjectDef.IsFinished))
			{
				rd = researchProjectDef;
			}

			return rd;
		}
	}
}
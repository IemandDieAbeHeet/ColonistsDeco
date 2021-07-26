using System.Collections.Generic;
using Verse;
using RimWorld;

namespace ColonistsDeco
{
	class Utility
	{
		public static ThingDef wallDef;

		public static ThingDef tornDef;

		public static Dictionary<ThingDef, List<TechLevel>> thingTechProgression = new Dictionary<ThingDef, List<TechLevel>>();

		public static Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)> decoDictionary = new Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)>();

		public static List<ThingDef> ceilingDefs = new List<ThingDef>();

		public static List<ThingDef> wallDefs = new List<ThingDef>();

		public static List<ThingDef> bedsideDefs = new List<ThingDef>();

		public static int wallHash;

		public static List<int> ceilingHashes = new List<int>();

		public static List<int> wallHashes = new List<int>();

		public static List<int> bedsideHashes = new List<int>();

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
					decoDictionary.Add(allDef, (allDef.GetModExtension<DecoModExtension>().decoTechLevels, allDef.GetModExtension<DecoModExtension>().decoLocationType));

					switch (allDef.GetModExtension<DecoModExtension>().decoLocationType)
                    {
						case DecoLocationType.Wall:
							wallDefs.Add(allDef);
							wallHashes.Add(allDef.GetHashCode());

							if(allDef.defName == "DECOPosterTorn")
                            {
								tornDef = allDef;
                            }
							break;
						case DecoLocationType.Bedside:
							bedsideDefs.Add(allDef);
							bedsideHashes.Add(allDef.GetHashCode());
							break;
						case DecoLocationType.Ceiling:
							ceilingDefs.Add(allDef);
							ceilingHashes.Add(allDef.GetHashCode());
							break;
                    }
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

			foreach(ResearchProjectDef researchDef in DefDatabase<ResearchProjectDef>.AllDefs)
            {
				if(researchDef.tab == DefDatabase<ResearchTabDef>.GetNamed("Decorations"))
                {
					researchProjectDefs.Add(researchDef);
                }
            }
		}

		public static bool IsCeilingDeco(Thing thing)
        {
			if (ceilingHashes.Contains(thing.def.GetHashCode()))
            {
				return true;
            }
			return false;
		}

		public static bool IsWallDeco(Thing thing)
		{
			if (wallHashes.Contains(thing.def.GetHashCode()))
			{
				return true;
			}
			return false;
		}

		public static bool IsBedsideDeco(Thing thing)
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

		public static List<ThingDef> GetDecoList(DecoLocationType decoLocationType)
		{
			List<ThingDef> decoList = new List<ThingDef>();
			List<ThingDef> locationDecoList = new List<ThingDef>();
			TechLevel maxTechLevel = TechLevel.Neolithic;

			foreach (ResearchProjectDef researchProjectDef in researchProjectDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					maxTechLevel = researchProjectDef.techLevel;
				}
			}

			foreach (ThingDef deco in decoDictionary.Keys)
			{
				(List<TechLevel>, DecoLocationType) decoTuple;
				if (decoDictionary.TryGetValue(deco, out decoTuple))
				{
					if (decoTuple.Item1.Any(t => t == maxTechLevel) && decoTuple.Item2 == decoLocationType)
					{
						decoList.Add(deco);
					}
					else if (decoTuple.Item2 == decoLocationType)
					{
						locationDecoList.Add(deco);
					}
				}
			}

			if (decoList.Count > 0)
			{
				return decoList;
			}
			else
			{
				return locationDecoList;
			}
		}
		
		public static bool ResearchLevelHasDecos(ResearchProjectDef researchLevel, DecoLocationType decoLocationType)
        {
			int count = 0;

			foreach(ThingDef deco in decoDictionary.Keys)
            {
				(List<TechLevel>, DecoLocationType) decoTuple;
				if (decoDictionary.TryGetValue(deco, out decoTuple))
				{
					if (decoTuple.Item1.Any(t => t == researchLevel.techLevel) && decoTuple.Item2 == decoLocationType)
					{
						count++;
					}
				}
			}

			if(count > 0)
            {
				return true;
            } else
            {
				return false;
            }
        }

		public static ResearchProjectDef GetHighestResearchedLevel()
        {
			List<ResearchProjectDef> rds = new List<ResearchProjectDef>();
			ResearchProjectDef rd = new ResearchProjectDef();

			rds = researchProjectDefs;
			foreach(ResearchProjectDef researchProjectDef in researchProjectDefs)
            {
				if (researchProjectDef.IsFinished)
				{
					rd = researchProjectDef;
				}
            }

			return rd;
		}
	}
}
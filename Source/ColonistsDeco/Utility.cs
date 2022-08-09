using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;
using Verse;

namespace ColonistsDeco
{
	class Utility
	{
		public static ThingDef wallDef;

		public static ThingDef tornDef;

		public static Dictionary<ThingDef, List<TechLevel>> thingTechProgression = new Dictionary<ThingDef, List<TechLevel>>();

		public static Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)> decoDictionary = new Dictionary<ThingDef, (List<TechLevel>, DecoLocationType)>();

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
					decoDictionary.Add(allDef, (allDef.GetModExtension<DecoModExtension>().decoTechLevels, allDef.GetModExtension<DecoModExtension>().decoLocationType));

					switch (allDef.GetModExtension<DecoModExtension>().decoLocationType)
                    {
						case DecoLocationType.Wall:
							wallDecoDefs.Add(allDef);
							wallDecoHashes.Add(allDef.GetHashCode());

							if(allDef.defName == "DECOPosterTorn")
                            {
								tornDef = allDef;
                            }
							break;
						case DecoLocationType.Bedside:
							bedsideDecoDefs.Add(allDef);
							bedsideDecoHashes.Add(allDef.GetHashCode());
							break;
						case DecoLocationType.Ceiling:
							ceilingDecoDefs.Add(allDef);
							ceilingDecoHashes.Add(allDef.GetHashCode());
							break;
                    }
				}

				switch(allDef.defName)
                {
					case "Wall":
						wallDef = allDef;
						allDef.comps.Add(attachableThingComp);
						wallHashes.Add(allDef.GetHashCode());
						break;
					case var val when new Regex("(Smoothed)+").IsMatch(val):
						wallDef = allDef;
						allDef.comps.Add(attachableThingComp);
						wallHashes.Add(allDef.GetHashCode());
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
			if (ceilingDecoHashes.Contains(thing.def.GetHashCode()))
            {
				return true;
            }
			return false;
		}

		public static bool IsWallDeco(Thing thing)
		{
			if (wallDecoHashes.Contains(thing.def.GetHashCode()))
			{
				return true;
			}
			return false;
		}

		public static bool IsBedsideDeco(Thing thing)
		{
			if (bedsideDecoHashes.Contains(thing.def.GetHashCode()))
			{
				return true;
			}
			return false;
		}

		public static bool IsWall(Thing thing)
		{
			if (wallHashes.Any(h => h == thing.def.GetHashCode()))
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
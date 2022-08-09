using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Settings
{
    internal class ColonistsDecoModSettings : ModSettings
	{
		public int wallDecorationLimit = 2;

		public int ceilingDecorationLimit = 2;

		public int defaultDecoCooldown = 60000;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref wallDecorationLimit, "wallDecorationLimit", 3);
			Scribe_Values.Look(ref ceilingDecorationLimit, "ceilingDecorationLimit", 2);
			Scribe_Values.Look(ref defaultDecoCooldown, "defaultDecoCooldown", 60000);
	}

		public void DoWindowContents(Rect canvas)
		{ 
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = canvas.width;
			listing_Standard.Begin(canvas);
			listing_Standard.Slider(ref wallDecorationLimit, 1, 25, () => "Wall decoration limit: " + wallDecorationLimit, 1f);
			listing_Standard.Gap(32f);
			listing_Standard.Slider(ref ceilingDecorationLimit, 1, 25, () => "Ceiling decoration limit: " + ceilingDecorationLimit, 1f);
			listing_Standard.Gap(32f);
			listing_Standard.Slider(ref defaultDecoCooldown, 0, 240000, () => "Decoration cooldown (in ticks): " + defaultDecoCooldown, 100f);
			if (Current.ProgramState == ProgramState.Playing) {
				listing_Standard.Gap(32f);
				if (listing_Standard.ButtonText("Remove all decorations"))
				{
					Find.WindowStack.Add(new Dialog_Confirm("Are you sure you want to remove all decorations?", RemoveDecos));
				}
			}
			listing_Standard.Gap(32f);
			if (listing_Standard.ButtonText("Restore To Default Settings"))
			{
				Find.WindowStack.Add(new Dialog_Confirm("Are you sure you want to restore Colonists' Deco's settings?", ResetSettings));
			}
			listing_Standard.End();
		}

		private void ResetSettings()
        {
			wallDecorationLimit = 2;
			defaultDecoCooldown = 60000;
        }
		
		private void RemoveDecos()
        {
			List<Thing> allThings = new List<Thing>();

			foreach(IntVec3 c in Current.Game.CurrentMap.AllCells)
            {
				foreach(Thing t in Current.Game.CurrentMap.thingGrid.ThingsListAtFast(c))
                {
					allThings.Add(t);
                }
			}

			foreach(Thing t in allThings)
            {
				if(Utility.IsWallDeco(t) || Utility.IsCeilingDeco(t) || Utility.IsBedsideDeco(t))
                {
					t.Destroy();
                }
            }
        }
	}
}

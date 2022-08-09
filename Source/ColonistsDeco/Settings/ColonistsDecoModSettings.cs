using System.Collections.Generic;
using System.Linq;
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
			var listingStandard = new Listing_Standard
			{
				ColumnWidth = canvas.width
			};
			
			listingStandard.Begin(canvas);
			listingStandard.Slider(ref wallDecorationLimit, 1, 25, () => "Wall decoration limit: " + wallDecorationLimit, 1f);
			listingStandard.Gap(32f);
			listingStandard.Slider(ref ceilingDecorationLimit, 1, 25, () => "Ceiling decoration limit: " + ceilingDecorationLimit, 1f);
			listingStandard.Gap(32f);
			listingStandard.Slider(ref defaultDecoCooldown, 0, 240000, () => "Decoration cooldown (in ticks): " + defaultDecoCooldown, 100f);
			if (Current.ProgramState == ProgramState.Playing) {
				listingStandard.Gap(32f);
				if (listingStandard.ButtonText("Remove all decorations"))
				{
					Find.WindowStack.Add(new Dialog_Confirm("Are you sure you want to remove all decorations?", RemoveDecos));
				}
			}
			listingStandard.Gap(32f);
			if (listingStandard.ButtonText("Restore To Default Settings"))
			{
				Find.WindowStack.Add(new Dialog_Confirm("Are you sure you want to restore Colonists' Deco's settings?", ResetSettings));
			}
			listingStandard.End();
		}

		private void ResetSettings()
        {
			wallDecorationLimit = 2;
			defaultDecoCooldown = 60000;
        }
		
		private void RemoveDecos()
		{
			var allThings = Current.Game.CurrentMap.AllCells
				.SelectMany(c => Current.Game.CurrentMap.thingGrid.ThingsListAtFast(c)).ToList();

			foreach (var t in allThings.Where(t =>
				         Utility.IsWallDeco(t) || Utility.IsCeilingDeco(t) || Utility.IsBedsideDeco(t)))
			{
				t.Destroy();
			}
		}
	}
}

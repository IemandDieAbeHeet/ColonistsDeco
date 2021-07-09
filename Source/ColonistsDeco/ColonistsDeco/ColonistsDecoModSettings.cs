using Verse;
using UnityEngine;

namespace ColonistsDeco
{
    internal class ColonistsDecoModSettings : ModSettings
	{
		public int posterLimit = 2;

		public int defaultDecoCooldown = 60000;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref posterLimit, "posterLimit", 3);
		}

		public void DoWindowContents(Rect canvas)
		{ 
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = canvas.width;
			listing_Standard.Begin(canvas);
			listing_Standard.Slider(ref posterLimit, 1, 25, () => "Poster Limit: " + posterLimit, 1f);
			listing_Standard.Gap(32f);
			listing_Standard.Slider(ref defaultDecoCooldown, 0, 240000, () => "Decoration cooldown (in ticks): " + defaultDecoCooldown, 100f);
			listing_Standard.Gap(32f);
			if (listing_Standard.ButtonText("Restore To Default Settings"))
			{
				Find.WindowStack.Add(new Dialog_Confirm("Are you sure you want to restore Colonists' Deco's settings?", ResetSettings));
			}
			listing_Standard.End();
		}

		private void ResetSettings()
        {
			posterLimit = 2;
			defaultDecoCooldown = 60000;
        }
	}
}

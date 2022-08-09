using ColonistsDeco.Settings;
using UnityEngine;
using Verse;

namespace ColonistsDeco
{
	internal class ColonistsDecoMain : Mod
	{
		public static ColonistsDecoModSettings settings;

		public ColonistsDecoMain(ModContentPack content)
			: base(content)
		{
			settings = GetSettings<ColonistsDecoModSettings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			settings.DoWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Colonists' Deco";
		}
	}
}
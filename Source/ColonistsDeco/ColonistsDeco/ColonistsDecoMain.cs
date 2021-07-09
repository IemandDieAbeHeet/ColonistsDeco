using Verse;
using UnityEngine;

namespace ColonistsDeco
{
	internal class ColonistsDecoMain : Mod
	{
		public static ColonistsDecoModSettings Settings;

		public ColonistsDecoMain(ModContentPack content)
			: base(content)
		{
			Settings = GetSettings<ColonistsDecoModSettings>();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Settings.DoWindowContents(inRect);
		}

		public override string SettingsCategory()
		{
			return "Colonists' Deco";
		}
	}
}
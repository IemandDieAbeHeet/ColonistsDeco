using System;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Settings
{
    internal static class ColonistsDecoModSettingsUtil
    {
		public static void Slider(this Listing_Standard list, ref int value, float min, float max, Func<string> label, float roundTo = -1f)
		{
			float value2 = value;
			var gapHeight = HorizontalSlider(list.GetRect(22f), ref value2, min, max, label?.Invoke(), roundTo);
			value = (int)value2;
			list.Gap(gapHeight);
		}

		public static float HorizontalSlider(Rect rect, ref float value, float leftValue, float rightValue, string label = null, float roundTo = -1f)
		{
			if (label != null)
			{
				var anchor = Text.Anchor;
				var font = Text.Font;
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.Label(rect, label);
				Text.Anchor = anchor;
				Text.Font = font;
				rect.y += 18f;
			}
			value = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (roundTo > 0f)
			{
				value = Mathf.RoundToInt(value / roundTo) * roundTo;
			}

			return 3f;
		}
	}
}

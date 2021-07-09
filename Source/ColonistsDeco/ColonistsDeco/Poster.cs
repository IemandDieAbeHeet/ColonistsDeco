using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ColonistsDeco
{
    class Poster : Building
    {
		public string InspectStringAddon = "Poster: ";

		private Texture2D posterImage = null;
		override public IEnumerable<Gizmo> GetGizmos()
		{
			posterImage = (Texture2D)Graphic.MatSouth.mainTexture;
			Command_Action item = new Command_Action
			{
				defaultLabel = "Inspect Poster",
				defaultDesc = "Take a closer look at the poster that one of your colonists hung up",
				icon = posterImage,
				action = openInspectWindow
			};

			yield return item;
		}

		private void openInspectWindow()
        {
			string decorationName = this.TryGetComp<CompDecoration>().decorationName;
			string decorationCreator = this.TryGetComp<CompDecoration>().decorationCreator;
			Find.WindowStack.Add(new Dialog_Poster("\"" + decorationName + "\", " + "hung up by: " + decorationCreator, posterImage));
		}
	}
}
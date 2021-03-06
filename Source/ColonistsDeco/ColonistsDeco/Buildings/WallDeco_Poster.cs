using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ColonistsDeco
{
    class WallDeco_Poster : Building
    {
		public string InspectStringAddon = "Poster: ";

		private Texture2D decoImage = null;
		private Texture2D inspectIcon = null;

		override public IEnumerable<Gizmo> GetGizmos()
		{
			decoImage = (Texture2D)Graphic.MatSouth.mainTexture;
			inspectIcon = ContentFinder<Texture2D>.Get("Icons/InspectIcon");

			if (inspectIcon == null || decoImage == null)
			{
				yield return null;
			}

			Command_Action item = new Command_Action
			{
				defaultLabel = "Inspect Poster",
				defaultDesc = "Take a closer look at the poster that one of your colonists hung up",
				icon = inspectIcon,
				action = openInspectWindow
			};

			yield return item;
		}

		private void openInspectWindow()
		{
			string decorationName = this.TryGetComp<CompDecoration>().decorationName;
			string decorationCreator = this.TryGetComp<CompDecoration>().decorationCreator;
			Find.WindowStack.Add(new Dialog_Inspect("\"" + decorationName + "\", " + "hung up by: " + decorationCreator, decoImage));
		}
	}
}
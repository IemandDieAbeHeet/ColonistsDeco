using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Buildings
{
    class CeilingDeco_Default : Building
	{
		public string InspectStringAddon = "Decoration: ";

		private Texture2D decoImage = null;
		private Texture2D inspectIcon = null;

		override public IEnumerable<Gizmo> GetGizmos()
		{
			decoImage = (Texture2D)Graphic.ExtractInnerGraphicFor(this).MatSouth.mainTexture;
			inspectIcon = ContentFinder<Texture2D>.Get("Icons/InspectIcon");

			if (inspectIcon == null || decoImage == null)
			{
				yield return null;
			}

			Command_Action item = new Command_Action
			{
				defaultLabel = "Inspect Decoration",
				defaultDesc = "Take a closer look at the decoration that one of your colonists hung up",
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
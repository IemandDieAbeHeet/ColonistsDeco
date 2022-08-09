using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Buildings
{
    class WallDeco_Poster : Building
    {
		public string inspectStringAddon = "Poster: ";

		private Texture2D _decoImage;
		private Texture2D _inspectIcon;

		public WallDeco_Poster(string inspectStringAddon)
		{
			this.inspectStringAddon = inspectStringAddon;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			_decoImage = (Texture2D)Graphic.MatSouth.mainTexture;
			_inspectIcon = ContentFinder<Texture2D>.Get("Icons/InspectIcon");

			if (_inspectIcon == null || _decoImage == null)
			{
				yield return null;
			}

			var item = new Command_Action
			{
				defaultLabel = "Inspect Poster",
				defaultDesc = "Take a closer look at the poster that one of your colonists hung up",
				icon = _inspectIcon,
				action = OpenInspectWindow
			};

			yield return item;
		}

		private void OpenInspectWindow()
		{
			var decorationName = this.TryGetComp<CompDecoration>().decorationName;
			var decorationCreator = this.TryGetComp<CompDecoration>().decorationCreator;
			Find.WindowStack.Add(new Dialog_Inspect("\"" + decorationName + "\", " + "hung up by: " + decorationCreator, _decoImage));
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ColonistsDeco
{
    class CompAttachableThing : ThingComp
    {
		public List<Thing> attachedThings = new List<Thing>();

        public override void CompTick()
        {
            base.CompTick();
            {
	            if (attachedThings == null) return;
	            foreach (var t in attachedThings)
	            {
		            t.Position = parent.Position;
	            }
            }
		}

        public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (attachedThings.Count <= 0)
			{
				return;
			}
			foreach (var attachedThing in attachedThings.Where(attachedThing => attachedThing.Spawned))
			{
				attachedThing.Destroy(DestroyMode.Deconstruct);
			}
			attachedThings.Clear();
		}

		public void AddAttachment(Thing attachment)
		{
			attachedThings.Add(attachment);
		}

		public void RemoveAttachment(Thing attachment)
        {
			attachedThings.Remove(attachment);
        }
	}
}

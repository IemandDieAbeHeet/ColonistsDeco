using System.Collections.Generic;
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
				if (attachedThings != null)
				{
					for (int i = 0; i < attachedThings.Count; i++)
					{
						attachedThings[i].Position = parent.Position;
					}
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
			foreach (Thing attachedThing in attachedThings)
			{
				if (attachedThing.Spawned)
				{
					attachedThing.Destroy(DestroyMode.Deconstruct);
				}
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

using Verse;
using RimWorld;
using UnityEngine;

namespace ColonistsDeco
{
    class BedsideDecoration : Building
    {
        float randomNum1 = Rand.Range(-.1f, .1f);
        float randomNum2 = Rand.Range(-.1f, .1f);

        public override void Print(SectionLayer layer)
        {
            Vector3 a = this.TrueCenter();
            Vector3 center = Vector3.zero;
            Rand.PushState();
            Rand.Seed = base.Position.GetHashCode();
            center += a;
            center.y = (float)def.Altitude;
            center.x += randomNum1;
            center.z += randomNum2;
            Material matSingle = Graphic.MatSingle;
            Printer_Plane.PrintPlane(size: new Vector2(def.graphicData.drawSize.x, def.graphicData.drawSize.y), layer: layer, center: center, mat: matSingle, rot: 0f, flipUv: false, uvs: null, colors: new Color32[4], topVerticesAltitudeBias: 0.1f, uvzPayload: this.HashOffset() % 1024);
            Rand.PopState();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref randomNum1, "randomNum1", 0f);
            Scribe_Values.Look(ref randomNum2, "randomNum2", 0f);
        }
    }
}
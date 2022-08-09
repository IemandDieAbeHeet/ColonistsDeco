using RimWorld;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Buildings
{
    class BedsideDeco_Default : Building
    {
        float randomNum1 = Rand.Range(-.05f, .05f);
        float randomNum2 = Rand.Range(-.05f, .05f);

        public override void Print(SectionLayer layer)
        {
            Vector3 center = this.TrueCenter();
            Rand.PushState();
            Rand.Seed = base.Position.GetHashCode();
            center.y = (float)def.Altitude;
            center.x += randomNum1;
            center.z += randomNum2;
            Rand.PopState();

            Vector2 size;
            bool flag;
            if (Graphic.ShouldDrawRotated)
            {
                size = Graphic.drawSize;
                flag = false;
            }
            else
            {
                size = (this.Rotation.IsHorizontal ? Graphic.drawSize.Rotated() : Graphic.drawSize);
                flag = (this.Rotation == Rot4.West && Graphic.WestFlipped) || (this.Rotation == Rot4.East && Graphic.EastFlipped);
            }

            Material material = Graphic.MatAt(this.Rotation, this);
            Graphic.TryGetTextureAtlasReplacementInfo(material, this.def.category.ToAtlasGroup(), flag, vertexColors: true, out material, out var uvs, out var vertexColor);
            Printer_Plane.PrintPlane(layer, center, size, material, 0f, flag, uvs, new Color32[4] { vertexColor, vertexColor, vertexColor, vertexColor });
            if (Graphic.ShadowGraphic != null && this != null)
            {
                Graphic.ShadowGraphic.Print(layer, this, 0f);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref randomNum1, "randomNum1", 0f);
            Scribe_Values.Look(ref randomNum2, "randomNum2", 0f);
        }
    }
}
using RimWorld;
using UnityEngine;
using Verse;

namespace ColonistsDeco.Buildings
{
    class BedsideDeco_Default : Building
    {
        private float _randomNum1 = Rand.Range(-.05f, .05f);
        private float _randomNum2 = Rand.Range(-.05f, .05f);

        public override void Print(SectionLayer layer)
        {
            var center = this.TrueCenter();
            Rand.PushState();
            Rand.Seed = Position.GetHashCode();
            center.y = def.Altitude;
            center.x += _randomNum1;
            center.z += _randomNum2;
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
                size = (Rotation.IsHorizontal ? Graphic.drawSize.Rotated() : Graphic.drawSize);
                flag = (Rotation == Rot4.West && Graphic.WestFlipped) || (Rotation == Rot4.East && Graphic.EastFlipped);
            }

            var material = Graphic.MatAt(Rotation, this);
            Graphic.TryGetTextureAtlasReplacementInfo(material, def.category.ToAtlasGroup(), flag, vertexColors: true,
                out material, out var uvs, out var vertexColor);
            Printer_Plane.PrintPlane(layer, center, size, material, 0f, flag, uvs,
                new[] {vertexColor, vertexColor, vertexColor, vertexColor});
            Graphic.ShadowGraphic?.Print(layer, this, 0f);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _randomNum1, "randomNum1");
            Scribe_Values.Look(ref _randomNum2, "randomNum2");
        }
    }
}
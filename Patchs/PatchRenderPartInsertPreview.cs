using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayableGarboGal.Patchs
{
    [HarmonyPatch]
    public static class PatchRenderPartInsertPreview
    {
        [HarmonyPatch(typeof(Combat), nameof(Combat.RenderHintsUnderlay)), HarmonyPostfix]
        public static void CombatRenderHintsUnderlayPostfix(Combat __instance, G g)
        {
            PartInsertHilightMarker marker = __instance.fx.Find(x => x is PartInsertHilightMarker) as PartInsertHilightMarker;
            if (marker == null || !marker.isHilighted) return;

            Rect rect = g.state.ship.GetShipRect();
            Draw.Rect(rect.x, rect.y, rect.w, rect.h, Colors.redd);
            // do rendering


            marker.isHilighted = false;
        }
    }
}

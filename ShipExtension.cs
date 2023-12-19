using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayableGarboGal
{
    public static class ShipExtension
    {
        public static void AddPartAt(this Ship ship, Part part, int index)
        {
            ship.x += index < ship.GetCenterPartIndex() ? -1 : 0;
            ship.xLerped = ship.x;
            ship.parts.Insert(index, Mutil.DeepCopy(part));
        }

        public static void RemovePartAt(this Ship ship, int index)
        {
            ship.x += index < ship.GetCenterPartIndex() ? 1 : 0;
            ship.xLerped = ship.x;
            ship.parts.RemoveAt(index);
        }

        public static void RemoveTempParts(this Ship ship)
        {
            for (int i = ship.parts.Count - 1; i >= 0; i--)
            {
                if (ship.parts[i] is TempPart tempPart)
                {
                    ship.RemovePartAt(i);
                }
            }
        }

        public static int GetCenterPartIndex(this Ship ship)
        {
            return (int)Math.Ceiling(ship.parts.Count * .5);
        }
    }
}

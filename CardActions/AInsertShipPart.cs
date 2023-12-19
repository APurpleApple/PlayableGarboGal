using FSPRO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayableGarboGal.CardActions
{
    public class AInsertShipPart : CardAction
    {

        public bool targetPlayer = true;

        public int x;

        public Part part;

        public override void Begin(G g, State s, Combat c)
        {
            Ship ship = (targetPlayer ? s.ship : c.otherShip);
            if (ship != null)
            {
                ship.AddPartAt(part, x);
                Audio.Play(Event.Status_PowerUp);
            }
        }


        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            if (s.route is Combat c)
            {
                PartInsertHilightMarker? marker = c.fx.Find(x => x is PartInsertHilightMarker) as PartInsertHilightMarker;
                if (marker == null)
                {
                    marker = new PartInsertHilightMarker();
                    c.fx.Add(marker);
                }

                marker.isHilighted = true;
                marker.insertIndex = x;
            }

            return list;
        }
    }
}

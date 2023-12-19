using PlayableGarboGal.CardActions;

namespace PlayableGarboGal.Cards
{
    public class TestCard : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AInsertShipPart() { x = GetInsertPos(c, s), part = Mod.parts["TempCannon"].GetPartObject() as Part });
            return actions;
        }

        public override void OnExitCombat(State s, Combat c)
        {
            s.ship.RemoveTempParts();
        }

        private int GetInsertPos(Combat c, State s)
        {
            return c.hand.FindIndex((x) => x == this) - (int)Math.Ceiling(c.hand.Count * .5) + (int)Math.Ceiling(s.ship.parts.Count * .5);
        }

        public override CardData GetData(State state)
        {
            CardData data = base.GetData(state);
            data.cost = 0;
            data.flippable = true;
            data.infinite = true;
            return data;
        }


        public override void OnFlip(G g)
        {
            if (g.state.route is Combat c)
            {
                int index = (c.hand.FindIndex((x) => x == this) + 1) % c.hand.Count;
                c.hand.Remove(this);
                c.hand.Insert(index, this);
            }
        }
    }
}

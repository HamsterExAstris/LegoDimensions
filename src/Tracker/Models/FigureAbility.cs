namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class FigureAbility
    {
        public int FigureId { get; set; }
        public Figure Figure { get; set; }

        public int AbilityId { get; set; }
        public Ability Ability { get; set; }
    }
}

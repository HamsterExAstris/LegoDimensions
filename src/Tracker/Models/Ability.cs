using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class Ability
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AbilityId { get; set; }
        public string LongDesc { get; set; }

        public virtual IEnumerable<FigureAbility> FigureAbilities { get; set; }
    }
}

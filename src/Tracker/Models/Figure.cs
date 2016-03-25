using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class Figure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FigureId { get; set; }
        public string Name { get; set; }
        public FranchiseType FranchiseType { get; set; }

        public int PackId { get; set; }
        public Pack Pack { get; set; }

        public virtual IEnumerable<FigureAbility> FigureAbilities { get; set; }
    }
}

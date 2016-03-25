using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class Pack
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PackId { get; set; }
        public string Name { get; set; }
        public int? WaveId { get; set; }
        public Wave Wave { get; set; }
        public PackType PackType { get; set; }
        public FranchiseType FranchiseType { get; set; }

        public IEnumerable<Figure> Figures { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} Pack", this.Name, this.PackType);
        }
    }

    public enum PackType
    {
        Starter,
        Level,
        Team,
        Fun
    }

    public enum FranchiseType
    {
        Multiple = -1,
        BackToTheFuture,
        Chima,
        DCComics,
        DoctorWho,
        Ghostbusters,
        JurassicWorld,
        TheLegoMovie,
        TheLordOfTheRings,
        MidwayArcade,
        Ninjago,
        Portal2,
        ScoobyDoo,
        TheSimpsons,
        TheWizardOfOz
    }
}

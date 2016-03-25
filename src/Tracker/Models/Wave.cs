using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class Wave
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WaveId { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}

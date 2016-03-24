using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Figure> Figures { get; set; }
        public DbSet<Pack> Packs { get; set; }
        public DbSet<Wave> Waves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Set up the many-to-many relationship between Figure and Ability.
            builder.Entity<FigureAbility>()
                .HasKey(fa => new { fa.AbilityId, fa.FigureId });
            builder.Entity<FigureAbility>()
                .HasOne<Ability>(fa => fa.Ability)
                .WithMany(a => a.FigureAbilities)
                .HasForeignKey(fa => fa.AbilityId);
            builder.Entity<FigureAbility>()
                .HasOne<Figure>(fa => fa.Figure)
                .WithMany(f => f.FigureAbilities)
                .HasForeignKey(fa => fa.FigureId);
        }
    }
}

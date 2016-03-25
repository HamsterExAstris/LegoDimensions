using System;
using System.Linq;

namespace ShatteredTemple.LegoDimensions.Tracker.Models
{
    public class SeedData
    {
        private ApplicationDbContext context_;

        public SeedData(ApplicationDbContext context)
        {
            this.context_ = context;
        }

        public void SeedContext()
        {
            SeedWaves(this.context_);
            // Explicitly save after seeing the waves because otherwise .Single() on the waves when seeding packs fails.
            // TODO: That seems like a bug, need to investigate more.
            this.context_.SaveChanges();

            SeedPacks(this.context_);
            this.context_.SaveChanges();
        }

        private static void SeedPacks(ApplicationDbContext context)
        {
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71171,
                Name = "Starter Pack",
                PackType = PackType.Starter,
                FranchiseType = FranchiseType.Multiple,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71201,
                Name = "Back to the Future\u2122 Level Pack",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.BackToTheFuture,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71202,
                Name = "The Simpsons\u2122",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.TheSimpsons,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71203,
                Name = "Portal\u00AE 2",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.Portal2,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71204,
                Name = "Doctor Who",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.DoctorWho,
                Wave = context.Waves.Single(w => w.WaveId == 2)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71205,
                Name = "Jurassic World\u2122",
                PackType = PackType.Team,
                FranchiseType = FranchiseType.JurassicWorld,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71206,
                Name = "Scooby-Doo!\u2122",
                PackType = PackType.Team,
                FranchiseType = FranchiseType.ScoobyDoo,
                Wave = context.Waves.Single(w => w.WaveId == 1)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71207,
                Name = "LEGO\u00AE Ninjago\u2122",
                PackType = PackType.Team,
                FranchiseType = FranchiseType.Ninjago,
                Wave = context.Waves.Single(w => w.WaveId == 2)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71228,
                Name = "Ghostbusters\u2122",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.Ghostbusters,
                Wave = context.Waves.Single(w => w.WaveId == 3)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71229,
                Name = "DC Comics\u2122",
                PackType = PackType.Team,
                FranchiseType = FranchiseType.DCComics,
                Wave = context.Waves.Single(w => w.WaveId == 3)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71233,
                Name = "Stay Puft",
                PackType = PackType.Fun,
                FranchiseType = FranchiseType.Ghostbusters,
                Wave = context.Waves.Single(w => w.WaveId == 4)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71234,
                Name = "Sensei Wu",
                PackType = PackType.Fun,
                FranchiseType = FranchiseType.Ninjago,
                Wave = context.Waves.Single(w => w.WaveId == 3)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71235,
                Name = "Midway Arcade\u2122",
                PackType = PackType.Level,
                FranchiseType = FranchiseType.MidwayArcade,
                Wave = context.Waves.Single(w => w.WaveId == 4)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71236,
                Name = "Superman",
                PackType = PackType.Fun,
                FranchiseType = FranchiseType.DCComics,
                Wave = context.Waves.Single(w => w.WaveId == 4)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71237,
                Name = "Aquaman",
                PackType = PackType.Fun,
                FranchiseType = FranchiseType.DCComics,
                Wave = context.Waves.Single(w => w.WaveId == 4)
            });
            CreateOrUpdate(context, new Pack()
            {
                PackId = 71238,
                Name = "Cyberman",
                PackType = PackType.Fun,
                FranchiseType = FranchiseType.DoctorWho,
                Wave = context.Waves.Single(w => w.WaveId == 3)
            });
        }

        private static void SeedWaves(ApplicationDbContext context)
        {
            CreateOrUpdate(context, new Wave()
            {
                WaveId = 1,
                ReleaseDate = new DateTime(2015, 09, 27)
            });
            CreateOrUpdate(context, new Wave()
            {
                WaveId = 2,
                ReleaseDate = new DateTime(2015, 11, 03)
            });
            CreateOrUpdate(context, new Wave()
            {
                WaveId = 3,
                ReleaseDate = new DateTime(2016, 01, 19)
            });
            CreateOrUpdate(context, new Wave()
            {
                WaveId = 4,
                ReleaseDate = new DateTime(2016, 03, 15)
            });
            CreateOrUpdate(context, new Wave()
            {
                WaveId = 5,
                ReleaseDate = new DateTime(2016, 05, 10)
            });
        }

        private static void CreateOrUpdate(ApplicationDbContext context, Figure figure)
        {
            Figure dbFigure;
            if (figure.FigureId > 0)
            {
                // Unlikely for now, but hey.
                dbFigure = context.Figures.SingleOrDefault(f => f.FigureId == figure.FigureId);
            }
            else
            {
                dbFigure = context.Figures.SingleOrDefault(f => f.Name == figure.Name);
            }
            if (dbFigure == null)
            {
                dbFigure = new Figure();
                context.Figures.Add(figure);
            }
            dbFigure.Name = figure.Name;
        }

        private static void CreateOrUpdate(ApplicationDbContext context, Pack pack)
        {
            var dbPack = context.Packs.SingleOrDefault(p => p.PackId == pack.PackId);
            if (dbPack == null)
            {
                dbPack = new Pack()
                {
                    PackId = pack.PackId
                };
                context.Packs.Add(pack);
            }
            dbPack.Name = pack.Name;
            dbPack.PackType = pack.PackType;
            dbPack.Wave = pack.Wave;
        }

        private static void CreateOrUpdate(ApplicationDbContext context, Wave wave)
        {
            var dbWave = context.Waves.SingleOrDefault(w => w.WaveId == wave.WaveId);
            if (dbWave == null)
            {
                dbWave = new Wave()
                {
                    WaveId = wave.WaveId
                };
                context.Waves.Add(wave);
            }
            dbWave.ReleaseDate = wave.ReleaseDate;
        }
    }
}

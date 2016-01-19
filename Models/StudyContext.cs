using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WASP_F_E.Models
{
    class StudyContext:DbContext
    {
        public DbSet<Study> Studies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Scenario>()
               .HasRequired(t => t.Study)
               .WithMany(t => t.Scenarios)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<PlantType>()
               .HasRequired(t => t.Study)
               .WithMany(t => t.PlantTypes)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<EmissionType>()
               .HasRequired(t => t.Study)
               .WithMany(t => t.Emissions)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<HPlant>()
               .HasRequired(t => t.Scenario)
               .WithMany(t => t.HPlants)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<TPlant>()
               .HasRequired(t => t.Scenario)
               .WithMany(t => t.TPlants)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<Plant>()
               .HasRequired(t => t.Type)
               .WithMany(t => t.Plants)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<EmissionValue>()
               .HasRequired(t => t.Plant)
               .WithMany(t => t.Emissions)
               .WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }
    }
}

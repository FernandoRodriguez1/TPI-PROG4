using MatchTickets.Domain.Entities;
using MatchTickets.Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MatchTickets.Infraestructure.Data
{
    public class DbContextCR : DbContext
    {
        public DbContextCR(DbContextOptions<DbContextCR> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<MembershipCard> MembershipCards { get; set; }
        public DbSet<SoccerMatch> SoccerMatches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                // Discriminador
                entity.HasDiscriminator<UserType>("UserType")
                      .HasValue<Admin>(UserType.Admin)
                      .HasValue<Client>(UserType.Client);

                // Configuración de Email como Value Object
                entity.OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("Email")
                         .IsRequired();
                });
            });



            // Relacion Client  Tickets (1:N)
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Tickets");
                entity.HasKey(e => e.TicketId);

                entity.HasOne(t => t.Client)           // Ticket tiene un Client
                      .WithMany(c => c.Tickets)        // Client tiene muchos Tickets
                      .HasForeignKey(t => t.ClientId);

                entity.HasOne(t => t.SoccerMatch)      // Ticket pertenece a un partido
                      .WithMany(sm => sm.Tickets)
                      .HasForeignKey(t => t.SoccerMatchId);
            });

            // Relacion Club MembershipCards (1:N)
            modelBuilder.Entity<MembershipCard>(entity =>
            {
                entity.ToTable("MembershipCards");
                entity.HasKey(e => e.MembershipId);

                entity.HasOne(mc => mc.Club)                  // MembershipCard tiene un Club
                      .WithMany(c => c.MembershipCards)      // Club tiene muchas MembershipCards
                      .HasForeignKey(mc => mc.ClubId);

                // Relacion MembershipCard Client (1:1)
                entity.HasOne(mc => mc.Client)
                      .WithOne(c => c.MembershipCard)
                      .HasForeignKey<MembershipCard>(mc => mc.ClientId);
            });

            // RelacionClub SoccerMatches (1:N)
            modelBuilder.Entity<SoccerMatch>(entity =>
            {
                entity.ToTable("SoccerMatches");
                entity.HasKey(e => e.SoccerMatchId);

                entity.HasOne(sm => sm.Club)                  // Partido pertenece a un Club
                      .WithMany(c => c.SoccerMatches)
                      .HasForeignKey(sm => sm.ClubId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}

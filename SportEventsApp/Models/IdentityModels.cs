using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SportEventsApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            EventUsers = new List<EventUsers>();
            Matches = new List<Match>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        [RegularExpression(@"^(011|010|012|015)([0-9]{8})$")]
        public string Mobile { get; set; }


        [ForeignKey("Group")]
        [Display(Name = "Group")]
        public int? Group_ID { get; set; }
        public virtual Group Group { get; set; }

        public virtual List<EventUsers> EventUsers { get; set; }
        public virtual List<Match> Matches { get; set; }

        //[ForeignKey("Store")]
        //[Display(Name = "Sore")]
        //public int? StoreId { get; set; }
        [InverseProperty("Owner")]
        public virtual List<Store> Stores { get; set; }

        public string CashNumber { get; set; }
        public string Address { get; set; }

        [ForeignKey("City")]
        [Display(Name = "City")]
        public string City_ID { get; set; }
        public virtual City City { get; set; }

        public string PictureUrl { get; set; }



    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventUsers> EventUsers { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<VodafoneCash> VodafoneCashs { get; set; }
        public virtual DbSet<EtisalatCash> EtisalatCashs { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<StorePhotos> StorePhotos { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<EntryFees> EntryFees { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany<Match>(s => s.Matches)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("MatchId");
                    cs.ToTable("UsersMatches");
                });
            modelBuilder.Entity<EventUsers>()
                .HasKey(c => new { c.EventId, c.UserId });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(c => c.EventUsers)
                .WithRequired()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Event>()
                .HasMany(c => c.EventUsers)
                .WithRequired()
                .HasForeignKey(c => c.EventId);

            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOptional(s => s.Store) // Mark Address property optional in Student entity
            //    .WithRequired(ad => ad.Owner);
                


        }
    }
}
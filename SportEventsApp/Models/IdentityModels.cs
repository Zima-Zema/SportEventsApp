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
    public static class UserRoles
    {
        public static string Admin { get { return "Admin"; } }
        public static string Player { get { return "Player"; } }
        public static string StoreOwner { get { return "StoreOwner"; } }
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {
            EventUsers = new List<EventUsers>();
            Matches = new List<Match>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            //userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier,this.Id));
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        [RegularExpression(@"^(011|010|012|015)([0-9]{8})$")]
        public string Mobile { get; set; }
        
        public string Name { get; set; }

        public virtual List<EventUsers> EventUsers { get; set; }
        public virtual List<Match> Matches { get; set; }

        [InverseProperty("Creator")]
        public virtual List<Match> MyMatches { get; set; }

        //[ForeignKey("Store")]
        //[Display(Name = "Sore")]
        //public int? StoreId { get; set; }
        [InverseProperty("Owner")]
        public virtual List<Store> Stores { get; set; }
        
        public string Address { get; set; }

        [ForeignKey("City")]
        [Display(Name = "City")]
        public string City_ID { get; set; }
        public virtual City City { get; set; }
        public string PictureUrl { get; set; }
        public bool ValidUser { get; set; } = false;



    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
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
        public virtual DbSet<InfoPoint> Points { get; set; }
        public virtual DbSet<RegularInfo> Infos { get; set; }
        public virtual DbSet<NestedInfo> NestedInfos { get; set; }

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
            modelBuilder.Entity<Event>()
                .HasMany<Store>(s => s.Stores)
                .WithMany(c => c.Events)
                .Map(cs =>
                {
                    cs.MapLeftKey("EventId");
                    cs.MapRightKey("StoreId");
                    cs.ToTable("EventsStores");
                });
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasMany<Group>(s => s.Groups)
            //    .WithMany(c => c.Users)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("UserId");
            //        cs.MapRightKey("GroupId");
            //        cs.ToTable("UsersGroups");
            //    });
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
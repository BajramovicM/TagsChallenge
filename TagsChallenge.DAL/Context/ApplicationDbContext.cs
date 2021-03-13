using TagsChallenge.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TagsChallenge.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Link> Links { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<LinkTag> LinkTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LinkTag>()
                .HasKey(lt => new { lt.LinkId, lt.TagId });
            modelBuilder.Entity<LinkTag>()
                .HasOne(lt => lt.Link)
                .WithMany(l => l.LinkTags)
                .HasForeignKey(lt => lt.LinkId);
            modelBuilder.Entity<LinkTag>()
                .HasOne(lt=> lt.Tag)
                .WithMany(t => t.LinkTags)
                .HasForeignKey(lt => lt.TagId);
        }
    }
}

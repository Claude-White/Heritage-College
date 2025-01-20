using Microsoft.EntityFrameworkCore;

namespace HFA.Models
{
    public class HFAContext(DbContextOptions<HFAContext> options) : DbContext(options)
    {
        public virtual DbSet<Executive> Executives { get; set; } = null!;

        public virtual DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; } = null!;

        public virtual DbSet<Article> Articles { get; set; } = null!;

        public virtual DbSet<Mission> Missions { get; set; } = null!;

        public virtual DbSet<ElectedMember> ElectedMembers { get; set; } = null!;

        public virtual DbSet<ElectedGroup> ElectedGroups { get; set; } = null!;

        public virtual DbSet<ReleaseNotes> ReleaseNotes { get; set; } = null!;

        public virtual DbSet<Image> Images { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FrequentlyAskedQuestion>().HasKey(faq => faq.FaqId);
            modelBuilder.Entity<ElectedGroup>().HasKey(group => group.GroupId);
            modelBuilder.Entity<ElectedMember>().HasKey(member => member.MemberId);
            modelBuilder.Entity<ReleaseNotes>().HasKey(note => note.ReleaseNoteId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.HeroImage)
                .WithOne()
                .HasForeignKey<Article>(a => a.HeroImageId);
        }
    }
}

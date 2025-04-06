using Microsoft.EntityFrameworkCore;
using Share.Model;

namespace AssignmentService.Repository
{
    public class AssigmentDbContext : DbContext
    {
        public AssigmentDbContext(DbContextOptions<AssigmentDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure the database connection string here if needed
            // optionsBuilder.UseSqlServer("YourConnectionString");
            optionsBuilder.UseSqlServer("Server=HUNGNM;Database=AssignmentDb;UserId=sa;Password=123;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key for the Assignment entity
            modelBuilder.Entity<Assignment>()
                .HasKey(a => a.AssignmentId);
            // Configure the primary key for the AssignmentAttachment entity
            modelBuilder.Entity<AssignmentAttachment>()
                .HasKey(aa => aa.AttachmentId);
            // Configure the primary key for the AssignmentComment entity
            modelBuilder.Entity<AssignmentComment>()
                .HasKey(ac => ac.CommentId);
            // Configure the primary key for the AssignmentSubmission entity
            modelBuilder.Entity<AssignmentSubmission>()
                .HasKey(sub =>sub.SubmissionId);
            // Configure the primary key for the SubmissionAttachment entity
            modelBuilder.Entity<SubmissionAttachment>()
                .HasKey(sa => sa.AttachmentId);
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentAttachment> AssignmentAttachments { get; set; }
        public DbSet<AssignmentComment> AssignmentComments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<SubmissionAttachment> SubmissionAttachments { get; set; }
    }
}

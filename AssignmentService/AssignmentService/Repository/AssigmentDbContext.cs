using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other;

namespace AssignmentService.Repository
{
    public class AssigmentDbContext : DbContext
    {
        public AssigmentDbContext(DbContextOptions<AssigmentDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region fluent api for Assignment
            modelBuilder.Entity<Assignment>(assigment =>
            {
                assigment.HasKey(a => a.AssignmentId);
                assigment.Property(a => a.AssignmentId)
                    .ValueGeneratedOnAdd();
                assigment.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(255);
                assigment.Property(a => a.Description)
                    .IsRequired()
                    .HasMaxLength(4000);
                assigment.Property(a => a.Status)
                      .IsRequired()
                      .HasMaxLength(20)
                      .HasDefaultValue(AssignmentStatus.Draf);
                assigment.Property(a => a.CreatedAt)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETDATE()");
                assigment.Property(a => a.UpdatedAt)
                      .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAddOrUpdate();
                assigment.Property(a => a.Deadline)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("GETDATE()");
                assigment.Property(a => a.ClassroomId)
                      .IsRequired()
                      .IsUnicode(false)
                      .HasMaxLength(255);
                assigment.Property(a => a.TeacherId)
                      .IsRequired()
                      .IsUnicode(false)
                      .HasMaxLength(255);

                assigment.HasMany(a => a.Attachments)
                .WithOne(attachment => attachment.Assignment)
                .HasForeignKey(attachment => attachment.AssignmentId)
                .HasConstraintName("FK_Assignment_Attachments")
                .OnDelete(DeleteBehavior.Cascade);

                assigment.HasMany(a => a.Comments)
                .WithOne(comment => comment.Assignment)
                .HasForeignKey(attachment => attachment.AssignmentId)
                .HasConstraintName("FK_Assignment_Comment")
                .OnDelete(DeleteBehavior.Cascade);

                assigment.HasMany(a => a.Submissions)
                .WithOne(submission => submission.Assignment)
                .HasForeignKey(submission => submission.AssignmentId)
                .HasConstraintName("FK_Assignment_Submission")
                .OnDelete(DeleteBehavior.Cascade);
                assigment.ToTable("Assignments");
            });
            #endregion

            #region fluent api for AssignmentAttachment
            modelBuilder.Entity<AssignmentAttachment>(attachment =>
            {
                attachment.HasKey(a => a.AttachmentId);
                attachment.Property(a => a.AttachmentId)
                    .ValueGeneratedOnAdd();
                attachment.Property(a => a.FileUrl)
                .HasMaxLength(1000);
                attachment.Property(a => a.FileType)
                .HasMaxLength(20);
                attachment.Property(a => a.UploadedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();
                attachment.Property(a => a.AssignmentId)
                .IsRequired();

                attachment.HasOne(x => x.Assignment)
                .WithMany(Assignment => Assignment.Attachments)
                .HasConstraintName("FK_Assignment_Attachments")
                .HasForeignKey(att => att.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

                attachment.ToTable("AssignmentAttachments");
            });
            #endregion

            #region fluent api for AssignmentComment
            modelBuilder.Entity<AssignmentComment>(comment =>
            {
                comment.HasKey(c => c.CommentId);
                comment.Property(c => c.CommentId)
                    .ValueGeneratedOnAdd();
                comment.Property(c => c.Content)
                .HasMaxLength(4000)
                .IsRequired();
                comment.Property(c => c.AssignmentId)
                .IsRequired();
                comment.Property(c => c.UserId)
                .HasMaxLength(255)
                .IsRequired();
                comment.Property(c => c.ParentCommentId)
                .IsRequired(false);
                comment.Property(c => c.CreatedAt)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd();
                comment.Property(c => c.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

                comment.HasOne(c => c.Assignment)
                      .WithMany(a => a.Comments)
                      .HasForeignKey(c => c.AssignmentId)
                      .HasConstraintName("FK_Assignment_Comment")
                      .OnDelete(DeleteBehavior.Cascade);

                comment.HasOne(c => c.ParentComment)
                      .WithMany(c => c.Replies)
                      .HasForeignKey(c => c.ParentCommentId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .HasConstraintName("FK_Cmt_Cmt");

                comment.ToTable("Comments");
            });
            #endregion

            #region fluent api for AssignmentSubmission
            modelBuilder.Entity<AssignmentSubmission>(entity =>
            {
                entity.HasKey(e => e.SubmissionId);

                entity.Property(e => e.SubmissionId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.AssignmentId)
                      .IsRequired();

                entity.Property(e => e.StudentId)
                      .IsRequired();

                entity.Property(e => e.SubmittedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Content)
                      .HasMaxLength(4000)
                      .IsUnicode(true);

                entity.Property(e => e.Status)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Grade)
                      .HasColumnType("float");

                entity.Property(e => e.Feedback)
                      .HasMaxLength(2000)
                      .IsUnicode(true);

                entity.Property(e => e.QuizAnswer)
                    .IsUnicode(true);

                entity.HasOne(e => e.Assignment)
                      .WithMany(a => a.Submissions)
                      .HasForeignKey(e => e.AssignmentId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Assignment_Submissions");

                entity.HasMany(e => e.Attachments)
                      .WithOne(a => a.Submission)
                      .HasForeignKey(a => a.SubmissionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Submission_Attachments");

                entity.ToTable("Submissions");
            });
            #endregion

            #region fluent api for SubmissionAttachment
            modelBuilder.Entity<SubmissionAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentId);

                entity.Property(e => e.AttachmentId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.SubmissionId)
                      .IsRequired();

                entity.Property(e => e.FileUrl)
                      .HasMaxLength(1000)
                      .IsUnicode(false);
                entity.Property(e => e.FileType)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.UploadedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Submission)
                      .WithMany(s => s.Attachments)
                      .HasForeignKey(e => e.SubmissionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Submission_Attachments");

                entity.ToTable("SubmissionAttachments");
            });
            #endregion

            #region fluent api for Question
            modelBuilder.Entity<Question>(question =>
            {
                question.HasKey(q => q.Id);

                question.Property(q => q.Id)
                    .ValueGeneratedOnAdd();

                question.Property(q => q.Content)
                    .IsRequired()
                    .HasMaxLength(4000);

                question.Property(q => q.AssignmentId)
                    .IsRequired();

                question.HasOne(q => q.Assignment)
                    .WithMany(a => a.Questions)
                    .HasForeignKey(q => q.AssignmentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Question_Assignment");

                question.HasMany(q => q.Answers)
                    .WithOne(a => a.Question)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Question_Answers");

                question.ToTable("Questions");
            });
            #endregion

            #region fluent api for Answer
            modelBuilder.Entity<Answer>(answer =>
            {
                answer.HasKey(a => a.Id);

                answer.Property(a => a.Id)
                      .ValueGeneratedOnAdd();

                answer.Property(a => a.Content)
                      .IsRequired()
                      .HasMaxLength(4000);

                answer.Property(a => a.IsCorrect)
                      .IsRequired();

                answer.Property(a => a.QuestionId)
                      .IsRequired();

                answer.HasOne(a => a.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Answer_Question");

                answer.ToTable("Answers");
            });
            #endregion
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AssignmentAttachment> AssignmentAttachments { get; set; }
        public DbSet<AssignmentComment> AssignmentComments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<SubmissionAttachment> SubmissionAttachments { get; set; }
    }
}

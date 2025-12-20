using Back_Quiz.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Back_Quiz.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<QuizTemplate> QuizTemplates { get; set; }
    public DbSet<QuizResult> QuizResults { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<QuizResult>()
            .HasOne(q => q.QuizTemplate)
            .WithMany()
            .HasForeignKey(q => q.QuizTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<AppUser>()
            .HasMany(a => a.QuizResults)
            .WithOne(q => q.User)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
}
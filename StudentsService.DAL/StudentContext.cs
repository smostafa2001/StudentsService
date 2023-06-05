using Microsoft.EntityFrameworkCore;
using StudentsService.DAL.Entities;

namespace StudentsService.DAL;
public class StudentContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("SERVER=.; INITIAL CATALOG=StudentDb; USER ID=sa; PASSWORD=1;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasMany(s => s.PhoneNumbers).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}

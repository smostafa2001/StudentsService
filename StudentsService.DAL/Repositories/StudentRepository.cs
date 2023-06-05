using Microsoft.EntityFrameworkCore;
using StudentsService.DAL.Entities;
using StudentsService.DAL.Mappers;
using StudentsService.Domain.Models;
using StudentsService.Domain.Repositories;

namespace StudentsService.DAL.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly StudentContext _context;

    public StudentRepository(StudentContext context) => _context = context;

    public async Task<int> CreateAsync(StudentModel studentForCreate)
    {
        var student = new Student
        {
            FirstName = studentForCreate.FirstName,
            LastName = studentForCreate.LastName,
            Description = studentForCreate.Description,
            StudentNumber = studentForCreate.StudentNumber
        };

        foreach (var item in studentForCreate.PhoneNumbers)
        {
            student.PhoneNumbers.Add(new PhoneNumber { Number = item });
        }

        await _context.AddAsync(student);
        await _context.SaveChangesAsync();
        return student.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var student = new Student { Id = id };
        _context.Remove(student);
        var result = await _context.SaveChangesAsync();
        return result;
    }

    public async Task<IEnumerable<StudentModel>> GetAllAsync() => await _context.Students.Include(s => s.PhoneNumbers).AsNoTracking()
        .Select(s => s.ToModel()).ToListAsync();

    public async Task<StudentModel> GetAsync(int id) => await _context.Students.Include(s => s.PhoneNumbers).AsNoTracking()
        .Select(s => s.ToModel()).FirstOrDefaultAsync(s => s.Id == id);
    public async Task<int> UpdateAsync(StudentForUpdateModel studentForUpdate)
    {
        var student = new Student
        {
            Id = studentForUpdate.Id,
            FirstName = studentForUpdate.FirstName,
            LastName = studentForUpdate.LastName,
            Description = studentForUpdate.Description
        };

        _context.Entry(student).Property(s => s.FirstName).IsModified = true;
        _context.Entry(student).Property(s => s.LastName).IsModified = true;
        _context.Entry(student).Property(s => s.Description).IsModified = true;
        var result = await _context.SaveChangesAsync();
        return result;
    }
}

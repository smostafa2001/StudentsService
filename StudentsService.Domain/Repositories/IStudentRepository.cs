using StudentsService.Domain.Models;

namespace StudentsService.Domain.Repositories;
public interface IStudentRepository
{
    Task<IEnumerable<StudentModel>> GetAllAsync();
    Task<StudentModel> GetAsync(int id);
    Task<int> CreateAsync(StudentModel studentForCreate);
    Task<int> DeleteAsync(int id);
    Task<int> UpdateAsync(StudentForUpdateModel studentForUpdate);
}

using Microsoft.Extensions.Logging;
using StudentsService.Domain.Models;
using StudentsService.Domain.Repositories;
using StudentsService.Domain.Services;

namespace StudentsService.BLL.Services;
public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository repository, ILogger<StudentService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> CreateAsync(StudentModel studentForCreate)
    {
        var result = await _repository.CreateAsync(studentForCreate);
        _logger.LogDebug($"Student Created {result}");
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        return result == 1;
    }

    public async Task<StudentModel> GetAsync(int id) => await _repository.GetAsync(id);

    public async Task<IEnumerable<StudentModel>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<bool> UpdateAsync(StudentForUpdateModel studentForUpdate)
    {
        var result = await _repository.UpdateAsync(studentForUpdate);
        return result == 1;
    }
}

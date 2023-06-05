using StudentsService.DAL.Entities;
using StudentsService.Domain.Models;

namespace StudentsService.DAL.Mappers;
public static class StudentMapper
{
    public static StudentModel ToModel(this Student student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        Description = student.Description,
        StudentNumber = student.StudentNumber,
        PhoneNumbers = student.PhoneNumbers.Select(pn => pn.Number).ToList()
    };

}

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using StudentsService.Domain.Services;
using StudentsService.Grpc.Protos.v1;
using static StudentsService.Grpc.Protos.v1.StudentService;
using Models = StudentsService.Domain.Models;

namespace StudentsService.Grpc.Services.v1;

public class StudentService : StudentServiceBase
{
    private readonly IStudentService _studentService;

    public StudentService(IStudentService studentService) => _studentService = studentService;

    public override async Task CreateStudent(IAsyncStreamReader<StudentCreateRequest> requestStream, IServerStreamWriter<StudentCreateReply> responseStream, ServerCallContext context)
    {
        await foreach (var item in requestStream.ReadAllAsync())
        {
            var serviceResult = await _studentService.CreateAsync(new Models.StudentModel
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Description = item.Description,
                StudentNumber = item.StudentNumber,
                PhoneNumbers = new List<string>(item.PhoneNumbers)
            });
            await responseStream.WriteAsync(new StudentCreateReply
            {
                Id = serviceResult
            });
        }

        await Task.CompletedTask;
    }

    public override async Task<StudentDeleteReply> DeleteStudent(StudentByIdRequest request, ServerCallContext context)
    {
        var serviceResult = await _studentService.DeleteAsync(request.Id);
        return new StudentDeleteReply { Success = serviceResult };
    }

    public override async Task<StudentReply> Get(StudentByIdRequest request, ServerCallContext context)
    {
        var student = await _studentService.GetAsync(request.Id);
        var status = new Status(StatusCode.NotFound, $"Student with id {request.Id} not found!");
        if (student is null) throw new RpcException(status);
        var reply = new StudentReply
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Description = student.Description,
            StudentNumber = student.StudentNumber
        };
        reply.PhoneNumbers.AddRange(student.PhoneNumbers);
        return reply;
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<StudentReply> responseStream, ServerCallContext context)
    {
        var students = await _studentService.GetAllAsync();
        foreach (var student in students)
        {
            var reply = new StudentReply
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Description = student.Description,
                StudentNumber = student.StudentNumber
            };
            reply.PhoneNumbers.AddRange(student.PhoneNumbers);
            await responseStream.WriteAsync(reply);
        }

        await Task.CompletedTask;
    }

    public override async Task<StudentUpdateReply> UpdateStudent(StudentUpdateRequest request, ServerCallContext context)
    {
        var serviceResult = await _studentService.UpdateAsync(new Models.StudentForUpdateModel
        {
            Id = request.Id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Description = request.Description
        });
        return new StudentUpdateReply { Success = serviceResult };
    }
}

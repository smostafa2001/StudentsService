using StudentsService.BLL.Services;
using StudentsService.DAL;
using StudentsService.DAL.Repositories;
using StudentsService.Domain.Repositories;
using StudentsService.Domain.Services;
using StudentsService.Grpc.Infrastructures;
using StudentsService.Grpc.Interceptors;
using GrpcServices = StudentsService.Grpc.Services.v1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<ProtoFileProvider>();
builder.Services.AddDbContext<StudentContext>();

builder.Services.AddGrpcReflection();
builder.Services.AddGrpc(c =>
{
    c.EnableDetailedErrors = true;
    c.Interceptors.Add<ExceptionInterceptor>();
});

var app = builder.Build();
app.MapGrpcReflectionService();
app.MapGrpcService<GrpcServices.StudentService>();
app.MapGet("/protos", (ProtoFileProvider protoFileProvider) => Results.Ok(protoFileProvider.GetAll()));
app.MapGet("/protos/v{version:int}/{protoName}", (ProtoFileProvider protoFileProvider, int version, string protoName) =>
{
    string filePath = protoFileProvider.GetPath(version, protoName);
    return string.IsNullOrEmpty(filePath)
        ? Results.NotFound()
        : Results.File(filePath);
});
app.MapGet("/protos/v{version:int}/{protoName}/view", async (ProtoFileProvider protoFileProvider, int version, string protoName) =>
{
    string fileContent = await protoFileProvider.GetContent(version, protoName);
    return string.IsNullOrEmpty(fileContent)
        ? Results.NotFound()
        : Results.Text(fileContent);
});
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

namespace StudentsService.Grpc.Infrastructures;

public class ProtoFileProvider
{
    private readonly string _contentRootPath;
    public ProtoFileProvider(IWebHostEnvironment environment) => _contentRootPath = environment.ContentRootPath;
    public Dictionary<string, IEnumerable<string?>> GetAll() =>
        Directory.GetDirectories($"{_contentRootPath}/Protos").Select(d => new
        {
            Version = d,
            Protos = Directory.GetFiles(d).Select(Path.GetFileName)
        }).ToDictionary(key => Path.GetRelativePath("Protos", key.Version), value => value.Protos);
    public async Task<string> GetContent(int version, string protoName)
    {
        var result = GetPath(version, protoName);
        return string.IsNullOrEmpty(result) ? string.Empty : await File.ReadAllTextAsync(result);
    }

    public string GetPath(int version, string protoName)
    {
        var filePath = $"{_contentRootPath}/Protos/v{version}/{protoName}";
        var doesExist = File.Exists(filePath);
        return doesExist ? filePath : string.Empty;
    }
}

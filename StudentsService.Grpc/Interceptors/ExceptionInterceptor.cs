using Grpc.Core;
using Grpc.Core.Interceptors;

namespace StudentsService.Grpc.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try { return await continuation(requestStream, context); }
        catch (Exception ex)
        {
            var trailers = ExceptionHandle(ex);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message), trailers, "Serverside exception message...");
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try { await continuation(requestStream, responseStream, context); }
        catch (Exception ex)
        {
            var trailers = ExceptionHandle(ex);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message), trailers, "Serverside exception message...");
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try { await continuation(request, responseStream, context); }
        catch (Exception ex)
        {
            var trailers = ExceptionHandle(ex);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message), trailers, "Serverside exception message...");
        }
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try { return await continuation(request, context); }
        catch (Exception ex)
        {
            var trailers = ExceptionHandle(ex);
            throw new RpcException(new Status(StatusCode.Internal, ex.Message), trailers, "Serverside exception message...");
        }
    }

    private static Metadata ExceptionHandle(Exception ex)
    {
        var correlationId = Guid.NewGuid();
        Metadata trailers = new()
        {
            { "CorrelationId", correlationId.ToString() },
            { "Interceptor", "True" }
        };
        return trailers;
    }
}

//using Common.Extensions;
//using System.Net;
//using System.Net.Mime;

//namespace API.Infrastructure;

//internal sealed class ExceptionAndLoggerMiddleware
//{
//    #region Properties
//    private static int BodyLengthLimitInBytes => 42000;

//    private static string DefaultBody => "{\"Message\": \"Large content\",\"Length\": 0}";

//    private RequestDelegate Next { get; }

//    private ILogger Logger { get; }

//    private RecyclableMemoryStreamManager MemoryStreamManager { get; }

//    private static HashSet<string> LogMethods { get; } = new()
//    {
//        "POST",
//        "PUT",
//        "PATCH",
//        "DELETE",
//#if DEBUG
//        "GET",
//#endif
//    };
//    #endregion

//    public ExceptionAndLoggerMiddleware(RequestDelegate next, ILogger<ExceptionAndLoggerMiddleware> logger)
//    {
//        Next = next;
//        Logger = logger;
//        MemoryStreamManager = new RecyclableMemoryStreamManager();
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            if (LogMethods.Contains(context.Request.Method))
//            {
//                var id = RandomString.UpperAlpha(5);
//                await LogRequest(context, id);
//                await LogResponse(context, id);
//            }
//            else
//                await Next(context);
//        }
//        catch (InnerException inner)
//        {
//            context.Response.ContentType = MediaTypeNames.Application.Json;
//            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//            var dto = new BadRequestDto
//            {
//                Errors = inner.Errors,
//                DevCode = inner.Message.ExceptionCode(),
//            };
//            await context.Response.WriteAsync(dto.ToJson());
//            Logger.LogWarning(inner.Message);
//        }
//        catch (Exception unknown)
//        {
//            context.Response.ContentType = MediaTypeNames.Application.Json;
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            var model = new BadRequestDto
//            {
//                Errors = new Dictionary<string, IEnumerable<string>>
//                {
//                    {"Alert", new[] {unknown.JoinInnerExceptions(), }},
//                },
//            };
//            await context.Response.WriteAsync(model.ToJson());
//            Logger.LogError(unknown.JoinInnerExceptions());
//        }
//    }

//    private async Task LogRequest(HttpContext context, string id)
//    {
//        var request = new LogRequestDto
//        {
//            Method = context.Request.Method,
//            Path = context.Request.Path,
//            Query = context.Request.QueryString.ToString(),
//            UserAgent = context.Request.Headers["User-Agent"],
//            Authorization = context.Request.Headers[nameof(LogRequestDto.Authorization)],
//            Culture = context.Request.Headers[nameof(LogRequestDto.Culture)],
//            BodyJson = await RequestBodyJson(context),
//        };

//        Logger.LogInformation($"_{nameof(request)}({id}): {request.ToJson()}");
//    }

//    private async Task<string> RequestBodyJson(HttpContext context)
//    {
//        context.Request.EnableBuffering();
//        if (context.Request.ContentLength > BodyLengthLimitInBytes)
//            return DefaultBody.Replace(0.ToString(), context.Request.ContentLength.ToString());

//        await using var requestStream = MemoryStreamManager.GetStream();
//        await context.Request.Body.CopyToAsync(requestStream);
//        context.Request.Body.Position = 0;

//        return ReadStreamInChunks(requestStream);
//    }

//    private static string ReadStreamInChunks(Stream stream)
//    {
//        const int readChunkBufferLength = 4096;

//        stream.Seek(0, SeekOrigin.Begin);

//        using var textWriter = new StringWriter();
//        using var reader = new StreamReader(stream);

//        var readChunk = new char[readChunkBufferLength];
//        int readChunkLength;

//        do
//        {
//            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
//            textWriter.Write(readChunk, 0, readChunkLength);
//        } while (readChunkLength > 0);

//        return textWriter.ToString();
//    }

//    private async Task LogResponse(HttpContext context, string id)
//    {
//        var origin = context.Response.Body;
//        await using var responseBody = MemoryStreamManager.GetStream();
//        context.Response.Body = responseBody;

//        try
//        {
//            await Next(context);
            
//            var response = new LogResponseDto
//            {
//                StatusCode = context.Response.StatusCode,
//                UserName = context.User.Identity?.Name,
//                BodyJson = await ResponseBodyJson(context),
//            };
//            Logger.LogInformation($"_{nameof(response)}({id}): {response.ToJson()}");
//            await responseBody.CopyToAsync(origin);
//        }
//        finally
//        {
//            context.Response.Body = origin;
//        }
//    }

//    private async Task<string> ResponseBodyJson(HttpContext context)
//    {
//        context.Response.Body.Seek(0, SeekOrigin.Begin);
//        if (context.Response.Body.Length > BodyLengthLimitInBytes)
//            return DefaultBody.Replace(0.ToString(), context.Response.Body.Length.ToString());

//        var bodyJson = await new StreamReader(context.Response.Body).ReadToEndAsync();
//        context.Response.Body.Seek(0, SeekOrigin.Begin);

//        return bodyJson;
//    }
//}

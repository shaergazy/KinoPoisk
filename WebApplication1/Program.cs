using API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
builder.Configure().Run();

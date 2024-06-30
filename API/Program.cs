using API.Extensions;
using Common.CommonServices;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
builder.Configure().Run();

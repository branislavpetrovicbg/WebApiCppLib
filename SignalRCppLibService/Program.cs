using SignalRCppLibService;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel web server
//builder.WebHost.UseKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxConcurrentConnections = 1;
//    serverOptions.Limits.MaxConcurrentUpgradedConnections = 1;
//});

// Add services to the container.

////builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumParallelInvocationsPerClient = 1;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<CppLibHub>("/CppLibHub");
});

app.Run();

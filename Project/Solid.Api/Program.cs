var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ativo para o stor abrir a documentação da API
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
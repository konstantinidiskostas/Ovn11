using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Extensions;
using MovieApi.Interfaces;
using MovieApi.Services;
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MovieApiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MovieApiContext") ??
                      throw new InvalidOperationException("Connection string 'MovieApiContext' not found.")));

builder.Services.AddScoped<IMovieApiDbContext, MovieApiContext>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
    {
        // Ενεργοποιεί το API Versioning και επιτρέπει στο .NET να διαχειρίζεται εκδόσεις.

        // Αν ένας πελάτης καλέσει το API χωρίς να γράψει έκδοση, ο server δεν θα βγάλει σφάλμα.
        options.AssumeDefaultVersionWhenUnspecified = true;

        // Ορίζει ότι η βασική/προεπιλεγμένη έκδοση του API μας είναι η 1.0.
        options.DefaultApiVersion = new ApiVersion(1, 0);

        options.ReportApiVersions = true;
    })
// ΑΥΤΟ ΕΔΩ ΕΙΝΑΙ ΤΟ ΚΛΕΙΔΙ: Αντικαθιστά αυτόματα το {version} στο Swagger!
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Ορίζει το Bearer scheme (το κουμπί "Authorize" στο Swagger UI)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Εισάγετε το JWT token σας εδώ"
    });

    // 2. Λέει στο Swagger να στέλνει το token σε κάθε request (νέο syntax v10)
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Products API",
        Version = "v1"
    });

    // Δημιουργεί ένα δεύτερο ξεχωριστό έγγραφο στο Swagger για την έκδοση v2
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Products API",
        Version = "v2"
    });
    // Βρίσκει το όνομα του αρχείου XML που παράγεται αυτόματα
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
    
    // Καθορίζει την πλήρη διαδρομή (path) όπου είναι αποθηκευμένο το αρχείο
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    // Λέει στο Swagger να συμπεριλάβει τα XML σχόλια στο UI
    options.IncludeXmlComments(xmlPath); 
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Έλεγξε ποιος εξέδωσε το token
            ValidateAudience = true, // Έλεγξε για ποιον προορίζεται
            ValidateLifetime = true, // Έλεγξε αν έχει λήξει
            ValidateIssuerSigningKey = true, // Έλεγξε την υπογραφή με το secret key
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddAuthorization();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Δημιουργεί την επιλογή "Products API v1" στο drop-down μενού και τη συνδέει με το αρχείο της v1
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1");

        // Δημιουργεί την επιλογή "Products API v2" στο drop-down μενού και τη συνδέει με το αρχείο της v2
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Products API v2");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.SeedData();
app.Run();
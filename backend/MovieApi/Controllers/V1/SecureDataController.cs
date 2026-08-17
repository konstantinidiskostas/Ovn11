using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize] // Απαιτεί ο χρήστης να έχει ένα έγκυρο JWT
public class SecureDataController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        // Ανάκτηση του Username από τα Claims του Token
        var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        return Ok(new { Message = $"Grattis {userName}, du har nått en skyddad endpoint!" });
    }
}
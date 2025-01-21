using Microsoft.AspNetCore.Mvc;

namespace OHMS_Service.Controllers;

[Route("HIA3/REST")]
[ApiController]
public class TempAAController : Controller {

    [HttpGet("Authenticate")]
    public async Task<IActionResult> Authenticate(string username, string password) {
        if (username.Contains("@cegep-heritage.qc.ca") && password != null) {
            if (username == "teacher@cegep-heritage.qc.ca") {
                string token =
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InRlYWNoZXJAY2VnZXAtaGVyaXRhZ2UucWMuY2EiLCJwYXNzd29yZCI6InBhc3N3b3JkIn0.LW2nPX16pwLbNp0K2UwOzOSa40YAUdCbzddEg-xvIcc";
                return Ok(new {Token = token});
            }

            if (username == "student@cegep-heritage.qc.ca") {
                string token =
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0dWRlbnRAY2VnZXAtaGVyaXRhZ2UucWMuY2EiLCJwYXNzd29yZCI6InBhc3N3b3JkIn0.wpFs8YQGiDnXjrOJtm-ddmMZ28E3SWmscWZtMfvhGus";
                return Ok(new {Token = token});
            }
        }
        return Unauthorized("Invalid username or password");
    }
    
    [HttpGet("Authorize")]
    public async Task<IActionResult> Authorize(string token, string appCode) {
        string teacherToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InRlYWNoZXJAY2VnZXAtaGVyaXRhZ2UucWMuY2EiLCJwYXNzd29yZCI6InBhc3N3b3JkIn0.LW2nPX16pwLbNp0K2UwOzOSa40YAUdCbzddEg-xvIcc";
        string studentToken = 
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6InN0dWRlbnRAY2VnZXAtaGVyaXRhZ2UucWMuY2EiLCJwYXNzd29yZCI6InBhc3N3b3JkIn0.wpFs8YQGiDnXjrOJtm-ddmMZ28E3SWmscWZtMfvhGus";

        if (token == teacherToken && appCode == "STARS") {
            return Ok(new List<string> { "Teacher" });
        }
        
        if (token == studentToken && appCode == "STARS") {
            return Ok(new List<string> { "Student" });
        }

        return Unauthorized("Invalid Credentials");
    }
}
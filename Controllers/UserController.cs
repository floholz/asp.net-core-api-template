using asp.net_core_api_template.Models.Responses;
using asp.net_core_api_template.Services;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_core_api_template.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly UserService _service;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService service, ILogger<UserController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet, Route("getUser")]
    public ActionResult<UserResponse> GetUser()
    {
        try
        {
            return Ok(_service.GetUsers());
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
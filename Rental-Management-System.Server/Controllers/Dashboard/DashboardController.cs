using Microsoft.AspNetCore.Mvc;
using Rental_Management_System.Server.Services.DashboardService;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardData()
    {
        var response = await _dashboardService.GetDashboardDataAsync();
        if (response == null) return NotFound();
        return response.Success ? Ok(response) : BadRequest(response);
    }
}

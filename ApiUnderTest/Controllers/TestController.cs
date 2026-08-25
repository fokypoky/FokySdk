using FokySdk.Controller;
using FokySdk.Types.Common;
using Microsoft.AspNetCore.Mvc;

namespace ApiUnderTest.Controllers;

public class TestDto
{
    public DateTime Date { get; set; }
}

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = ServiceResult<TestDto>.Ok(new TestDto() { Date = DateTime.UtcNow });
        return this.MapResponse(res);
    }

    [HttpGet("/test/test")]
    public async Task<IActionResult> Get2()
    {
        var res = ServiceResult<PaginatedResponse<TestDto>>.PartialContent(new PaginatedResponse<TestDto>()
        {
            Data = [new TestDto() { Date = DateTime.UtcNow }, new TestDto() { Date = DateTime.UtcNow.AddDays(-7) }],
            TotalCount = 56
        });

        return this.MapResponse(res);
    }
}
using Microsoft.AspNetCore.Mvc;
using Domain.Dtos;
using Domain.ServiceInterfaces;

namespace Api.Controllers;

/// <summary>
/// 問い合わせコントローラー
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ContactController(IContactService contactService) : ControllerBase
{
    [HttpPost("inquiry")]
    public async Task<IActionResult> SendInquiry([FromBody] ContactRequestDto request, CancellationToken cancellationToken)
    {
        var result = await contactService.SendContactInquiryAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessages);
    }
}

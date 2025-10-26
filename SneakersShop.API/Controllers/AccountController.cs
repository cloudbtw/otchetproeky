using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakersShop.Core.Interfaces;
using System.Security.Claims;

namespace SneakersShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpPost("topup")]
    public async Task<ActionResult> TopUpBalance([FromBody] decimal amount)
    {
        if (amount <= 0)
            return BadRequest("Сумма должна быть положительной");

        var user = await _userRepository.GetByIdAsync(GetUserId());
        if (user == null)
            return NotFound();

        user.Balance += amount;
        await _userRepository.UpdateAsync(user);

        return Ok(new { Balance = user.Balance });
    }
}
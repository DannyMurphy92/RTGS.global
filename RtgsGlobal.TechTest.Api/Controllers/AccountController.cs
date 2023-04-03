using Microsoft.AspNetCore.Mvc;
using RtgsGlobal.TechTest.Api.Services.Interfaces;

namespace RtgsGlobal.TechTest.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
	private readonly IAccountProvider _accountProvider;
	private readonly ITransferService _transferService;

	public AccountController(IAccountProvider accountProvider, ITransferService transferService)
	{
		_accountProvider = accountProvider;
		_transferService = transferService;
	}

	[HttpPost("{accountIdentifier}", Name = "Deposit")]
	public IActionResult Deposit(string accountIdentifier, [FromBody] decimal amount)
	{
		_accountProvider.Deposit(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("{accountIdentifier}/withdraw", Name = "Withdrawal")]
	public IActionResult Withdraw(string accountIdentifier, [FromBody] decimal amount)
	{
		_accountProvider.Withdraw(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("transfer", Name = "Transfer")]
	public IActionResult Transfer(AccountTransferDto transfer)
	{
		_transferService.Transfer(transfer);
		return Accepted();
	}

	[HttpGet("{accountIdentifier}", Name = "GetBalance")]
	public AccountBalance Get(string accountIdentifier) => _accountProvider.GetBalance(accountIdentifier);
}



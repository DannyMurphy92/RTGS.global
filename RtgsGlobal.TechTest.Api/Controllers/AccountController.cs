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
		if (!DoesAccountExist(accountIdentifier))
		{
			return NotFound($"Account {accountIdentifier} not found");
		}

		if (amount < 0)
		{
			return BadRequest("Cannot deposit negative amount");
		}

		_accountProvider.Deposit(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("{accountIdentifier}/withdraw", Name = "Withdrawal")]
	public IActionResult Withdraw(string accountIdentifier, [FromBody] decimal amount)
	{
		if (!DoesAccountExist(accountIdentifier))
		{
			return NotFound($"Account {accountIdentifier} not found");
		}
		_accountProvider.Withdraw(accountIdentifier, amount);
		return Ok();
	}

	[HttpPost("transfer", Name = "Transfer")]
	public IActionResult Transfer(AccountTransferDto transfer)
	{
		if (!DoesAccountExist(transfer.CreditorAccountIdentifier))
		{
			return NotFound($"Account {transfer.CreditorAccountIdentifier} not found");
		}

		if (!DoesAccountExist(transfer.DebtorAccountIdentifier))
		{
			return NotFound($"Account {transfer.DebtorAccountIdentifier} not found");
		}

		if (transfer.CreditorAccountIdentifier == transfer.DebtorAccountIdentifier)
		{
			return BadRequest("Cannot transfer between the same account");
		}

		_transferService.Transfer(transfer);
		return Accepted();
	}

	[HttpGet("{accountIdentifier}", Name = "GetBalance")]
	public IActionResult Get(string accountIdentifier)
	{
		if (!DoesAccountExist(accountIdentifier))
		{
			return NotFound($"Account {accountIdentifier} not found");
		}
		var balance = _accountProvider.GetBalance(accountIdentifier);
		return Ok(balance);
	}

	private bool DoesAccountExist(string accountIdentifier) => _accountProvider.AccountExists(accountIdentifier);
}



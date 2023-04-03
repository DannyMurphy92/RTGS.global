using Microsoft.AspNetCore.Mvc;
using RtgsGlobal.TechTest.Api.Models.Exceptions;
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
		try
		{
			_accountProvider.Deposit(accountIdentifier, amount);
			return Ok();
		}
		catch (AccountNotFoundException ex)
		{
			return NotFound($"Account {ex._accountIdentifier} not found");
		}
		catch (InvalidDepositException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("{accountIdentifier}/withdraw", Name = "Withdrawal")]
	public IActionResult Withdraw(string accountIdentifier, [FromBody] decimal amount)
	{
		try
		{
			_accountProvider.Withdraw(accountIdentifier, amount);
			return Ok();
		}
		catch (AccountNotFoundException ex)
		{
			return NotFound($"Account {ex._accountIdentifier} not found");
		}
	}

	[HttpPost("transfer", Name = "Transfer")]
	public IActionResult Transfer(AccountTransferDto transfer)
	{
		try
		{
			_transferService.Transfer(transfer);
			return Accepted();
		}
		catch (AccountNotFoundException ex)
		{
			return NotFound($"Account {ex._accountIdentifier} not found");
		}
		catch (InvalidTransferException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpGet("{accountIdentifier}", Name = "GetBalance")]
	public IActionResult Get(string accountIdentifier)
	{
		try
		{
			var balance = _accountProvider.GetBalance(accountIdentifier);
			return Ok(balance);
		}
		catch (AccountNotFoundException ex)
		{
			return NotFound($"Account {ex._accountIdentifier} not found");
		}
	}

	private bool DoesAccountExist(string accountIdentifier) => _accountProvider.AccountExists(accountIdentifier);
}



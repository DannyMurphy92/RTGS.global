using RtgsGlobal.TechTest.Api.Models.Exceptions;
using RtgsGlobal.TechTest.Api.Services.Interfaces;

namespace RtgsGlobal.TechTest.Api.Services;

public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, AccountBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, AccountBalance> { { "account-a", new AccountBalance() }, { "account-b", new AccountBalance() } };
	}

	public AccountBalance GetBalance(string accountIdentifier)
	{
		if (!AccountExists(accountIdentifier))
		{
			throw new AccountNotFoundException(accountIdentifier);
		}
		return _accounts[accountIdentifier];
	}

	public void Deposit(string accountIdentifier, decimal amount)
	{
		if (amount < 0)
		{
			throw new InvalidDepositException("Cannot deposit negative amount");
		}

		AddTransaction(accountIdentifier, amount);
	}

	public void Withdraw(string accountIdentifier, decimal amount) => AddTransaction(accountIdentifier, -1 * amount);

	private void AddTransaction(string accountIdentifier, decimal amount)
	{

		if (!AccountExists(accountIdentifier))
		{
			throw new AccountNotFoundException(accountIdentifier);
		}
		AccountBalance accountBalance = _accounts[accountIdentifier];
		_accounts[accountIdentifier] =
			accountBalance with { Balance = accountBalance.Balance + amount };
	}

	public bool AccountExists(string accountIdentifier)
	{
		return _accounts.ContainsKey(accountIdentifier);
	}
}


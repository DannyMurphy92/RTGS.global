using RtgsGlobal.TechTest.Api.Services.Interfaces;

namespace RtgsGlobal.TechTest.Api.Services;

public class AccountProvider : IAccountProvider
{
	private readonly IDictionary<string, AccountBalance> _accounts;

	public AccountProvider()
	{
		_accounts = new Dictionary<string, AccountBalance> { { "account-a", new AccountBalance() }, { "account-b", new AccountBalance() } };
	}

	public AccountBalance GetBalance(string accountIdentifier) => _accounts[accountIdentifier];

	public void Deposit(string accountIdentifier, decimal amount) => AddTransaction(accountIdentifier, amount);



	public void Withdraw(string accountIdentifier, decimal amount) => AddTransaction(accountIdentifier, -1 * amount);

	private void AddTransaction(string accountIdentifier, decimal amount)
	{
		AccountBalance accountBalance = _accounts[accountIdentifier];
		_accounts[accountIdentifier] =
			accountBalance with { Balance = accountBalance.Balance + amount };
	}
}


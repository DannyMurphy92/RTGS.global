namespace RtgsGlobal.TechTest.Api.Services.Interfaces;

public interface IAccountProvider
{
	AccountBalance GetBalance(string accountIdentifier);
	void Deposit(string accountIdentifier, decimal amount);

	void Withdraw(string accountIdentifier, decimal amount);
	bool AccountExists(string accountIdentifier);
}

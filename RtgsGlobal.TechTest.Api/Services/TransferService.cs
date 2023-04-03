
using RtgsGlobal.TechTest.Api.Services.Interfaces;

namespace RtgsGlobal.TechTest.Api.Services;

public class TransferService : ITransferService
{
	private readonly IAccountProvider _accountProvider;

	public TransferService(IAccountProvider accountProvider)
	{
		_accountProvider = accountProvider;
	}
	public void Transfer(AccountTransferDto transfer)
	{
		_accountProvider.Withdraw(transfer.DebtorAccountIdentifier, transfer.Amount);
		_accountProvider.Deposit(transfer.CreditorAccountIdentifier, transfer.Amount);
	}
}

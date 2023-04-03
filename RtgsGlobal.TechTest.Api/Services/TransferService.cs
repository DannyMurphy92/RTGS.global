
using RtgsGlobal.TechTest.Api.Models.Exceptions;
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

		if (!_accountProvider.AccountExists(transfer.DebtorAccountIdentifier))
		{
			throw new AccountNotFoundException(transfer.DebtorAccountIdentifier);
		}


		if (!_accountProvider.AccountExists(transfer.CreditorAccountIdentifier))
		{
			throw new AccountNotFoundException(transfer.CreditorAccountIdentifier);
		}

		if (transfer.CreditorAccountIdentifier == transfer.DebtorAccountIdentifier)
		{
			throw new InvalidTransferException("Cannot transfer between the same account");
		}

		_accountProvider.Withdraw(transfer.DebtorAccountIdentifier, transfer.Amount);
		_accountProvider.Deposit(transfer.CreditorAccountIdentifier, transfer.Amount);
	}
}

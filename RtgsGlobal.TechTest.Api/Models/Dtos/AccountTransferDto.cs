public class AccountTransferDto
{
	public AccountTransferDto(string debtorAccountIdentifier, string creditorAccountIdentifier, decimal amount)
	{
		DebtorAccountIdentifier = debtorAccountIdentifier;
		CreditorAccountIdentifier = creditorAccountIdentifier;
		Amount = amount;
	}

	public string DebtorAccountIdentifier { get; set; }
	public string CreditorAccountIdentifier { get; set; }
	public decimal Amount { get; set; }
}

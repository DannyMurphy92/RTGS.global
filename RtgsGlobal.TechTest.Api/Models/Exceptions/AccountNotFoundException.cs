using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtgsGlobal.TechTest.Api.Models.Exceptions;

public class AccountNotFoundException : Exception
{
	public readonly string _accountIdentifier;
	public AccountNotFoundException(string accountIdentifier)
	{
		_accountIdentifier = accountIdentifier;
	}
}


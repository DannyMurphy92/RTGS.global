using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtgsGlobal.TechTest.Api.Models.Exceptions;

public class InvalidDepositException : Exception
{
	public InvalidDepositException(string message) : base(message) { }
}


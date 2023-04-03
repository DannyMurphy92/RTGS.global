using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RtgsGlobal.TechTest.Api.Models.Exceptions;

public class InvalidTransferException : Exception
{
	public InvalidTransferException(string message) : base(message) { }
}


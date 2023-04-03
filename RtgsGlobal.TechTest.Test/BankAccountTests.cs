using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RtgsGlobal.TechTest.Api;
using RtgsGlobal.TechTest.Api.Controllers;
using Xunit;

namespace RtgsGlobal.TechTest.Test;

public class BankAccountTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client;

	public BankAccountTests(WebApplicationFactory<Program> fixture)
	{
		_client = fixture
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.AddRtgsServices();
			}))
			.CreateDefaultClient();
	}

	[Fact]
	public async Task GivenAccountExistsWithNoTransactions_ThenGetBalanceShouldReturnZero()
	{
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(0, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenDepositIsAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(1000, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExistsAndDepositIsAdded_WhenWithdrawalIsAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");

		await _client.PostAsJsonAsync("/account/account-a/withdraw", "100");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(900, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenMultipleDepositsAreAdded_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/account-a", "1000");
		await _client.PostAsJsonAsync("/account/account-a", "2000");
		var result = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");

		Assert.Equal(3000, result.Balance);
	}

	[Fact]
	public async Task GivenAccountExists_WhenTransferIsMade_ThenGetBalanceShouldReturnExpected()
	{
		await _client.PostAsJsonAsync("/account/transfer", new AccountTransferDto("account-a", "account-b", 1000));
		var accountA = await _client.GetFromJsonAsync<AccountBalance>("/account/account-a");
		var accountB = await _client.GetFromJsonAsync<AccountBalance>("/account/account-b");

		Assert.Equal(-1000, accountA.Balance);
		Assert.Equal(1000, accountB.Balance);
	}

	[Fact]
	public async Task GivenAccountDoesNotExistWithNoTransactions_ThenReturns404()
	{
		var result = await _client.GetAsync("/account/not-an-account");
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_WhenDepositIsAdded_ThenReturn404()
	{
		var result = await _client.PostAsJsonAsync("/account/not-an-account", "1000");
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountDoesNotExist_WhenWithdrawalIsAdded_ThenReturn404()
	{
		var result = await _client.PostAsJsonAsync("/account/not-an-account/withdraw", "100");
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Theory]
	[InlineData("account-a", "not-an-account")]
	[InlineData("not-an-account", "account-a")]
	public async Task GivenAccountDoesNotExist_WhenTransferIsMade_Return404(string debtorAccountIdentified, string creditorAccountIdentified)
	{
		var result = await _client.PostAsJsonAsync("/account/transfer", new AccountTransferDto(debtorAccountIdentified, creditorAccountIdentified, 1000));
		Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountExists_WhenNegativeDepositIsAdded_ThenGetBadRequestShouldBeReturned()
	{
		var result = await _client.PostAsJsonAsync("/account/account-a", "-1000");
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
	}

	[Fact]
	public async Task GivenAccountExists_WhenTransferIsMadeToAndFromSameAccount_ThenReturnBadRequest()
	{
		var result = await _client.PostAsJsonAsync("/account/transfer", new AccountTransferDto("account-a", "account-a", 1000));
		Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
	}

}


using RtgsGlobal.TechTest.Api.Services;
using RtgsGlobal.TechTest.Api.Services.Interfaces;


namespace RtgsGlobal.TechTest.Api;
public static class StartupExtensions
{
	public static IServiceCollection AddRtgsServices(this IServiceCollection services)
	{
		services.AddSingleton<IAccountProvider, AccountProvider>();
		services.AddScoped<ITransferService, TransferService>();

		return services;
	}
}

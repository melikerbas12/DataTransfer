using System;
using System.Threading.Tasks;
using DataTransfer.Console.Service.Process;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataTransfer.Console
{
    class Program
    {
        public static Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            System.Console.WriteLine("Console Calculator in C#\r");
            System.Console.WriteLine("--------------------------\n");

            // Ask the user to choose an option.
            System.Console.WriteLine("Choose an option from the following list:");
            System.Console.WriteLine("\tl  - Localization");
            System.Console.WriteLine("\tll - LocalizationLanguage");
            System.Console.WriteLine("\taa - AdminAuthority");
            System.Console.Write("Your option? ");
            
            var menu = System.Console.ReadLine();
            switch (menu)
            {
                case "l":
                    #region Localization
                    var localizationService = LocalizationWithDependencyInjection(host.Services);
                    var languageResponse = localizationService.GetList().Objects;
                    localizationService.Insert(languageResponse);
                    #endregion
                    break;
                case "ll":
                    #region LocalizationLanguage
                    var localizationLanguageService = LocalizationLanguageWithDependencyInjection(host.Services);
                    var localizationLanguageResponse = localizationLanguageService.GetList().Objects;
                    localizationLanguageService.Insert(localizationLanguageResponse);
                    #endregion
                    break;
                case "aa":

                    #region AdminAuthority
                    var adminAuthorityService = AdminAuthorityWithDependencyInjection(host.Services);
                    var adminAuthorityResponse = adminAuthorityService.GetList().Objects;
                    adminAuthorityService.Insert(adminAuthorityResponse);
                    #endregion
                    break;
            }
            // Wait for the user to respond before closing.
            System.Console.Write("Press any key to close the Calculator console app...");
            System.Console.ReadKey(true);

            return host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddTransient<ILocalizationService, LocalizationService>()
                        .AddTransient<ILocalizationLanguageService, LocalizationLanguageService>()
                          .AddTransient<IAdminAuthorityService, AdminAuthorityService>()
                           .AddTransient<IAdminAuthorityLanguageService, AdminAuthorityLanguageService>());

        }
        public static ILocalizationService LocalizationWithDependencyInjection(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var localizationService = provider.GetRequiredService<ILocalizationService>();

            return localizationService;
        }
        public static ILocalizationLanguageService LocalizationLanguageWithDependencyInjection(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var localizationLanguageService = provider.GetRequiredService<ILocalizationLanguageService>();

            return localizationLanguageService;
        }
        public static IAdminAuthorityService AdminAuthorityWithDependencyInjection(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var adminAuthorityService = provider.GetRequiredService<IAdminAuthorityService>();

            return adminAuthorityService;
        }
        public static IAdminAuthorityLanguageService AdminAuthorityLanguageWithDependencyInjection(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var adminAuthorityLanguageService = provider.GetRequiredService<IAdminAuthorityLanguageService>();

            return adminAuthorityLanguageService;
        }

    }
}
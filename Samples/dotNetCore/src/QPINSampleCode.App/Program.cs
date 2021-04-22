using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using QPINSampleCode.UIHandler;
using QPINSampleCode.Service;
using System.IO;

namespace QPINSampleCode.App
{
    class Program
    {        
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            IServiceCollection services = new ServiceCollection();
            CredentialModel credential = new CredentialModel();
            config.GetSection("Credential").Bind(credential);
			// make sure CustomerCode, Username and Password are in the appsettings.json
            if ( string.IsNullOrWhiteSpace(credential.CustomerCode) 
                || string.IsNullOrWhiteSpace(credential.Username)
                || string.IsNullOrWhiteSpace(credential.Password) 
                )
            {
                Console.WriteLine("Please check CustomerCode, Username and Password in appsettings.json");
                Console.ReadLine();
                Environment.Exit(0);
            }
            services.AddSingleton(credential);
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IUIFactory, UIFactory>();
            services.AddSingleton<IQPINClient, QPINClient>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var uiFactory = serviceProvider.GetService<IUIFactory>();
            while (true)
            {
                try
                {
                    Console.WriteLine("Please Select an option or service operation");
                    Console.WriteLine("Enter Option number (1: Generate PINOffset, 2: Verify PINOffset)");
                    var keyInfo = Console.ReadKey();
                    Console.WriteLine();

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            uiFactory.ShowUI(UI.GENERATEPINOFFSET);
                            break;
                        case ConsoleKey.D2:
                            uiFactory.ShowUI(UI.VERIFYPINOFFSET);
                            break;
                    }
                    bool decision = Confirm("Would you like to Continue with other Request");
                    if (decision)
                        continue;
                    else
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static bool Confirm(string title)
        {
            ConsoleKey response;
            do
            {
                Console.Write($"{ title } [y/n] ");
                response = Console.ReadKey(false).Key;
                if (response != ConsoleKey.Enter)
                {
                    Console.WriteLine();
                }
            } while (response != ConsoleKey.Y && response != ConsoleKey.N);

            return (response == ConsoleKey.Y);
        }
    }
}

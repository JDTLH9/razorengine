using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using razorengine.Models;
using RazorEngine;
using RazorEngine.Templating;

namespace razorengine
{
    internal class Program
    {
        private static int Main()
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                // ReSharper disable once ObjectCreationAsStatement
                new AppDomainSetup
                {
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
                };
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
                return exitCode;
            }

            var template = Template.Get("MyView.cshtml");
            var persons = new []
            {
                new Person {Age = 21, Forename = "Ken", Surname = "Tobin"},
                new Person {Age = 19, Forename = "Sharon", Surname = "Testshire"}
            };
            Console.Write(Engine.Razor.RunCompile(template, "key", typeof(Name), new Name {Persons = persons}));
            Console.ReadLine();
            return 0;
        }
    }
}

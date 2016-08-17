using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
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
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
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

            Console.Write(Engine.Razor.RunCompile(template, "key", typeof(Name), new Name()));
            Console.ReadLine();
            return 0;
        }
    }
}

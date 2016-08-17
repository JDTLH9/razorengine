using System;
using System.IO;

namespace razorengine
{
    public static class Template
    {
        public static string Get(string templateName)
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Views", templateName));
        }
    }
}

using AspNetCore.Introduction.Interfaces;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Introduction.Models
{
    public class Derived : Base
    {
        public Derived(ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<Derived>())
        {
        }

        public void SomeMethod()
        {
            Logger.LogInformation("Some text");
        }
    }
}
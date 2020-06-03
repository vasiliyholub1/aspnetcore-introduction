using Microsoft.Extensions.Logging;

namespace AspNetCore.Introduction.Interfaces
{
    public abstract class Base
    {
        protected readonly ILogger Logger;

        protected Base(ILogger logger)
        {
            Logger = logger;
        }
    }
}
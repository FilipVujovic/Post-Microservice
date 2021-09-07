using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Logger
{
    public class LoggerRepository<T> : ILoggerRepository<T>
    {

        private readonly ILogger<T> logger;

        public LoggerRepository(ILogger<T> logger)
        {
            this.logger = logger;
        }
        public void LogException(Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, message, args);
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
        }
    }
}

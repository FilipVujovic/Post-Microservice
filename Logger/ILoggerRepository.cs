using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Logger
{
    public interface ILoggerRepository<T>
    {
        public void LogException(Exception ex, string message, params object[] args);

        public void LogInformation(string message);
    }
}

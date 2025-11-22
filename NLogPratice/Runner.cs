using Microsoft.Extensions.Logging;

namespace NLogPratice
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            this._logger = logger;
        }

        public void DoAction(string name)
        {
            this._logger.LogInformation("UpdateItem", "Doing hard work! {Action}", name);
        }
    }
}
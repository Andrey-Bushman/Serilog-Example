namespace WorkerService1 {
    public class Worker : BackgroundService {
        private readonly IHostEnvironment _hostEnv;
        private readonly ILogger<Worker> _logger;

        public Worker(IHostEnvironment hostEnv, ILogger<Worker> logger) {
            _hostEnv = hostEnv;
            _logger = logger;
        }

        /// <summary>
        /// Сформировать дополнительный набор свойств, который хотим
        /// добавлять ко всем сообщениям лога в рамках Scope, в котором
        /// этот набор будет использоваться.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, object> CreateScopeInformation() {

            var scopeInfo = new Dictionary<string, object> {
                { "MachineName", Environment.MachineName },
                { "UserName", Environment.UserName },
                { "Environment", _hostEnv.EnvironmentName },
                { "AppName", "Logging Scopes" },
            };

            return scopeInfo;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {

            // Формируем область, в рамках которой каждому отправляемому сообщению лога будут присваиваться
            // дополнительные свойства из объекта, переданного в качестве параметра методу BeginScope().
            using (_logger.BeginScope(CreateScopeInformation())) {

                while (!stoppingToken.IsCancellationRequested) {

                    await DoWork(_logger);
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private static async Task DoWork(ILogger logger) {

            logger.LogInformation(" -> Воркер запущен в: {time}", DateTimeOffset.Now);
            logger.LogWarning(" -> Некоторое тестовое предупреждение...");
            logger.LogError(new Exception("Шишкин лес"), " -> Некоторое тестовое исключение...");
        }
    }
}

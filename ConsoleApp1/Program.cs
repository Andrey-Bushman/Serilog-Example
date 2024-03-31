using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1 {
    internal class Program {

        /// <summary>
        /// Сформировать дополнительный набор свойств, который хотим
        /// добавлять ко всем сообщениям лога в рамках Scope, в котором
        /// этот набор будет использоваться.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, object> CreateScopeInformation() {

            var scopeInfo = new Dictionary<string, object> {
                { "MachineName", Environment.MachineName },
                { "UserName", Environment.UserName },
                { "AppName", "Logging Scopes" },
            };

            return scopeInfo;
        }

        static void Main(string[] args) {

            try {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serilog.config.json")
                .Build();

                // Сначала получаем реализацию логгера от Serilog...
                using (var serilogLogger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger())
                // На основе полученного логгера Serilog формируем фабрику логгеров, которая будет возвращать экземпляры
                // интерфейса Microsoft.Extensions.Logging.ILoggerFactory.
                using (ILoggerFactory loggerFactory = new SerilogLoggerFactory(serilogLogger)) {

                    // Наконец, получаем нужный нам экземпляр Microsoft.Extensions.Logging.ILogger:
                    Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger<Program>();

                    // Формируем область, в рамках которой каждому отправляемому сообщению лога будут присваиваться
                    // дополнительные свойства из объекта, переданного в качестве параметра методу BeginScope().
                    using (logger.BeginScope(CreateScopeInformation())) {

                        DoWork(logger);
                    }
                }
            }
            catch (Exception ex) {
                throw;
            }
        }

        private static void DoWork(Microsoft.Extensions.Logging.ILogger logger) {

            logger.LogInformation("Hello, World!");
            logger.LogWarning("Warning, World!");
            logger.LogError(new Exception(), "Error, World!");
        }
    }
}

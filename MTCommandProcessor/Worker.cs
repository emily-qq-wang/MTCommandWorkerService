// <copyright file="Worker.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using MTCommandProcessor.Service;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="Worker" />.
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Defines the _logger.
        /// </summary>
        private readonly ILogger<Worker> _logger;
        private readonly ICommandProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{Worker}"/>.</param>
        public Worker(ILogger<Worker> logger, ICommandProcessor processor)
        {
            _logger = logger;
            this.processor = processor;
        }

        /// <summary>
        /// The StartAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            processor.StartProcessor();
            return base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// The StopAsync.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The service has started stopping procedures...");
            await processor.StopProcessor();
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// The ExecuteAsync.
        /// </summary>
        /// <param name="stoppingToken">The stoppingToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await processor.Process();
                _logger.LogInformation("Processing  Complete");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

using Hangfire;
using HangfireService.Services;

namespace HangfireService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _emailService;

        public Worker(ILogger<Worker> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                RecurringJob.AddOrUpdate(() => _emailService.SendEmail(), "0 35 7 ? * MON-FRI");
            }
        }
    }
}
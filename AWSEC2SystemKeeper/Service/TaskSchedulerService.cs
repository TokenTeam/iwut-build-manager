namespace AWSEC2SystemKeeper.Service
{
    public class TaskSchedulerService(ILogger<TaskSchedulerService> logger)
    {
        private Timer? _timer;
        DateTime _lastCallTime = DateTime.MinValue;
        TimeSpan _delay = TimeSpan.Zero;
        private readonly ILogger _logger = logger;

        public void SetTimerTime(Action task, TimeSpan delay)
        {
            if (_lastCallTime == DateTime.MinValue)
                _delay = delay;
            else
                _delay = _lastCallTime + _delay + delay - DateTime.Now;
            _lastCallTime = DateTime.Now;
            _logger.LogInformation("New Task! Stop Time set to {StopTime}", _lastCallTime + _delay);
            _timer?.Dispose();
            _timer = new Timer(
                _ =>
                {
                    _timer?.Dispose();
                    _timer = null;
                    task.Invoke();
                },
                null,
                _delay,
                Timeout.InfiniteTimeSpan
            );
        }
    }
}

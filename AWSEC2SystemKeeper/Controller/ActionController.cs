using AWSEC2SystemKeeper.Service;
using Microsoft.AspNetCore.Mvc;

namespace AWSEC2SystemKeeper.Controller
{
    [Route("Action")]
    public class ActionController(TaskSchedulerService taskSchedulerService, AWSService awsService)
        : ControllerBase
    {
        private readonly TaskSchedulerService _taskSchedulerService = taskSchedulerService;
        private readonly AWSService _awsService = awsService;
        private bool _isAvailable = true;
        private static readonly HashSet<string> _tasks = [];
        private static readonly object _availableLock = new();

        [HttpGet(nameof(SetAction))]
        public IActionResult SetAction(string task, int expireTime = 300)
        {
            lock (_availableLock)
            {
                if (!_isAvailable)
                    return BadRequest("Not available!");
                if (_tasks.Contains(task))
                    return BadRequest("Task already exist!");
                _tasks.Add(task);
                _taskSchedulerService.SetTimerTime(
                    async () =>
                    {
                        lock (_availableLock)
                            _isAvailable = false;
                        await _awsService.StopEC2InstanceAsync();
                    },
                    TimeSpan.FromSeconds(expireTime)
                );
                return Ok();
            }
        }

        [HttpGet(nameof(StopAction))]
        public IActionResult StopAction(string task)
        {
            lock (_availableLock)
            {
                if (!_tasks.Contains(task))
                    return BadRequest("Task not exist.");
                _tasks.Remove(task);
                if (_tasks.Count == 0)
                {
                    _isAvailable = false;
                    Task.Run(_awsService.StopEC2InstanceAsync);
                }
                return Ok();
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows;
using Translumo.Infrastructure.Dispatching;

namespace Translumo.Services
{
    public class InteractionActionDispatcher : IActionDispatcher
    {
        private readonly ConcurrentDictionary<string, object> _consumers = new ConcurrentDictionary<string, object>();

        public void RegisterConsumer<TArgument, TResult>(string actionName, Func<TArgument, Task<TResult>> actionHandler)
        {
            if (_consumers.ContainsKey(actionName))
            {
                throw new InvalidOperationException($"Action consumer with key '{actionName}' is already registered");
            }

            _consumers[actionName] = actionHandler;
        }

        public async Task<TResult> DispatchActionAsync<TArgument, TResult>(string actionName, TArgument argument)
        {
            if (!_consumers.ContainsKey(actionName))
            {
                throw new ArgumentException("Consumer key mismatch", nameof(actionName));
            }

            var targetFunc = _consumers[actionName] as Func<TArgument, Task<TResult>>;
            if (targetFunc == null)
            {
                throw new ArgumentException("Consumer action signature mismatch", nameof(argument));
            }

            Func<Task<TResult>> action = () =>
            {
                return targetFunc.Invoke(argument);
            };

            var operation = await Application.Current.Dispatcher.Invoke(action);

            return operation;
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Translumo.Utils.Extensions
{
    public static class WaitHandleExtensions
    {
        public static Task WaitOneAsync(this WaitHandle waitHandle, CancellationToken cancellationToken, int timeoutMilliseconds = Timeout.Infinite)
        {
            if (waitHandle == null)
                throw new ArgumentNullException(nameof(waitHandle));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            CancellationTokenRegistration ctr = cancellationToken.Register(() => tcs.TrySetCanceled());
            TimeSpan timeout = timeoutMilliseconds > Timeout.Infinite ? TimeSpan.FromMilliseconds(timeoutMilliseconds) : Timeout.InfiniteTimeSpan;

            RegisteredWaitHandle rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle,
                (_, timedOut) =>
                {
                    if (timedOut)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult(true);
                    }
                },
                null, timeout, true);

            Task<bool> task = tcs.Task;

            _ = task.ContinueWith(_ =>
            {
                rwh.Unregister(null);
                return ctr.Unregister();
            }, CancellationToken.None);

            return task;
        }
    }
}

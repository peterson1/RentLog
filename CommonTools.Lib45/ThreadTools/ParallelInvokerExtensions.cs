using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonTools.Lib45.ThreadTools
{
    public static class ParallelInvokerExtensions
    {
        private static ConcurrentBag<bool> _successBag;
        private static ConcurrentBag<bool> _failBag;

        public static Action AsParallelJob(this List<Action> origJobs,
            Action<int, int, int> progressHandler = null)
        {
            var newJobs = origJobs.Select(_ 
                        => WrapExecution(_, progressHandler, origJobs.Count)).ToList();

            _successBag = new ConcurrentBag<bool>();
            _failBag    = new ConcurrentBag<bool>();

            return () => Parallel.Invoke(newJobs.ToArray());
        }


        private static Action WrapExecution(Action origJob, Action<int, int, int> progressHandler, int totalJobsCount) => () =>
        {
            try
            {
                origJob.Invoke();
                _successBag.Add(true);
                //progressHandler.Invoke()
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Parallel Jobs");
                _failBag.Add(false);
            }
            progressHandler?.Invoke(_successBag.Count, 
                        _failBag.Count, totalJobsCount);
        };
    }
}

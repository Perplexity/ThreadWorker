using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadWorker
{
    public class ThreadWorker<T>
    {
        private List<Thread> WorkerThreads { get; }

        public ThreadWorker(Action<T> job, List<T> collection, int threads)
        {
            WorkerThreads = new List<Thread>();
            if (threads > collection.Count)
            {
                threads = collection.Count;
            }
            var remainder = collection.Count % threads;
            var itemsPerThread = (collection.Count - remainder) / threads;
            var splitCollection = collection.Split(itemsPerThread);
            foreach (var coll in splitCollection)
            {
                var thread = new Thread(() =>
                {
                    foreach (var item in coll)
                    {
                        job.Invoke(item);
                    }
                });
                WorkerThreads.Add(thread);
            }
        }

        public void Start()
        {
            foreach (var thread in WorkerThreads)
            {
                thread.Start();
            }
        }
    }
}

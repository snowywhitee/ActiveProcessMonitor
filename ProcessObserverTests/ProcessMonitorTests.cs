using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ProcessObserver;
using Xunit;

namespace ProcessObserverUnitTests
{
    public class ProcessMonitorTests
    {
        private readonly ProcessMonitor _processMonitor = new ProcessMonitor();
        
        [Fact]
        public void UpdateSimpleTest()
        {
            _processMonitor.Update();
            var processes = Process.GetProcesses();
            var set1 = new HashSet<int>();
            foreach (var process in processes)
            {
                set1.Add(process.Id);
            }
            //wait for a change
            Thread.Sleep(5000);
            _processMonitor.Update();
            var newProcesses = Process.GetProcesses();
            var set2 = new HashSet<int>();
            foreach (var process in newProcesses)
            {
                set2.Add(process.Id);
            }
            int delta = 0;
            foreach (var id in set1)
            {
                if (!set2.Contains(id))
                {
                    delta++;
                }
            }

            foreach (var id in set2)
            {
                if (!set1.Contains(id))
                {
                    delta++;
                }
            }
            Assert.Equal(delta, _processMonitor.UpdatedProcesses.Count);
        }
        
        [Fact]
        public void UpdateComplexTest()
        {
            _processMonitor.Update();
            var processes = Process.GetProcesses();
            var set1 = new HashSet<int>();
            foreach (var process in processes)
            {
                set1.Add(process.Id);
            }
            //wait for a change
            Thread.Sleep(5000);
            _processMonitor.Update();
            var newProcesses = Process.GetProcesses();
            var set2 = new HashSet<int>();
            foreach (var process in newProcesses)
            {
                set2.Add(process.Id);
            }

            var set3 = new HashSet<int>();
            foreach (var id in set1)
            {
                if (!set2.Contains(id))
                {
                    set3.Add(id);
                }
            }

            foreach (var id in set2)
            {
                if (!set1.Contains(id))
                {
                    set3.Add(id);
                }
            }

            Assert.All(_processMonitor.UpdatedProcesses, x => set3.Contains(x.Id));
        }
    }
}
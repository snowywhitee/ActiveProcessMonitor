using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ActiveProcessMonitor.Models;

namespace ProcessObserver
{
    public class ProcessMonitor
    {
        private Dictionary<int, Process> _processes = new Dictionary<int, Process>();
        private Dictionary<int, Process> _newProcesses = new Dictionary<int, Process>();
        private Dictionary<int, Process> _inactiveProcesses = new Dictionary<int, Process>();
        private List<CustomProcess> _updatedProcesses = new List<CustomProcess>();

        public List<CustomProcess> UpdatedProcesses { get => _updatedProcesses; }

        public void Update()
        {
            GetNewProcesses();
            GetInactiveProcesses();
            foreach (var process in _newProcesses)
            {
                _processes.Add(process.Key, process.Value);
            }
            foreach (var process in _inactiveProcesses)
            {
                _processes.Remove(process.Key);
            }
            MakeCollection();
        }
        private void MakeCollection()
        {
            _updatedProcesses.Clear();
            foreach (var process in _newProcesses)
            {
                _updatedProcesses.Add(new CustomProcess(process.Value) {New = true});
            }
            foreach (var process in _inactiveProcesses)
            {
                _updatedProcesses.Add(new CustomProcess(process.Value) {New = false});
            }
        }
        private void GetInactiveProcesses()
        {
            _inactiveProcesses.Clear();
            var processes = GetProcesses();
            foreach (var process in _processes)
            {
                if (!processes.ContainsKey(process.Key))
                {
                    _inactiveProcesses.Add(process.Key, process.Value);
                }
            }
        }

        private void GetNewProcesses()
        {
            _newProcesses.Clear();
            foreach (var process in GetProcesses())
            {
                if (!_processes.ContainsKey(process.Key))
                {
                    _newProcesses.Add(process.Key, process.Value);
                }
            }
        }
        private Dictionary<int, Process> GetProcesses()
        {
            return Process.GetProcesses().ToDictionary(p => p.Id);
        }
    }
}
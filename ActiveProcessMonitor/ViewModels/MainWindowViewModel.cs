using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Timers;
using ActiveProcessMonitor.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace ActiveProcessMonitor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Timer _timer = new Timer(2000);
        
        //InfoSource indexes values:
        //0 - all processes count (sys as well)
        //1 - sys processes count
        //2 - thread count
        public static ObservableCollectionExtended<int> InfoSource { get; } =
            new ObservableCollectionExtended<int>();
        private ReadOnlyObservableCollection<int> _totalInfo;
        public ReadOnlyObservableCollection<int> TotalInfo => _totalInfo;
        
        
        public static ObservableCollectionExtended<CustomProcess> Source { get; } =
            new ObservableCollectionExtended<CustomProcess>();
        private ReadOnlyObservableCollection<CustomProcess> _processes;
        public ReadOnlyObservableCollection<CustomProcess> Processes => _processes;

        public MainWindowViewModel()
        {
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += Update;
            Source.ToObservableChangeSet().ObserveOn(RxApp.MainThreadScheduler).Bind(out _processes).Subscribe();
            InfoSource.ToObservableChangeSet().ObserveOn(RxApp.MainThreadScheduler).Bind(out _totalInfo).Subscribe();
            for (int i = 0; i < 3; i++)
            {
                InfoSource.Add(0);
            }
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            PipeClient.Receive();
            var receivedProcesses = PipeClient.Processes;
            foreach (var process in receivedProcesses)
            {
                InfoSource[2] += process.ThreadCount;
                //Those are system processes (like with PID 0), built-in activity monitors do not
                //show them, so we won't as well. Although, we will include them in total count;
                
                if (string.IsNullOrEmpty(process.Name))
                {
                    if (process.New)
                    {
                        InfoSource[1]++;
                    }
                    else
                    {
                        InfoSource[1]--;
                    }
                    continue;
                }
                

                if (process.New)
                {
                    Source.Add(process);
                }
                else
                {
                    Source.Remove(process);
                }
            }

            InfoSource[0] = Source.Count + InfoSource[1];
        }
    }
}
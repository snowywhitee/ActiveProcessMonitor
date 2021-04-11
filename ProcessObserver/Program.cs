using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Timers;
using System.Xml.Serialization;
using ActiveProcessMonitor.Models;
using Timer = System.Timers.Timer;

namespace ProcessObserver
{
    public static class PipeServer
    {
        public static NamedPipeServerStream _pipe = new NamedPipeServerStream("ProcessMonitor");
        private static XmlSerializer _serializer = new XmlSerializer(typeof(List<CustomProcess>));
        private static Timer _timer = new Timer(2000);
        private static ProcessMonitor _processMonitor = new ProcessMonitor();
        
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            _pipe.WaitForConnection();
            _timer.AutoReset = true;
            _timer.Elapsed += Send;
            _timer.Enabled = true;
            while (true)
            {
                Thread.Sleep(2000);
            }
        }

        private static void Send(object sender, ElapsedEventArgs e)
        {
            _processMonitor.Update();
            StreamWriter streamWriter = new StreamWriter(_pipe);
            streamWriter.WriteLine(ToXml());
            streamWriter.Flush();
            _pipe.WaitForPipeDrain();
        }

        private static string ToXml()
        {
            StringWriter stringWriter = new StringWriter();
            _serializer.Serialize(stringWriter, _processMonitor.UpdatedProcesses);
            string xml = stringWriter.ToString().Replace(Environment.NewLine, " ");
            stringWriter.Close();
            return xml;
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            _pipe.Close();
        }
    }
}
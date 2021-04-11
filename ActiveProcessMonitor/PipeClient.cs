using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Xml.Serialization;
using ActiveProcessMonitor.Models;

namespace ActiveProcessMonitor
{
    public static class PipeClient
    {
        private static List<CustomProcess> _processes = new List<CustomProcess>();
        private static NamedPipeClientStream _pipe = new NamedPipeClientStream("ProcessMonitor");
        private static XmlSerializer _serializer = new XmlSerializer(typeof(List<CustomProcess>));

        public static List<CustomProcess> Processes { get => _processes; }

        public static void Run()
        {
            _pipe.Connect();
        }

        public static void Receive()
        {
            StreamReader streamReader = new StreamReader(_pipe);
            string? xmlData = streamReader.ReadLine();
            _processes = FromXml(xmlData);
        }

        private static List<CustomProcess> FromXml(string? xmlData)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                return new List<CustomProcess>();
            }

            StringReader stringReader = new StringReader(xmlData);
            if (!(_serializer.Deserialize(stringReader) is List<CustomProcess> receivedProcesses))
            {
                receivedProcesses = new List<CustomProcess>();
            }
            stringReader.Close();

            return receivedProcesses;
        }
    }
}
namespace ActiveProcessMonitor.Models
{
    public class CustomProcess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ThreadCount { get; set; }
        public bool New { get; set; }

        public CustomProcess()
        {
        }

        public CustomProcess(System.Diagnostics.Process process)
        {
            Id = process.Id;
            Name = process.ProcessName;
            ThreadCount = process.Threads.Count;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CustomProcess)
            {
                return false;
            }
            return Id == ((CustomProcess)obj).Id;
        }
    }
}
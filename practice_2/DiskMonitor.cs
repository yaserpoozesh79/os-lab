using System;
using System.Management;
using System.Diagnostics;

namespace App{
    public class DiskMonitor: IDisposable
    {
        private ManagementEventWatcher _watcher;
        private int eventCounter = 0;

        private Process? startedProcess;

        public DiskMonitor()
        {
            SetupWatcher();
        }

        private void SetupWatcher()
        {
            var query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent");
            _watcher = new ManagementEventWatcher(query);
            _watcher.EventArrived += Watcher_EventArrived;
            _watcher.Start();
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            try
            {
                Console.Write($"{++this.eventCounter}-Event arrived!: ");
                var eventType = (ushort)e.NewEvent.Properties["EventType"].Value;
                switch (eventType)
                {
                    case 2: // Arrival
                        Console.WriteLine("Disk inserted");
                        startedProcess = Process.Start("mspaint");
                        break;
                    case 3: // Removal
                        Console.WriteLine("Disk removed");
                        if(startedProcess != null)
                            startedProcess.Kill();
                        break;
                    default:
                        Console.WriteLine($"Unknown event type: {eventType}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling disk event: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _watcher.Stop();
            _watcher.Dispose();
        }
    }
}
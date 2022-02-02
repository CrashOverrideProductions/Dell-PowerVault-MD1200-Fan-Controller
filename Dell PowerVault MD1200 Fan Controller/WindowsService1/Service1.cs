
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Timers;

namespace FanController
{
    public partial class Service1 : ServiceBase
    {
        // Service Status Controls
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        // Setup Program Variables
        Timer WorkTimer = new Timer();
        bool IsWorkCycleInProgress = false;
        Controller control = new Controller();


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Configure the Work Timer - 2 Seconds.
            WorkTimer.Interval = (2 * 1000); // 60 Seconds
            WorkTimer.Elapsed += WorkTimer_Elapsed;
            WorkTimer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            try
            {
                // Enable Manual Fan Control
               // control.startControl();

                // Add to Log
                EventLog.WriteEntry("Dell PowerEdge Fan Controller", "Service Started", EventLogEntryType.Information); // "App Name", "Message"
            }
            catch (Exception ex)
            {
                string errorMsg = "Error Starting Service" + Environment.NewLine + ex.Message;
                EventLog.WriteEntry("Dell PowerEdge Fan Controller", errorMsg, EventLogEntryType.Error); // "App Name", "Message"
                OnStop();
            }
        }

        protected override void OnStop()
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Stop the Work Timer
            WorkTimer.Stop();

            // Add to Event Log
            EventLog.WriteEntry("Dell PowerEdge Fan Controller", "Stopping Service", EventLogEntryType.Information); // "App Name", "Message"

            // Enable Automatic Fan Control
           // control.endControl();

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        private void WorkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Ensure no other work is being completed.
                if (!IsWorkCycleInProgress)
                {
                    // Set work cycle is in progress.
                    IsWorkCycleInProgress = true;

                    // Start Conducting Work.
                    control.controlMain();

                    // Add to Event Log
                    IsWorkCycleInProgress = false;
                }
            }
            catch (Exception ex)
            {
                IsWorkCycleInProgress = false;
                string errorMsg = "Error Running Service" + Environment.NewLine + ex.Message;
                EventLog.WriteEntry("Dell PowerEdge Fan Controller", errorMsg, EventLogEntryType.Error); // "App Name", "Message"
                OnStop();
            }
        }

        // Service States
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;

namespace Wakett.Rates.Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        private readonly IServiceManager _serviceManager;

        public Service1(IServiceManager serviceManager)
        {
            InitializeComponent();
            _serviceManager = serviceManager;
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(ExecuteTasks, null, 0, Timeout.Infinite);
        }

        protected override void OnStop()
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
        }

        private void ExecuteTasks(object state)
        {
            _serviceManager.ExecuteTasks();
            _timer.Change(60000, Timeout.Infinite);
        }
    }
}

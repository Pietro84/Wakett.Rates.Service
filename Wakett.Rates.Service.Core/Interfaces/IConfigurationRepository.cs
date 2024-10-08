﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Core.Interfaces
{
    public interface IConfigurationRepository
    {
        TaskConfiguration GetTaskConfiguration();
        void UpdateLastRunTime(int taskId, DateTime lastRunTime);

    }
}

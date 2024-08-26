using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wakett.Rates.Service.Core.Models
{
    public class TaskConfiguration
    {
        public int TaskId { get; set; }
        public int TickTime { get; set; }
        public DateTime? LastRunTime { get; set; }
    }
}

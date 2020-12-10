using System;
using Newtonsoft.Json;

namespace DashboardApi.Models
{
    public class CovidCaseTotal
    {
        public int ConfirmedTotal { get; set; }
        public int DeathsTotal { get; set; }
        public int RecoveredTotal { get; set; }
    }
}
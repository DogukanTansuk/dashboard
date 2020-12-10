using System;
using Newtonsoft.Json;

namespace DashboardApi.Models
{
    public class CovidCase
    {
        public string CountryRegion { get; set; }
        public DateTime EntryDate { get; set; }
        public int ConfirmedToday { get; set; }
        public int DeathsToday { get; set; }
        public int RecoveredToday { get; set; }
        public int ConfirmedChange { get; set; }
        public int DeathsChange { get; set; }
        public int RecoveredChange { get; set; }
    }
}
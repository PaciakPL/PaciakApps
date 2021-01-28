using System;

namespace DAL.Entities
{
    public class Event
    {
        public string User { get; set; }
        public DateTime? Date { get; set; }
        public int Steps { get; set; }
        public int LastValue { get; set; }
        public int NewValue { get; set; }
        public bool Ok { get; set; }
        public int Diff { get; set; }
    }
}
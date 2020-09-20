using System;

namespace ClassAid.Models.Schedule
{
    /// <summary>
    /// Core scheduling model.
    /// Contains subject name, teacher, start and end time, and course code
    /// field.
    /// </summary>
    public class ScheduleModel
    {
        public string Subject { get; set; }
        public Teacher Teacher { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CourseCode { get; set; }
    }
}

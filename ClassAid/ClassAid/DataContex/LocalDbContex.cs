using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Dapper;
using ClassAid.Models.Schedule;
using System.IO;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ClassAid.Models.Users;
using Xamarin.Essentials;

namespace ClassAid.DataContex
{
    public class LocalDbContex : DbContext
    {
        public DbSet<ScheduleModel> schedules;
        public DbSet<Teacher> teachers;
        public DbSet<EventModel> events;
        public DbSet<Student> students;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "classaiddb.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }
        public LocalDbContex()
        {
            this.Database.EnsureCreated();
        }
        public IEnumerable<ScheduleModel> GetSchedules()
        {
            using(var contex = new LocalDbContex())
            {
                return schedules;
            }
        }
        public void AddSchedule(ScheduleModel schedule)
        {
            schedules.Add(schedule);
        }
        public void AddTeacher(Teacher teacher)
        {
            teachers.Add(teacher);
        }
        private static string DatabaseFilename = "classaid.db";
        private static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine("Data Source="+basePath, DatabaseFilename);
            }
        }
        public static void SaveSchedules(ScheduleModel schedules)
        {
            using (IDbConnection cnn = new SqliteConnection(DatabasePath))
            {
                var d = cnn.Execute("INSERT INTO \'SCHEDULE\' (Subject, Teacher, StartTime, EndTime, CourseCode, DayOfWeek) VALUES " +
                    "(@Subject, @Teacher, @StartTime, @EndTime, @CourseCode, @DayOfWeek)", schedules);
                Debug.WriteLine(d);
            }
        }
        public static IEnumerable<ScheduleModel> LoadSchedules()
        {
            Debug.WriteLine(DatabasePath);
            using (IDbConnection cnn = new SqliteConnection(DatabasePath))
            {
                var schedules = cnn.Query<ScheduleModel>("SELECT * FROM SCHEDULE");
                //Debug.WriteLine(schedules.ToList()[0].CourseCode);
                Debug.WriteLine("LOL");
                return schedules;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Dapper;
using ClassAid.Models.Schedule;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using ClassAid.Models.Users;

namespace ClassAid.DataContex
{
    public class LocalDbContex
    {
        private static string ConectionString
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine("Data Source=" + basePath + "/classaid.db3");
            }
        }
        public static void SaveSchedules(ScheduleModel schedules)
        {
            using (IDbConnection db = new SqliteConnection(ConectionString))
            {              
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Data", JsonConvert.SerializeObject(schedules));
                db.Execute("INSERT OR REPLACE INTO schedule (Data) Values (@Data);", parameters);
            }
        }
        public static void SaveBatchDetails(BatchDetails batchDetails)
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {               
                cnn.Execute("INSERT OR REPLACE INTO batchdetails (Department,Semester,Section,University) " +
                    "Values (@Department,@Semester,@Section,@University);", batchDetails);
            }
        }
        public static void SaveTeacher(Teacher teacher)
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                cnn.ExecuteAsync("INSERT OR REPLACE INTO teachers (Name, Designation, Phone) " +
                    "Values (@Name, @Designation, @Phone);", teacher);
            }
        }
        public static void SaveEvents(EventModel eventModel)
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                cnn.ExecuteAsync("INSERT OR REPLACE INTO events (Details, Title, Time) " +
                    "Values (@Details, @Title, @Time);", eventModel);
            }
        }
        public static void SaveUser<T>(T user)
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                cnn.ExecuteAsync(@"INSERT OR REPLACE INTO user (TeamCode, AdminKey, Name,
                            ID, Phone, Key, IsActive, IsAdmin) " +
                    "Values (@TeamCode, @AdminKey, @Name," +
                    "@ID, @Phone, @Key, @IsActive, @IsAdmin);", user);
            }
        }
        public static IEnumerable<Teacher> GetTeachers()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                var teacher = cnn.Query<Teacher>("SELECT * FROM teachers;");
                return teacher;
            }
        }
        public static IEnumerable<EventModel> GetEvents()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                var events = cnn.Query<EventModel>("SELECT * FROM events;");
                return events;
            }
        }
        public static BatchDetails GetBatchDetails()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                BatchDetails batch = cnn.Query<BatchDetails>("SELECT * FROM schedule")
                    .FirstOrDefault();
                return batch;
            }
        }
        public static IEnumerable<ScheduleModel> GetSchedules()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                var schedules = cnn.Query<string>("SELECT * FROM schedule").Select(p =>
                JsonConvert.DeserializeObject<ScheduleModel>(p));
                return schedules;
            }
        }
        public static Student GetUser()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                var user = cnn.Query<Student>("SELECT * FROM user").FirstOrDefault();
                return user;
            }
        }
        public static void CreateTables()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                string createTable = @"CREATE TABLE IF NOT EXISTS batchdetails (
                            Department text,Semester text,Section text,University text);";
                cnn.ExecuteAsync(createTable);
                createTable = @"CREATE TABLE IF NOT EXISTS schedule (
                            Data text);";
                cnn.ExecuteAsync(createTable);
                createTable = @"CREATE TABLE IF NOT EXISTS events (
                            Details text, Title text, Time text)";
                cnn.ExecuteAsync(createTable);
                createTable = @"CREATE TABLE IF NOT EXISTS teachers (
                            Name text, Designation text, Phone text)";
                cnn.ExecuteAsync(createTable);
                createTable = @"CREATE TABLE IF NOT EXISTS user (
                            TeamCode text, AdminKey text, Name text,
                            ID text, Phone text, Key text, IsActive boolean, 
                            IsAdmin boolean)";
                cnn.Execute(createTable);
            }
        }
        public static void DropTables()
        {
            using (IDbConnection cnn = new SqliteConnection(ConectionString))
            {
                string sql = "DROP TABLE IF EXISTS batchdetails;";
                cnn.ExecuteAsync(sql);
                sql = "DROP TABLE IF EXISTS schedule;";
                cnn.ExecuteAsync(sql);
                sql = "DROP TABLE IF EXISTS events;";
                cnn.ExecuteAsync(sql);
                sql = "DROP TABLE IF EXISTS teachers;";
                cnn.ExecuteAsync(sql);
                sql = "DROP TABLE IF EXISTS user;";
                cnn.ExecuteAsync(sql);
            }
        }
    }
}

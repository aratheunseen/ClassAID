using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ClassAidUniversal.Users;
using System.Data;
using Dapper;
using System.Data.SQLite;

namespace ClassAidBackend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthAPI : ControllerBase
    {
        // GET: api/<AdminAuthAPI>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AdminAuthAPI>/5
        [HttpGet("{key}")]
        public string Get(int key)
        {
            return "value";
        }

        // POST api/<AdminAuthAPI>
        [HttpPost]
        public string Post([FromBody] Admin admin)
        {
            string AdminDB = $"Data Source={Program.rootpath}/Data/class_aid_database.db; Version=3";

            try
            {
                using (IDbConnection cnn = new SQLiteConnection(AdminDB))
                {
                    cnn.Execute("INSERT INTO Admin (Username,Password,Email,Phone,AdminKey) VALUES (@Username,@Password,@Email,@Phone,@AdminKey)", admin);
                    return "{'Error':'False','Key':" + admin.Key + "'}";
                }
            }
            catch (Exception)
            {
                return "{'Error':'True','Message':'User already exists. Try using different Email or username.'}";
            }
        }

        // PUT api/<AdminAuthAPI>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<AdminAuthAPI>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

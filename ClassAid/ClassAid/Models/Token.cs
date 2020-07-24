using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models
{
    class Token
    {
        public int ID { get; set; }
        public string AccessToken { get; set; }
        public string ErrorDesc { get; set; }
        public DateTime ExpireDate { get; set; }
        public Token()
        {

        }
    }
}

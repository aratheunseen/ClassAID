using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassAidUniversal.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ClassAidBackend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static string path;
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //path = Program.rootpath;
        }
        public void OnPostRegestration()
        {
            Admin admin = new Admin(Username, Password);
            path = admin.Key;
        }
    }
}

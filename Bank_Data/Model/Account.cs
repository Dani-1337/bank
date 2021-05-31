using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Model
{
    public class Account : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Balance Balance_ { get; set; }
        public User User_ { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Model
{
    public class User : BaseModel
    {
        public enum Gender
		{
            Male = 0,
            Female = 1
		}

        public int Age { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Gender Gender_ { get; set; }
        public Address Address_ { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Model
{
	public class Balance : BaseModel
    {
        public double MainBalance { get; set; }
        public List<History> Histories { get; set; }
        public List<Credit> Credits { get; set; }
    }
}

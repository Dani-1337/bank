using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Model
{
    public class Credit : BaseModel
    {
        public double CreditTaken { get; set; }
        public double CreditLeft { get; set; }
        public DateTime CreateDate { get; set; }
        public List<History> Histories { get; set; }
    }
}

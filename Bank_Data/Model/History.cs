using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Model
{
    public class History : BaseModel
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public Account To { get; set; }
        public Account From { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_Data.Context;
using Bank_Data.Model;

namespace Bank_Business
{
    class AddressBusiness
    {
        AddressContext context;

        public List<Address> GetAll()
        {
            using (context = new AddressContext())
            {
                return context.addresses.ToList();
            }
        }
    }
}

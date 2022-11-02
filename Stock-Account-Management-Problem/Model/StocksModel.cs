using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Account_Management_Problem.Model
{
    public class StocksModel
    {
        public List<CommonProperties> Stocks { get; set; }
    }
    public class CommonProperties
    {
        public string CompanyName { get; set; }
        public int NoOfShares { get; set; }
        public int SharePrice { get; set; }
    }
}

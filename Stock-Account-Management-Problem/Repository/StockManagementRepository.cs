using Newtonsoft.Json;
using Stock_Account_Management_Problem.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Account_Management_Problem.Repository
{
    public class StockManagementRepository
    {
        public String JsonFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\StocksData.json";

        public void StockPortfolio()
        {
            var JsonFileData = File.ReadAllText(JsonFilePath);

            StocksModel StocksData = JsonConvert.DeserializeObject<StocksModel>(JsonFileData);

            int ValueofTotalStocks = 0;

            foreach (var Stocks in StocksData.Stocks)
            {
                
                Console.Write("Company Name : " + Stocks.CompanyName + "\n" +
                "Number of Stocks : " + Stocks.NoOfShares + "\n" +
                "Price Of One Share : Rs." + Stocks.SharePrice + "\n");

                Console.WriteLine($"The Value of Stocks of {Stocks.CompanyName} is : Rs.{Stocks.SharePrice * Stocks.NoOfShares} \n");

                ValueofTotalStocks += (Stocks.NoOfShares * Stocks.SharePrice); 
            }
            Console.Write("The Value of Total Stocks is : Rs." +ValueofTotalStocks);
            Console.WriteLine("Json Data : \n" + JsonFileData);
        }
    }
}

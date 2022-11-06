using Newtonsoft.Json;
using Stock_Account_Management_Problem.Model;

namespace Stock_Account_Management_Problem.Repository
{
    public class StockManagementRepository
    {
        public string JsonFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\StocksData.json";
         
        public void StockPortfolio()
        {
            string JsonFileData = File.ReadAllText(JsonFilePath);
            StocksModel StocksData = JsonConvert.DeserializeObject<StocksModel>(JsonFileData);

            Console.WriteLine("******************** Company Details ********************");

            foreach (var Stocks in StocksData.Stocks)
            {               
                Console.Write("Company Name : " + Stocks.CompanyName +
                "\nNumber of Stocks : " + Stocks.NoOfShares +
                "\nPrice Of One Share : Rs." + Stocks.SharePrice +
                "\nStock Symbol : " + Stocks.StockSymbol + "\n");
                
                Console.WriteLine("---------------------------------------------");
            }
            
        }
        public void StockPortfolio1(string CompnayName)
        {
            bool CompanyExist = false;
            string JsonFileData = File.ReadAllText(JsonFilePath);
            StocksModel StocksData = JsonConvert.DeserializeObject<StocksModel>(JsonFileData);
            foreach (var Stocks in StocksData.Stocks)
            {
                if (Stocks.CompanyName == CompnayName)
                {
                    CompanyExist = true;
                    Console.Write("Company Name : " + Stocks.CompanyName +
                    "\nNumber of Stocks : " + Stocks.NoOfShares +
                    "\nPrice Of One Share : Rs." + Stocks.SharePrice + 
                    "\nStock Symbol : " + Stocks.StockSymbol + "\n\n");
                    break;
                }
            }
            if (!CompanyExist) 
            {
                Console.WriteLine($"{CompnayName} Doesn't Exist in Company List");
            }

        }
    }
}

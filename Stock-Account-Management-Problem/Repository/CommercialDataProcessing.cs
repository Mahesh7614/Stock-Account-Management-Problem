using Newtonsoft.Json;
using Stock_Account_Management_Problem.Model;


namespace Stock_Account_Management_Problem.Repository
{
    public class CommercialDataProcessing
    {
        public static string CustomerInfoFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\CustomerInfo.json";
        public static string JsonFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\StocksData.json";
        List<StockAccount> stockAccounts = new List<StockAccount>();
        StocksModel StocksData = new StocksModel();

        public void StockAccounts()
        {
            string Accounts = File.ReadAllText(CustomerInfoFilePath);
            stockAccounts = JsonConvert.DeserializeObject<List<StockAccount>>(Accounts);
        }
        public void CompanyAccount()
        {
            string JsonFileData = File.ReadAllText(JsonFilePath);
            StocksData = JsonConvert.DeserializeObject<StocksModel>(JsonFileData);
        }
        public void SaveComapny()
        {
            string StockDetail1 = JsonConvert.SerializeObject(StocksData);
            File.WriteAllText(JsonFilePath, StockDetail1);
        }
        public void SaveCustomer()
        {
            string StockDetail = JsonConvert.SerializeObject(stockAccounts);
            File.WriteAllText(CustomerInfoFilePath, StockDetail);
        }
        public void BuyStocks(string CustomerName, string CompanyName, int shares)
        {
            double total = 0;
            string StockCompanyName = " ";
            string StockSymbol = " ";
            double StockSharePrices = 0;
            bool CompanyExist = false;
            bool CustomerExist = false;

            CompanyAccount();
            StockAccounts();
            foreach (var item in StocksData.Stocks)
            {
                if (item.CompanyName == CompanyName)
                {
                    CompanyExist = true;
                    foreach (StockAccount Customeritem in stockAccounts)
                    {
                        if (Customeritem.CustomerInfo.Name == CustomerName)
                        {
                            if (item.NoOfShares >= shares)
                            {
                                item.NoOfShares -= shares;
                                total = shares * item.SharePrice;
                                StockCompanyName = item.CompanyName;
                                StockSharePrices = item.SharePrice;
                                StockSymbol = item.StockSymbol;
                            }
                            else
                            {
                                Console.WriteLine($"Stocks Not Available at {CompanyName}");
                            }
                            break;
                        }
                    }
                }
            }
            if (!CompanyExist)
            {
                Console.WriteLine($"{CompanyName} Dosen't Exist in Company List");

            }
            SaveComapny();

            foreach (StockAccount items in stockAccounts)
            {
                if (items.CustomerInfo.Name == CustomerName)
                {
                    CustomerExist = true;
                    if (items.CustomerInfo.Balance >= total)
                    {
                        items.CustomerInfo.Balance -= total;
                        if (CompanyName == StockCompanyName)
                        {
                            bool Exit = false;
                            foreach (ShareDetail shareitem in items.ShareDetail)
                            {
                                if (shareitem.CompanyName == CompanyName)
                                {
                                    Exit = true;
                                    shareitem.NoOfShares += shares;
                                    break;
                                }
                            }
                            if (!Exit)
                            {
                                items.ShareDetail.Add(new ShareDetail() { CompanyName = StockCompanyName, NoOfShares = shares, SharePrice = StockSharePrices, StockSymbol = StockSymbol });

                            }
                            Console.WriteLine($"\n<<<<<<<<<<<<<<< {CustomerName} have SucessFully Purchased {shares} Stocks of {StockCompanyName} ({StockSymbol}) for Price Rs.{StockSharePrices} each stock at {DateTime.Now} >>>>>>>>>>>>>>>>");
                            Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Total Remaining Balance of {CustomerName} ({StockSymbol}) is Rs.{items.CustomerInfo.Balance} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Insufficient Balance in {CustomerName} Account ");
                    }
                    break;
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine($"{CustomerName} Doesn't Exits in Customer List");

            }

            SaveCustomer();
        }
        public void SellStocks(string CustomerName, string CompanyName, int shares)
        {
            double StockSharePrices = 0;
            string StockSymbol = " ";
            bool CompanyExist = false;
            bool CustomerExist = false;
            bool CompanyExistAtCustomer = false;

            StockAccounts();
            CompanyAccount();
            foreach (StockAccount items in stockAccounts)
            {
                if (items.CustomerInfo.Name == CustomerName)
                {
                    CustomerExist = true;
                    foreach (ShareDetail Shareitem in items.ShareDetail)
                    {
                        if (Shareitem.CompanyName == CompanyName)
                        {
                            CompanyExistAtCustomer = true;
                            if (Shareitem.NoOfShares >= shares)
                            {
                                Shareitem.NoOfShares -= shares;
                                StockSharePrices = Shareitem.SharePrice;
                                StockSymbol = Shareitem.StockSymbol;

                                if (Shareitem.NoOfShares == 0)
                                {
                                    items.ShareDetail.Remove(Shareitem);
                                }
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Stocks Not Available at {CompanyName}");
                            }

                        }
                    }
                    if (!CompanyExistAtCustomer)
                    {
                        Console.WriteLine($"{CompanyName} doesn't Exits in {CustomerName} Share Details");
                    }
                    items.CustomerInfo.Balance += shares * StockSharePrices;
                    Console.WriteLine($"\n<<<<<<<<<<<<<<< {CustomerName} have SucessFully Sold {shares} Stocks of {CompanyName} ({StockSymbol}) for Price Rs.{StockSharePrices} each stock at {DateTime.Now} >>>>>>>>>>>>>>>>");
                    Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Total Balance of {CustomerName} ({StockSymbol}) is Rs.{items.CustomerInfo.Balance} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    break;
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine($"{CustomerName} Doesn't Exits in Customer List");

            }
            SaveCustomer();

            foreach (var item in StocksData.Stocks)
            {
                if (item.CompanyName == CompanyName)
                {

                    CompanyExist = true;
                    foreach (StockAccount Customeritem in stockAccounts)
                    {
                        if (Customeritem.CustomerInfo.Name == CustomerName)
                        {
                            item.NoOfShares += shares;
                            break;
                        }
                    }
                }
            }
            if (!CompanyExist)
            {
                StocksData.Stocks.Add(new CommonProperties { CompanyName = CompanyName, NoOfShares = shares, SharePrice = StockSharePrices, StockSymbol = StockSymbol });
            }
            SaveComapny();
        }
        public void ValueOfAccounts(string CustomerName)
        {
            double TotalValue = 0;
            bool CustomerExist = false;
            StockAccounts();
            foreach (var item in stockAccounts)
            {
                if (item.CustomerInfo.Name == CustomerName)
                {
                    CustomerExist = true;
                    foreach (var shares in item.ShareDetail)
                    {
                        TotalValue += shares.NoOfShares * shares.SharePrice;
                    }
                    Console.WriteLine($"Total Value of stocks of {CustomerName} is {TotalValue}");
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine($"{CustomerName} Doesn't Exits in Customer List");
            }
        }
        public void DisplayCustomerInfo()
        {
            Console.WriteLine("*********************** Customer Details *******************");
            StockAccounts();
            foreach (StockAccount CustomerInfo in stockAccounts)
            {
                Console.WriteLine("Customer Name : " + CustomerInfo.CustomerInfo.Name +
                    "\nMobile Number : " + CustomerInfo.CustomerInfo.MobileNumber +
                    "\nEmail : " + CustomerInfo.CustomerInfo.Email +
                    "\nAddress : " + CustomerInfo.CustomerInfo.Address +
                    "\nBalance : " + CustomerInfo.CustomerInfo.Balance);

                foreach (ShareDetail shares in CustomerInfo.ShareDetail)
                {
                    Console.WriteLine("---------------------------------------------");
                    Console.Write("Company Name : " + shares.CompanyName +
                    "\nNumber of Shares : " + shares.NoOfShares +
                    "\nPrice Per Share : " + shares.SharePrice +
                    "\nStock Symbol : " + shares.StockSymbol + "\n");
                }
                Console.WriteLine("=============================================");
            }
        }
        public void DisplayCustomerInfo1(string CustomerName)
        {
            bool CustomerExist = false;
            StockAccounts();
            foreach (StockAccount CustomerInfo in stockAccounts)
            {
                if (CustomerInfo.CustomerInfo.Name == CustomerName)
                {
                    CustomerExist = true;
                    Console.WriteLine("Customer Name : " + CustomerInfo.CustomerInfo.Name +
                        "\nMobile Number : " + CustomerInfo.CustomerInfo.MobileNumber +
                        "\nEmail : " + CustomerInfo.CustomerInfo.Email +
                        "\nAddress : " + CustomerInfo.CustomerInfo.Address +
                        "\nBalance : " + CustomerInfo.CustomerInfo.Balance);
                    Console.WriteLine("---------------------------------------------");

                    foreach (ShareDetail shares in CustomerInfo.ShareDetail)
                    {
                        Console.Write("Company Name : " + shares.CompanyName +
                        "\nNumber of Shares : " + shares.NoOfShares +
                        "\nPrice Per Share : " + shares.SharePrice +
                        "\nStock Symbol : " + shares.StockSymbol + "\n");

                        Console.WriteLine("---------------------------------------------");
                    }
                    break;
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine($"{CustomerName} Doesn't Exits in Customer List");
            }
        }
        public void AddCompany(string CompanyName, int NoOfshares, double SharePrice, string stockSymbol)
        {
            CompanyAccount();
            StocksData.Stocks.Add(new CommonProperties { CompanyName = CompanyName, NoOfShares = NoOfshares, SharePrice = SharePrice, StockSymbol = stockSymbol });
            SaveComapny();
            Console.WriteLine($"\n********* Successfull Added the {CompanyName} in Comapny List *********\n");
        }
        public void RemoveComapny(string companyName)
        {
            CompanyAccount();
            StockManagementRepository stock = new StockManagementRepository();
            foreach (var stockitem in StocksData.Stocks)
            {
                if (stockitem.CompanyName == companyName)
                {
                    Predicate<CommonProperties> RemoveComapany = X => X == stockitem;
                    StocksData.Stocks.RemoveAll(RemoveComapany);
                    break;
                }
            }
            SaveComapny();
            Console.WriteLine($"\n********* Successfull Removed the {companyName} in Comapny List *********\n");
            stock.StockPortfolio();
        }
    }
}

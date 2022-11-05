﻿using Newtonsoft.Json;
using Stock_Account_Management_Problem.Model;


namespace Stock_Account_Management_Problem.Repository
{
    public class CommercialDataProcessing
    {
        public static string CustomerInfoFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\CustomerInfo.json";
        public static string JsonFilePath = @"C:\Users\Mahesh\OneDrive\Desktop\Assignments\RFP .Net Assignment\Stock-Account-Management-Problem\Stock-Account-Management-Problem\JSONFile\StocksData.json";
        string JsonFileData = File.ReadAllText(JsonFilePath);
        string Accounts = File.ReadAllText(CustomerInfoFilePath);
        List<StockAccount> stockAccounts = new List<StockAccount>();
        StocksModel StocksData = new StocksModel();

        public void StockAccounts()
        {
            string Accounts = File.ReadAllText(CustomerInfoFilePath);
            stockAccounts = JsonConvert.DeserializeObject<List<StockAccount>>(Accounts);
        }
        public void CompanyAccount()
        {

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
                    foreach (StockAccount item2 in stockAccounts)
                    {
                        if (item2.CustomerInfo.Name == CustomerName)
                        {
                            if (item.NoOfShares >= shares)
                            {
                                item.NoOfShares -= shares;
                                total = shares * item.SharePrice;
                                StockCompanyName = item.CompanyName;
                                StockSharePrices = item.SharePrice;
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
                Console.WriteLine("Company Name Dosen't Exist in Company List");

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
                            foreach (ShareDetail item2 in items.ShareDetail)
                            {
                                if (item2.CompanyName == CompanyName)
                                {
                                    Exit = true;
                                    item2.NoOfShares += shares;
                                    break;
                                }
                            }
                            if (!Exit)
                            {
                                items.ShareDetail.Add(new ShareDetail() { CompanyName = StockCompanyName, NoOfShares = shares, SharePrice = StockSharePrices });

                            }
                            Console.WriteLine($"\n<<<<<<<<<<<<<<< {CustomerName} have SucessFully Purchased {shares} Stocks of {StockCompanyName} at Price Rs.{StockSharePrices} >>>>>>>>>>>>>>>>");
                            Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Total Remaining Balance of {CustomerName} is {items.CustomerInfo.Balance} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Insufficient Balance in Customer Account Name {CustomerName}");
                    }
                    break;
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine("Customer Name Doesn't Exits in Customer List");

            }

            SaveCustomer();
        }
        public void SellStocks(string CustomerName, string CompanyName, int shares)
        {
            double StockSharePrices = 0;
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
                    foreach (ShareDetail item2 in items.ShareDetail)
                    {
                        if (item2.CompanyName == CompanyName)
                        {
                            CompanyExistAtCustomer = true;
                            if (item2.NoOfShares >= shares)
                            {
                                item2.NoOfShares -= shares;
                                StockSharePrices = item2.SharePrice;
                                if (item2.NoOfShares == 0)
                                {
                                    items.ShareDetail.Remove(item2);
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
                    Console.WriteLine($"\n<<<<<<<<<<<<<<< {CustomerName} have SucessFully Sold {shares} Stocks of {CompanyName} at Price Rs.{StockSharePrices} >>>>>>>>>>>>>>>>");
                    Console.WriteLine($"<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Total Balance of {CustomerName} is {items.CustomerInfo.Balance} >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    break;
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine("Customer Name Doesn't Exits in Customer List");

            }
            SaveCustomer();

            foreach (var item in StocksData.Stocks)
            {
                if (item.CompanyName == CompanyName)
                {

                    CompanyExist = true;
                    foreach (StockAccount item2 in stockAccounts)
                    {
                        if (item.CompanyName == CompanyName)
                        {

                            item.NoOfShares += shares;
                            break;
                        }
                    }
                }
            }
            if (!CompanyExist)
            {
                StocksData.Stocks.Add(new CommonProperties { CompanyName = CompanyName, NoOfShares = shares, SharePrice = StockSharePrices });
            }
            SaveComapny();
        }
        public void ValueOfAccounts(string CutomerName)
        {
            double TotalValue = 0;
            bool CustomerExist = false;
            StockAccounts();
            foreach (var item in stockAccounts)
            {
                if (item.CustomerInfo.Name == CutomerName)
                {
                    CustomerExist = true;   
                    foreach (var shares in item.ShareDetail)
                    {
                        TotalValue += shares.NoOfShares * shares.SharePrice;
                    }
                    Console.WriteLine($"Total Value of stocks of {CutomerName} is {TotalValue}");
                }
            }
            if (!CustomerExist)
            {
                Console.WriteLine("Customer Name Doesn't Exits in Customer List");
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
                Console.WriteLine("---------------------------------------------");
               
                foreach (ShareDetail shares in CustomerInfo.ShareDetail)
                {
                    Console.Write("Company Name : " + shares.CompanyName +
                    "\nNumber of Shares : " + shares.NoOfShares +
                    "\nPrice Per Share : " + shares.SharePrice + "\n");
                    Console.WriteLine("---------------------------------------------");
                }
                Console.WriteLine("=============================================");
            }
        }
        public void DisplayCustomerInfo1(string CustomerName)
        {
            StockAccounts();
            foreach (StockAccount CustomerInfo in stockAccounts)
            {
                if (CustomerInfo.CustomerInfo.Name == CustomerName)
                {
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
                        "\nPrice Per Share : " + shares.SharePrice + "\n");

                        Console.WriteLine("---------------------------------------------");
                    }
                    break;
                }
            }
        }

    }
}
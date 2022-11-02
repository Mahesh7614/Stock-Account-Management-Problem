using Stock_Account_Management_Problem.Repository;

namespace Stock_Account_Management_Problem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StockManagementRepository repository = new StockManagementRepository();
            repository.StockPortfolio();
        }
    }
}
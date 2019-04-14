using Billing.Entities;

namespace Billing.DAL.Helpers
{
    public static class BankHelper
    {
        public static string getBankName(string SearchKey)
        {
            string BankName = string.Empty;
            switch (SearchKey)
            {
                case "CityBankLimited": { BankName = "The City Bank Limited"; break; }
                case "PubaliBankLtd": { BankName = "Pubali Bank Limited"; break; }
                case "IslamiBank": { BankName = "Islami Bank Limited"; break; }
                case "BracBankLtd": { BankName = "Brac Bank Limited"; break; }
                case "DBBL": { BankName = "Dutch-Bangla Bank Limited"; break; }
                case "JanataBankLtd": { BankName = "Janata Bank Limited"; break; }
                case "HSBC": { BankName = "HSBC Bank"; break; }
                case "Barclays": { BankName = "Barclays Bank"; break; }
                case "Lloyds": { BankName = "Lloyds Bank"; break; }
                default: { BankName = string.Empty; break; }
            }
            return BankName;
        }

        public static double getBankBalance(int AccountId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            BankAccount Balance = db.BankAccounts.Find(AccountId);
            return Balance.Balance;
        }
    }
}
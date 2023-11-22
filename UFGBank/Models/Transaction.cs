
using System.ComponentModel.DataAnnotations;
using UFGBank.Areas.Identity.Data;

namespace UFGBank.Models;

public enum TransactionType
{
    Deposit,
    Withdraw
};

public class Transaction
{
    public int ID { get; set; }

    public TransactionType Type { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }


    [DataType(DataType.DateTime)] public DateTime Date { get; set; }


    public string BankUserID { get; set; }
    public UFGBankUser AspNetUsers { get; set; }
}

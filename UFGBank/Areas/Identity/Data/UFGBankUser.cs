using System;
using System.Collections.Generic;
using System.Linq;
using UFGBank.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UFGBank.Areas.Identity.Data;

// Add profile data for application users by adding properties to the UFGBankUser class
public class UFGBankUser : IdentityUser
{
    
    [Required] public string FirstName { get; set; } = null!;

    [Required] public string LastName { get; set; } = null!;

    [Required]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    public ICollection<Transaction> Transactions { get; set; }
}


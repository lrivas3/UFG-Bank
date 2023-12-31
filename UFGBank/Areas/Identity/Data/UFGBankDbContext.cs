﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UFGBank.Areas.Identity.Data;
using UFGBank.Models;

namespace UFGBank.Data;

public class UFGBankDbContext : IdentityDbContext<UFGBankUser>
{
    public UFGBankDbContext(DbContextOptions<UFGBankDbContext> options)
        : base(options)
    {
    }

    public DbSet<UFGBankUser> AspNetUsers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UFGBank.Areas.Identity.Data;
using UFGBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using UFGBank.Data;
using UFGBank.Models;

namespace UFGBank.Controllers;

[Authorize]
public class TransactionController : Controller
{
    private readonly UFGBankDbContext _context;


    public TransactionController(UFGBankDbContext context)
    {
        _context = context;
    }

    [BindProperty] public InputModel Input { get; set; }


    //GET: Transaction/Deposit
    public IActionResult Deposit()
    {
        return View();
    }

    //POST: Transaction/Deposit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit([Bind("Amount")] Transaction transaction)
    {
        var currentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        transaction.BankUserID = currentId;
        transaction.Date = DateTime.UtcNow;
        transaction.Type = TransactionType.Deposit;
        var bankUser = _context.AspNetUsers.Where(i => i.Id == currentId).First();

        try
        {
            if (ModelState.IsValid)
            {
                bankUser.Balance += Input.Amount;
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Unable to complete the transaction. " +
                                         "Try again, and if the problem persists " +
                                         "see your system administrator.");
        }

        return View(transaction);
    }

    //GET: Transaction/Withdraw
    public IActionResult Withdraw()
    {
        return View();
    }

    //POST: Transaction/Withdraw
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Withdraw([Bind("Amount")] Transaction transaction)
    {
        var currentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        Console.WriteLine("TransactionController.cs: currentId = " + currentId);
        transaction.BankUserID = currentId;
        transaction.Date = DateTime.UtcNow;
        transaction.Type = TransactionType.Withdraw;
        var bankUser = _context.AspNetUsers.Where(i => i.Id == currentId).First();

        try
        {
            if (ModelState.IsValid && (bankUser.Balance - Input.Amount) > 0)
            {
                bankUser.Balance -= Input.Amount;
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Unable to complete the transaction. " +
                                         "Try again, and if the problem persists " +
                                         "see your system administrator.");
        }

        return View(transaction);
    }

    //GET: Transaction/PersonalTransactions
    public async Task<IActionResult> PersonalTransactions()
    {
        var currentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        var userTransactions = await _context.AspNetUsers
            .Include(s => s.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == currentId);

        UtcToLocalDate(userTransactions);

        return View(userTransactions);
    }

    //maybe should be a Post call so id doesn't show up in URL
    //GET: Transaction/PersonalTransactions/id
    [Route("{id}")]
    public async Task<IActionResult> PersonalTransactions(string id)
    {
        var currentId = id;

        var userTransactions = await _context.AspNetUsers
            .Include(s => s.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == currentId);

        UtcToLocalDate(userTransactions);

        return View(userTransactions);
    }

    public class InputModel : Transaction
    {
    }

    public void UtcToLocalDate(UFGBankUser user)
    {
        DateTime utcToLocal;

        foreach (var transaction in user.Transactions)
        {
            utcToLocal = transaction.Date.ToLocalTime();
            transaction.Date = utcToLocal;
        }
    }
}
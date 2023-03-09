using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IMS.Authorization;

namespace IMS.Pages.Invoices
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext applicationDbContext,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
            ) : base(applicationDbContext, authorizationService, userManager)
        {
        }

        public Invoice Invoice { get; set; }

        // displays the details' page...
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Invoice = await Context.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);
            
            if (Invoice == null)
                return NotFound();

            var isInvoiceCreator = await AuthorizationService.AuthorizeAsync(User, Invoice, InvoiceOperations.Read);

            var isManager = User.IsInRole(Constants.InvoiceManagerRole);

            // if the current user isn't the creator of the invoice (accountant) and also not a manager
            if (!isInvoiceCreator.Succeeded && !isManager)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, InvoiceStatus status)
        {
            Invoice = await Context.Invoice.FindAsync(id);

            if (Invoice == null)
                return NotFound();

            var invoiceOperation = status == InvoiceStatus.APPROVED ? InvoiceOperations.Approve
                : InvoiceOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Invoice, invoiceOperation);

            if (!isAuthorized.Succeeded)
                return Forbid();

            Invoice.Status = status;
            Context.Invoice.Update(Invoice);

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index"); 
        }
    }
}

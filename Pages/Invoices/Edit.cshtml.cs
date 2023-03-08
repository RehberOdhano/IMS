using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IMS.Authorization;

namespace IMS.Pages.Invoices
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext applicationDbContext,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
            ) : base(applicationDbContext, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;

        // this function is called when we load the edit invoice page...
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Invoice == null)
            {
                return NotFound();
            }

            var invoice =  await Context.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);
            
            if (invoice == null)
            {
                return NotFound();
            }

            // only the person/user who has added the invoice(s), only those invoice(s) will be
            // shown on the page...

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Invoice, InvoiceOperations.Update);

            if (!isAuthorized.Succeeded)
                return Forbid();



            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        
        // this function is called when we submit the data in order to edit the form...
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var invoice = await Context.Invoice.AsNoTracking().SingleOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null)
                return NotFound();
            
            // sending the invoice creator id to the view...
            // we'll set this id in our edit view... 
            Invoice.InvoiceCreatorId = invoice.InvoiceCreatorId;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Invoice, InvoiceOperations.Update);

            if (!isAuthorized.Succeeded)
                return Forbid();

            Context.Attach(Invoice).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(Invoice.InvoiceId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InvoiceExists(int id)
        {
          return Context.Invoice.Any(e => e.InvoiceId == id);
        }
    }
}

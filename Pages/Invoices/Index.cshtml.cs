using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IMS.Authorization;

namespace IMS.Pages.Invoices
{
    // now this will make the index page fully acccessible to view to anyone...
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext applicationDbContext,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager
            ) : base(applicationDbContext, authorizationService, userManager)
        {
        }

        public IList<Invoice> Invoice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // fetching all the invoices...
            var invoices = from invoice in Context.Invoice
                           select invoice;

            // checking if the current user is the manager...
            var isManager = User.IsInRole(Constants.InvoiceManagerRole);

            // checking if the current user is the admin..
            var isAdmin = User.IsInRole(Constants.InvoiceAdminRole);

            var currentUserId = UserManager.GetUserId(User);

            // if the current user isn't a manager, its an accountant, then grab all those
            // invoices that are specific to that accountant...
            if (!isManager && !isAdmin)
            {
                invoices = invoices.Where(invoice => invoice.InvoiceCreatorId == currentUserId);
            }

            Invoice = await invoices.ToListAsync();
        }
    }
}

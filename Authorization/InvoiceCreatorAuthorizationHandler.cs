using Microsoft.AspNetCore.Identity;
using IMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IMS.Authorization
{
    public class InvoiceCreatorAuthorizationHandler : 
        AuthorizationHandler<OperationAuthorizationRequirement, Invoice>
    {
        readonly UserManager<IdentityUser> _userManager;
        public InvoiceCreatorAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // here is the user and he/she wants to do that operation(s)... now check your requirements...
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, 
            Invoice invoice)
        {

            // if the authorization fails
            if (context.User == null || invoice == null)
                return Task.CompletedTask;
            
            if(requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.DeleteOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.ReadOperationName)
            {
                return Task.CompletedTask;
            }

            // if the authorization is succeeded!
            if (invoice.InvoiceCreatorId == _userManager.GetUserId(context.User))
                context.Succeed(requirement);

            // if all above conditions are false
            return Task.CompletedTask;
        }
    }
}

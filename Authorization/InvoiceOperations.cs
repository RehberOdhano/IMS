using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IMS.Authorization
{
    public static class InvoiceOperations
    {
        public static readonly OperationAuthorizationRequirement Create =
            new() { Name = Constants.CreateOperationName };
        
        public static readonly OperationAuthorizationRequirement Read =
            new() { Name = Constants.ReadOperationName };
        
        public static readonly OperationAuthorizationRequirement Update =
            new() { Name = Constants.UpdateOperationName };
        
        public static readonly OperationAuthorizationRequirement Delete =
            new() { Name = Constants.DeleteOperationName };
        
        public static readonly OperationAuthorizationRequirement Approve =
            new() { Name = Constants.ApprovedOperationName };
        
        public static readonly OperationAuthorizationRequirement Reject =
            new() { Name = Constants.RejectedOperationName };
    }

    public static class Constants
    {
        public static readonly string CreateOperationName = "CREATE";
        public static readonly string ReadOperationName = "READ";
        public static readonly string UpdateOperationName = "UPDATE";
        public static readonly string DeleteOperationName = "DELETE";

        public static readonly string ApprovedOperationName = "APPROVED";
        public static readonly string RejectedOperationName = "REJECTED";

        public static readonly string InvoiceManagerRole = "INVOICE_MANAGER";
        public static readonly string InvoiceAdminRole = "INVOICE_ADMIN";
    }
}

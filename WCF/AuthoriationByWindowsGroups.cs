public static class UserPermissions
{
    public static bool CheckRole(Role role)
    {
        try 
        {
            var permission = new PrincipalPermission(null, role.ToString());
            permission.Demand();
        }
        catch (SecurityException) 
        {
            return false;
        }
        return true;
    }

    public static void AssertRole(Role role)
    {
        if (!CheckRole(role)) 
        {
            throw new WebFaultException(HttpStatusCode.Forbidden);
        }
    }
}

public enum Role
{
    Administrator,
    Customer
}


//Usage:
public class CustomerService : ICustomerService
{
        public List<Order> GetOrders()
        {
            UserPermissions.AssertRole(Role.Customer);
            // Code to get orders.
        }
}

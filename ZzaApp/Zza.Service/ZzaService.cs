using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Web;
using System.Web.Security;
using Zza.Constants;
using Zza.Data;
using Zza.Entities;

namespace Zza.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ZzaService : IZzaService, IDisposable
    {
        readonly ZzaDbContext _Context = new ZzaDbContext();

        public void Dispose()
        {
            _Context.Dispose();
        }

        public List<Customer> GetCustomers()
        {
            return _Context.Customers.ToList();
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "BUILTIN\\Administrators")]
        [PrincipalPermission(SecurityAction.Demand, Role = Sids.Admin)]
        public List<Product> GetProducts()
        {
            //IPrincipal principal = Thread.CurrentPrincipal;
            //Sids.DebugLogAllSidRoleValuesForUser(principal);

            //if (!principal.IsInRole("BUILTIN\\Administrators"))
            //if (!principal.IsInRole(Sids.Admin))
            //{
            //    throw new SecurityException("AccessDenied");
            //}

            return _Context.Products.ToList();
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void SubmitOrder(Order order)
        {
            _Context.Orders.Add(order);
            order.OrderItems.ForEach(oi => _Context.OrderItems.Add(oi));
            _Context.SaveChanges();
        }
    }
}
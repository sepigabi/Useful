namespace MyNamespace.Authorization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        RoleName1 = 1,
        RoleName2 = 2,
        RoleName3 = 3,
        RoleName4 = 4
    }

    public enum Environments
    {
        Production,
        Test,
        Development
    }
}

namespace MyNamespace.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    public class RoleManager
    {
        private static readonly Object sync;
        private static volatile RoleManager instance;
        private Environments? environment;
        private Dictionary<Roles, string> roles;


        static RoleManager()
        {
            sync = new Object();
        }
        private RoleManager()
        {
        }

        public static RoleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (sync)
                    {
                        if (instance == null)
                        {
                            instance = new RoleManager();
                        }
                    }
                }
                return instance;
            }
        }

        public Environments Environment
        {
            get
            {
                if (this.environment == null)
                {
                    var environmentName = System.Configuration.ConfigurationManager.AppSettings["EnvironmentName"];
                    if (string.IsNullOrEmpty(environmentName))
                    {
                        throw new Exception("Please set the 'EnvironmentName' appSetting in the web.config properly!");
                    }
                    if (!Enum.TryParse(environmentName, out Environments environment))
                    {
                        throw new Exception($"Please set the 'EnvironmentName' appSetting in the web.config properly! Allowed environmentNames are: {string.Join(",", Enum.GetNames(typeof(Environments)))}");
                    }
                    this.environment = environment;
                }
                return this.environment.Value;
            }
        }

        public Dictionary<Roles, string> Roles
        {
            get
            {
                if (this.roles == null)
                {
                    InitializeRoles(this.Environment);
                }
                return this.roles;
            }
        }

        public IEnumerable<Roles> GetUserRoles()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if(IsPrincipalInRole(role, principal))
                {
                    yield return role;
                }
            }
        }

        public bool CheckUserPermission(Roles role)
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return IsPrincipalInRole(role, principal);
        }

        private bool IsPrincipalInRole(Roles role, WindowsPrincipal principal)
        {
            if (this.Roles.TryGetValue(role, out string roleName))
            {
                if (!string.IsNullOrEmpty(roleName) && (roleName == "*" || principal.IsInRole(roleName)))
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeRoles(Environments environment)
        {
            this.roles = new Dictionary<Roles, string>();
            switch (environment)
            {
                case Environments.Production:
                    this.roles.Add(Authorization.Roles.RoleName1, "Prod_role1");
                    this.roles.Add(Authorization.Roles.RoleName2, "Prod_role2");
                    this.roles.Add(Authorization.Roles.RoleName3, "Prod_role3");
                    this.roles.Add(Authorization.Roles.RoleName4, "Prod_role4");
                    break;
                case Environments.Test:
                    this.roles.Add(Authorization.Roles.RoleName1, "Test_role1");
                    this.roles.Add(Authorization.Roles.RoleName2, "Test_role2");
                    this.roles.Add(Authorization.Roles.RoleName3, "Test_role3");
                    this.roles.Add(Authorization.Roles.RoleName4, "Test_role4");
                    break;
                case Environments.Development:
                    this.roles.Add(Authorization.Roles.RoleName1, "*");
                    this.roles.Add(Authorization.Roles.RoleName2, "*");
                    this.roles.Add(Authorization.Roles.RoleName3, "*");
                    this.roles.Add(Authorization.Roles.RoleName4, "*");
                    break;
                default:
                    break;
            }
        }
    }
}

namespace MyNamespace.Authorization
{
    using System;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AllowAnonymousInDevelopmentEnvironmentAttribute : Attribute
    {
    }
}

namespace MyNamespace.Authorization
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private Roles? role;

        public Roles Role
        {
            get
            {
                if (this.role.HasValue)
                {
                    return this.role.Value;
                }
                else
                {
                    throw new Exception("Role has not value");
                }
            }
            set
            {
                this.role = value;
            }
        }


        protected virtual bool IsAuthenticated(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            IPrincipal user = actionContext.ControllerContext.RequestContext.Principal;
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            return true;
        }

        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!IsAuthenticated(actionContext))
            {
                return false;
            }

            if (this.role.HasValue)
            {
                return RoleManager.Instance.CheckUserPermission(this.role.Value);
            }

            return true;
        }


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (SkipAuthorization(actionContext))
            {
                return;
            }

            if (!IsAuthenticated(actionContext))
            {
                HandleUnAuthenticated(actionContext);
                return;
            }

            if (!IsAuthorized(actionContext))
            {
                HandleUnAuthorized(actionContext);
            }
        }

        private void HandleUnAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            actionContext.Response.Content = new StringContent("You are not authorized to access this API.");
        }

        private void HandleUnAuthenticated(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            var allowAnonymusInDevelopmentEnvironment = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousInDevelopmentEnvironmentAttribute>().Any()
                 || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousInDevelopmentEnvironmentAttribute>().Any();

            var isDevelopmentEnvironment = RoleManager.Instance.Environment == Environments.Development;

            return allowAnonymusInDevelopmentEnvironment && isDevelopmentEnvironment;
        }
    }
}

//USAGE:
//Web.config:
    <configuration>
      <appSettings>
       <add key="EnvironmentName" value="Development"/> <!--Production,Test,Development-->
      </appSettings>
    </configuration>

//Controller:
    [AllowAnonymousInDevelopmentEnvironment]
    [CustomAuthorize]
    public class MyController : ApiController
    {
        [CustomAuthorize(Role = Roles.RoleName1)]
        [Route("GetSomething")]
        [HttpPost]
        public IHttpActionResult MyMethod([FromBody]string parameterName)
        {
          …
        }
    }



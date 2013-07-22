using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Lombiq.Watcher
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission WatchItems = new Permission { Description = "Watch content items", Name = "WatchItems" };

        public virtual Feature Feature { get; set; }


        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                WatchItems
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Authenticated",
                    Permissions = new[] { WatchItems }
                }
            };
        }
    }
}
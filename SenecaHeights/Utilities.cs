using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SenecHeights.DataAccessLayer;

namespace SenecaHeights
{
    public static class Utilities
    {
        public static Boolean UserIsInRole(string userId, string roleName)
        {
            var sqlString = "SELECT Id FROM AspNetRoles WHERE Name = '" + roleName + "'";
            string roleId;
            using (var context = new SenecaContext())
            {
                roleId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                if (roleId == null)
                {
                    return false;
                }
            }

            sqlString = "SELECT UserId FROM AspNetUserRoles WHERE ";
            sqlString += "UserId = '" + userId + "' AND RoleId ='" + roleId + "'";
            using (var context = new SenecaContext())
            {
                var success = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                if (success != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
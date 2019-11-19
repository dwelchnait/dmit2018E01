using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region Additional Namespaces
using System.ComponentModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;
using System.Configuration;
#endregion

namespace WebApp.Security
{
    [DataObject]
    public class SecurityController
    {
        #region Constructor & Dependencies
        private readonly ApplicationUserManager UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        public SecurityController()
        {
            UserManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }

        private void CheckResult(IdentityResult result, string item, string action)
        {
            if (!result.Succeeded)
                throw new Exception($"Failed to " + action + $" " + item + 
                    $":<ul> {string.Join(string.Empty, result.Errors.Select(x => $"<li>{x}</li>"))}</ul>");
        }
        #endregion

        #region ApplicationUser CRUD
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ApplicationUser> ListUsers()
        {
            return UserManager.Users.Where(x => x.EmployeeId.HasValue ||
                                x.CustomerId.HasValue).OrderBy(x => x.UserName).ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddUser(ApplicationUser user)
        {
            if (user.EmployeeId == 0 )
            {
                throw new Exception("Pick a customer or employee.");
            }
            
            user.CustomerId = null;
            

            IdentityResult result = UserManager.Create(user, ConfigurationManager.AppSettings["newUserPassword"]);
            CheckResult(result,"user", "add");
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdateUser(ApplicationUser user)
        {
            var existing = UserManager.FindById(user.Id);
            if (existing == null)
                throw new Exception("The specified user was not found");
            else if (existing.UserName == ConfigurationManager.AppSettings["adminUserName"] && existing.UserName != user.UserName)
                throw new Exception("You are not allowed to modify the website administrator's user name");
            // Change certain parts of the found user
            if (user.EmployeeId == 0)
            {
                existing.EmployeeId = null;
            }
 
            existing.Email = user.Email;
            existing.PhoneNumber = user.PhoneNumber;
            existing.UserName = user.UserName; // Generally NOT a good idea to change this!
            //var result = UserManager.Update(existing);
            CheckResult(UserManager.Update(existing), "user", "update");
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void DeleteUser(ApplicationUser user)
        {
            var existing = UserManager.FindById(user.Id);
            if (existing == null)
                throw new Exception("The specified user was not found");
            else if (existing.UserName == ConfigurationManager.AppSettings["adminUserName"])
                throw new Exception("You are not allowed to delete the website administrator");
            using (var context = new ApplicationDbContext())
            {
                if (existing.Roles.Count() > 0)
                {
                    foreach (var item in existing.Roles)
                    {
                        RemoveUserRole(existing.Id, item.RoleId);
                    }
                }

                CheckResult(UserManager.Delete(existing), "user", "remove");
            }
        }
        #endregion

        #region IdentityRole CRUD
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<IdentityRole> ListRoles()
        {
            return RoleManager.Roles.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddRole(IdentityRole role)
        {
            CheckResult(RoleManager.Create(role), "role", "add");
        }

        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdateRole(IdentityRole role)
        {
            var existing = RoleManager.FindById(role.Id);
            if (existing == null)
                throw new Exception("The specified role could not be found");
            else if (existing.Name == ConfigurationManager.AppSettings["adminRole"])
                throw new Exception("Cannot rename the administrator role");
            existing.Name = role.Name;
            CheckResult(RoleManager.Update(existing), "role", "update");
        }

        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void DeleteRole(IdentityRole role)
        {
            var existing = RoleManager.FindById(role.Id);
            if (existing == null)
                throw new Exception("The specified role could not be found");
            if (existing.Name == ConfigurationManager.AppSettings["adminRole"])
                throw new Exception("Cannot delete the administrator role");
            if (existing.Users.Count > 0)
            {
                throw new Exception("Role has users. Remove or reassign role users");
            }
            CheckResult(RoleManager.Delete(existing), "role", "remove");
        }
        #endregion

        #region UserRole CRUD
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ApplicationUser> ListUserEmployees()
        {
            return UserManager.Users.Where(x => x.EmployeeId.HasValue).OrderBy(x => x.UserName).ToList();
        }

        public void AddUserRole(string userid, string roleid)
        {
            var existinguser = UserManager.FindById(userid);
            if (existinguser == null)
            {
                throw new Exception("The specified user could not be found");
            }
            var existingrole = RoleManager.FindById(roleid);
            if (existingrole == null)
                throw new Exception("The specified role could not be found");
            if (existinguser.Roles.Where(x => x.RoleId.Equals(roleid)).FirstOrDefault() == null)
            {
               UserManager.AddToRole(existinguser.Id, existingrole.Name);                
            }
            else
            {
                throw new Exception("The specified user already in role.");
            }
        }

        public void RemoveUserRole(string userid, string roleid)
        {
            
            var existinguser = UserManager.FindById(userid);
            if (existinguser == null)
            {
                throw new Exception("The specified user could not be found");
            }
            var existingrole = existinguser.Roles.Where(x => x.RoleId.Equals(roleid)).FirstOrDefault();
            if (existingrole == null)
            {
                throw new Exception("User does not belong to the role");
            }
            CheckResult(UserManager.RemoveFromRole(existinguser.Id, RoleManager.FindById(roleid).Name),"user role", "remove");
        }
        #endregion

        #region Employee/Customer IDs
        //Add this code to your SecurityController in your WebApp. This code will return an id value for either of Employee or Customer 
        //of the logged in user. You can then use the value to obtain the appropriate employee or customer record from your database.

        public int? GetCurrentUserEmployeeId(string userName)
        {
            int? id = null;
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated)
            {
                var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var appUser = manager.Users.SingleOrDefault(x => x.UserName == userName);
                if (appUser != null)
                    id = appUser.EmployeeId;
            }
            return id;
        }

        public int? GetCurrentUserCustomerId(string userName)
        {
            int? id = null;
            var request = HttpContext.Current.Request;
            if (request.IsAuthenticated)
            {
                var manager = request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var appUser = manager.Users.SingleOrDefault(x => x.UserName == userName);
                if (appUser != null)
                    id = appUser.CustomerId;
            }
            return id;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Xml;

namespace MediaDataApplication.WcfService.Authorization {

    public class XmlRoleProvider : RoleProvider {
        private readonly Dictionary<string, string[]> rolesAndUsers = new Dictionary<string, string[]>(16, StringComparer.InvariantCultureIgnoreCase);

        private readonly Dictionary<string, string[]> usersAndRoles = new Dictionary<string, string[]>(16, StringComparer.InvariantCultureIgnoreCase);

        private string xmlFileName;

        public override string ApplicationName {
            get {
                throw new NotSupportedException();
            }
            set {
                throw new NotSupportedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotSupportedException();
        }

        public override void CreateRole(string roleName) {
            throw new NotSupportedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole) {
            throw new NotSupportedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch) {
            throw new NotSupportedException();
        }

        public override string[] GetAllRoles() {
            int i = 0;
            var roles = new string[this.rolesAndUsers.Count];
            foreach (KeyValuePair<string, string[]> pair in this.rolesAndUsers) {
                roles[i++] = pair.Key;
            }
            return roles;
        }

        // This method is used by PrincipalPermissionAttribute
        public override string[] GetRolesForUser(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                throw new ArgumentException("username");
            }
            string[] roles;
            if (!this.usersAndRoles.TryGetValue(username, out roles)) {
                throw new ProviderException("Invalid user name");
            }
            return roles;
        }

        public override string[] GetUsersInRole(string roleName) {
            // Validate input parameters
            if (roleName == null) {
                throw new ArgumentNullException();
            }
            if (roleName == string.Empty) {
                throw new ArgumentException();
            }
            // Make sure the role name is valid
            string[] users;
            if (!this.rolesAndUsers.TryGetValue(roleName, out users)) {
                throw new ProviderException("Invalid role name");
            }
            // Return user names
            return users;
        }

        public override void Initialize(string name, NameValueCollection config) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name)) {
                name = "XmlRoleProvider";
            }

            // Add a default "description" attribute to config if the attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Read-only XML role provider");
            }

            base.Initialize(name, config);

            // Initialize xmlFileName and make sure the path is app-relative
            string path = config["xmlFileName"];
            if (String.IsNullOrEmpty(path)) {
                path = "~/App_Data/Users.xml";
            }
            if (!VirtualPathUtility.IsAppRelative(path)) {
                throw new ArgumentException("xmlFileName must be app-relative");
            }
            string fullyQualifiedPath =
                VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(HttpRuntime.AppDomainAppVirtualPath),
                                           path);
            this.xmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            config.Remove("xmlFileName");

            // Make sure we have permission to read the XML data source and  throw an exception if we don't
            var permission = new FileIOPermission(FileIOPermissionAccess.Read, this.xmlFileName);
            permission.Demand();

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr)) {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }

            // Read the role data source. NOTE: Unlike  XmlMembershipProvider, this provider can
            // read the data source at this point because Read-
            // RoleDataStore doesn't call into the role manager
            this.ReadRoleDataStore();
        }

        public override bool IsUserInRole(string username, string roleName) {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(username));
            Contract.Requires<ArgumentNullException>(!string.IsNullOrWhiteSpace(roleName));

            // Make sure the user name and role name are valid
            if (!this.usersAndRoles.ContainsKey(username)) {
                throw new ProviderException("Invalid user name");
            }
            if (!this.rolesAndUsers.ContainsKey(roleName)) {
                throw new ProviderException("Invalid role name");
            }

            // Determine whether the user is in the specified role 
            string[] roles = this.usersAndRoles[username];
            return roles.Any(role => String.Compare(role, roleName, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames) {
            throw new NotSupportedException();
        }

        public override bool RoleExists(string roleName) {
            if (string.IsNullOrWhiteSpace(roleName)) {
                throw new ProviderException("Role name cannot be empty or null.");
            }

            return this.rolesAndUsers.ContainsKey(roleName);
        }

        #region Private Helpers

        private void ReadRoleDataStore() {
            var doc = new XmlDocument();
            doc.Load(this.xmlFileName);
            XmlNodeList nodes = doc.GetElementsByTagName("User");

            foreach (XmlNode node in nodes) {
                if (node["UserName"] == null) {
                    throw new ProviderException("Missing UserName element");
                }

                string user = node["UserName"].InnerText;
                if (String.IsNullOrEmpty(user)) {
                    throw new ProviderException("Empty UserName element");
                }

                if (node["Roles"] == null || String.IsNullOrEmpty(node["Roles"].InnerText)) {
                    this.usersAndRoles.Add(user, new string[0]);
                }
                else {
                    string[] roles = node["Roles"].InnerText.Split(',');
                    // Add the role names to _UsersAndRoles and key them by user name
                    this.usersAndRoles.Add(user, roles);
                    foreach (string role in roles) {
                        // Add the user name to _RolesAndUsers and key it by role names
                        string[] users1;
                        if (this.rolesAndUsers.TryGetValue(role, out users1)) {
                            var users2 = new string[users1.Length + 1];
                            users1.CopyTo(users2, 0);
                            users2[users1.Length] = user;
                            this.rolesAndUsers.Remove(role);
                            this.rolesAndUsers.Add(role, users2);
                        }
                        else {
                            this.rolesAndUsers.Add(role, new string[] { user });
                        }
                    }
                }
            }
        }

        #endregion
    }

}
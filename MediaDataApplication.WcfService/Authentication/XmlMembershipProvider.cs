using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Xml;

namespace MediaDataApplication.WcfService.Authentication {

    public class XmlMembershipProvider : MembershipProvider {
        private Dictionary<string, MembershipUser> users;
        private string xmlFileName;

        public override string ApplicationName {
            get {
                throw new NotSupportedException();
            }
            set {
                throw new NotSupportedException();
            }
        }

        public override bool EnablePasswordRetrieval {
            get {
                return false;
            }
        }

        public override bool EnablePasswordReset {
            get {
                return false;
            }
        }

        public override int MaxInvalidPasswordAttempts {
            get {
                throw new NotSupportedException();
            }
        }

        public override int MinRequiredNonAlphanumericCharacters {
            get {
                throw new NotSupportedException();
            }
        }

        public override int MinRequiredPasswordLength {
            get {
                throw new NotSupportedException();
            }
        }

        public override int PasswordAttemptWindow {
            get {
                throw new NotSupportedException();
            }
        }

        public override MembershipPasswordFormat PasswordFormat {
            get {
                throw new NotSupportedException();
            }
        }

        public override string PasswordStrengthRegularExpression {
            get {
                throw new NotSupportedException();
            }
        }

        public override bool RequiresQuestionAndAnswer {
            get {
                throw new NotSupportedException();
            }
        }

        public override bool RequiresUniqueEmail {
            get {
                throw new NotSupportedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword) {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username,
                                                             string password,
                                                             string newPasswordQuestion,
                                                             string newPasswordAnswer) {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(string username,
                                                  string password,
                                                  string email,
                                                  string passwordQuestion,
                                                  string passwordAnswer,
                                                  bool isApproved,
                                                  object providerUserKey,
                                                  out MembershipCreateStatus status) {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch,
                                                                  int pageIndex,
                                                                  int pageSize,
                                                                  out int totalRecords) {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch,
                                                                 int pageIndex,
                                                                 int pageSize,
                                                                 out int totalRecords) {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
            // NOTE: This implementation ignores pageIndex and pageSize, and it doesn't sort the MembershipUser objects returned
            // Make sure the data source has been loaded
            this.ReadMembershipDataStore();
            var allUsers = new MembershipUserCollection();
            foreach (KeyValuePair<string, MembershipUser> pair in this.users) {
                allUsers.Add(pair.Value);
            }
            totalRecords = allUsers.Count;
            return allUsers;
        }

        public override int GetNumberOfUsersOnline() {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer) {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline) {
            // Note: This implementation ignores userIsOnline
            if (String.IsNullOrEmpty(username)) {
                return null;
            }
            // Make sure the data source has been loaded
            this.ReadMembershipDataStore();
            // Retrieve the user from the data source
            MembershipUser user;
            if (this.users.TryGetValue(username, out user)) {
                return user;
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email) {
            throw new NotSupportedException();
        }

        public override void Initialize(string name, NameValueCollection config) {
            if (config == null) {
                throw new ArgumentNullException("config");
            }

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name)) {
                name = "XmlMembershipProvider";
            }

            // Add a default "description" attribute to config if the attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "Read-only XML membership provider");
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

            // Make sure we have permission to read the XML data source and throw an exception if we don't
            var permission = new FileIOPermission(FileIOPermissionAccess.Read, this.xmlFileName);
            permission.Demand();

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0) {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr)) {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
        }

        public override string ResetPassword(string username, string answer) {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName) {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user) {
            throw new NotSupportedException();
        }

        public override bool ValidateUser(string username, string password) {
            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password)) {
                return false;
            }
            try {
                // Make sure the data source has been loaded
                this.ReadMembershipDataStore();

                // Validate the user name and password
                MembershipUser user;
                if (this.users.TryGetValue(username, out user)) {
                    if (user.Comment == password) {
                        // NOTE: A read/write membership provider would update the user's LastLoginDate here.
                        // A fully featured provider would also fire an AuditMembershipAuthenticationSuccess Web event
                        return true;
                    }
                }
                // NOTE: A fully featured membership provider would fire an AuditMembershipAuthenticationFailure Web event here
                return false;
            }
            catch (Exception) {
                return false;
            }
        }

        #region Private Helpers

        private void ReadMembershipDataStore() {
            lock (this) {
                if (this.users != null) {
                    return;
                }

                this.users = new Dictionary<string, MembershipUser>(16, StringComparer.InvariantCultureIgnoreCase);
                var doc = new XmlDocument();
                doc.Load(this.xmlFileName);
                XmlNodeList nodes = doc.GetElementsByTagName("User");
                foreach (XmlNode node in nodes) {
                    var user = new MembershipUser(Name,
                                                  node["UserName"].InnerText,
                                                  null,
                                                  null,
                                                  String.Empty,
                                                  node["Password"].InnerText,
                                                  true,
                                                  false,
                                                  DateTime.Now,
                                                  DateTime.Now,
                                                  DateTime.Now,
                                                  DateTime.Now,
                                                  new DateTime(1980, 1, 1));
                    this.users.Add(user.UserName, user);
                }
            }
        }

        #endregion
    }

}
using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.IdentityModel.Selectors;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Security;

namespace MediaDataApplication.WcfService.Authentication {

    public class CustomAuthenticationManager : ServiceAuthenticationManager {
        public override ReadOnlyCollection<IAuthorizationPolicy> Authenticate(
            ReadOnlyCollection<IAuthorizationPolicy> authPolicy,
            Uri listenUri,
            ref Message message) {
            var serviceClientCredentials =
                OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("ClientCredentials", "ns").Split(',');
            var userName = serviceClientCredentials[0];
            var password = serviceClientCredentials[1];

            UserNamePasswordValidator.CreateMembershipProviderValidator(Membership.Provider)
                                     .Validate(userName, password);

            var clientIdentity = new GenericIdentity(userName);
            message.Properties["Principal"] = new GenericPrincipal(clientIdentity, Roles.GetRolesForUser(userName));

            return authPolicy;
        }
    }

}
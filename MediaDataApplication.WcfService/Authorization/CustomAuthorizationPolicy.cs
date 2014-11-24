using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.ServiceModel;

namespace MediaDataApplication.WcfService.Authorization {

    internal class CustomAuthorizationPolicy : IAuthorizationPolicy {
        private Guid id = Guid.NewGuid();

        #region IAuthorizationPolicy Members

        public bool Evaluate(EvaluationContext evaluationContext, ref object state) {
            var user = OperationContext.Current.IncomingMessageProperties["Principal"] as IPrincipal;

            if (user != null) {
                evaluationContext.Properties["Identities"] = new List<IIdentity> { user.Identity };
            }
            evaluationContext.Properties["Principal"] = user;

            return false;
        }

        public ClaimSet Issuer {
            get {
                return ClaimSet.System;
            }
        }

        public string Id {
            get {
                return this.id.ToString();
            }
        }

        #endregion
    }

}
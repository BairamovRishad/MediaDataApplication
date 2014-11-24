using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace MediaDataApplication.AspNetMvcClient.CredentialMessageInspector {

    public class CredentialsInspector : IClientMessageInspector {
        private const string PASSWORD = "654321";
        private const string USER_NAME = "ASP.NET MVC";

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState) { }

        public object BeforeSendRequest(ref Message request, IClientChannel channel) {
            var messageHeader = new MessageHeader<string>(USER_NAME + "," + PASSWORD);
            request.Headers.Add(messageHeader.GetUntypedHeader("ClientCredentials", "ns"));
            return null;
        }

        #endregion
    }

}
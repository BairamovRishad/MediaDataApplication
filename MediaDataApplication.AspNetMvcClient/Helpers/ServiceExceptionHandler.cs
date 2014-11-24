using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using MediaDataApplication.AspNetMvcClient.MediaDataServiceReference;
using NLog;

namespace MediaDataApplication.AspNetMvcClient.Helpers {

    internal sealed class ServiceExceptionHandler {
        private readonly Logger logger;
        private string message;

        public ServiceExceptionHandler() : this(LogManager.GetCurrentClassLogger()) { }

        public ServiceExceptionHandler(Logger logger) {
            this.logger = logger;
        }

        public string Message {
            get {
                return this.message;
            }
        }

        public void Handle(Exception e) {
            this.message = String.Empty;

            if (e is TimeoutException) {
                this.message += "The service operation timed out. " + e.Message;
            }
            else if (e is FaultException<MediaFault>) {
                this.message += "MediaFault returned: " + (e as FaultException<MediaFault>).Detail.FaultMessage;
            }
            else if (e is FaultException<UserFault>) {
                this.message += "UserFault returned: " + (e as FaultException<UserFault>).Detail.FaultMessage;
            }
            else if (e is FaultException) {
                this.message += "Unknown Fault: " + e;

                MessageFault mf = (e as FaultException).CreateMessageFault();
                if (mf.HasDetail) {
                    this.message += mf.GetDetail<string>();
                }
            }
            else if (e is CommunicationException) {
                this.message += "There was a communication problem. " + e.Message + e.StackTrace;
            }
            else {
                this.message += "Other exception: " + e.Message + e.StackTrace;
            }
            this.logger.Error(this.message);
        }
    }

}
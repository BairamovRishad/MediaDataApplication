using System;
using System.ServiceModel.Configuration;

namespace MediaDataApplication.AspNetMvcClient.CredentialMessageInspector {

    public class InspectorInserter : BehaviorExtensionElement {
        public override Type BehaviorType {
            get {
                return typeof(CustomInspectorBehaviorExtension);
            }
        }

        protected override object CreateBehavior() {
            return new CustomInspectorBehaviorExtension();
        }
    }

}
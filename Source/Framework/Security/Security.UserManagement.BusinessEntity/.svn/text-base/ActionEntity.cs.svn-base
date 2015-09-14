using System.Runtime.Serialization;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class ActionEntity
    {
        public ActionEntity() { }

        public ActionEntity
        (
            string actionID,
            string actionCode,
            string actionDesc
        )
        {
            ActionID = actionID;
            ActionCode = actionCode;
            ActionDesc = actionDesc;
        }
                
        [DataMember]
        public string ActionID
        {
            get;
            set;
        }

        [DataMember]
        public string ActionCode
        {
            get;
            set;
        }

        [DataMember]
        public string ActionDesc
        {
            get;
            set;
        }

    }
}
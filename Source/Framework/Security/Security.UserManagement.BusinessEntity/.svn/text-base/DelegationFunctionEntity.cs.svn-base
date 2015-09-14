using System.Runtime.Serialization;

namespace HiiP.Framework.Security.UserManagement.BusinessEntity
{
    [DataContract(Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public class DelegationFunctionEntity
    {
        public DelegationFunctionEntity() { }

        public DelegationFunctionEntity
        (
            int functionID,
            string name,
            string desc
        )
        {
            DelegationFunctionId = functionID;
            Name = name;
            Description = desc;
        }

        [DataMember]
        public int DelegationFunctionId
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

    }
}

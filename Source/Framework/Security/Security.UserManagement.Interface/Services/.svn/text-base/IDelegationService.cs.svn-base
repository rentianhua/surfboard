using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using NCS.IConnect.Common;
using HiiP.Framework.Security.UserManagement.BusinessEntity;


namespace HiiP.Framework.Security.UserManagement.Interface
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed, Namespace = "http://hiip.ooh.dhs.vic.gov.au/")]
    public interface IDelegationService
    {
        /// <summary>
        ///  Retrieve all Delegation functions
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<DelegationFunctionEntity> GetAllDelegationFunctions();


        /// <summary>
        ///  Retrieve all Delegation Detail Entity
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<DelegationValueEntity> GetAllDelegationValueEntity();


        /// <summary>
        ///  Find Delegation Value Entity
        /// </summary>
        /// <param name="objDelegationSearchCriteria"></param>        
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<DelegationValueEntity> FindDelegationValueEntity(DelegationSearchCriteria objDelegationSearchCriteria);


        /// <summary>
        ///  Search Delegation Value Entity By DelegationValueID
        /// </summary>
        /// <param name="delegationValueID"></param>        
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        DelegationValueEntity SearchDelegationValueByID(int delegationValueID);
                

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void UpdateDelegationValue(DelegationValueEntity objDelegationValueEntity);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        void InsertDelegationValue(DelegationValueEntity objDelegationValueEntity);


        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<CodeTableEntity> GetAllAct();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllBranch();

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllUnit(string organisationalUnitID);


        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        LookupOrganisationalUnitDataSet.LookupOrganisationalUnitDataTable GetAllSubUnit(string organisationalUnitID);

        /// <summary>
        ///  Return financial limit/Retrieve Delegation values
        /// </summary>
        /// <param name="delegationFunctionId">Delegation function (e.g. “Purchase Land”)</param>
        /// <param name="userName">Logged-in user (framework retrieves user’s branch / unit / subunit / grade )</param>
        /// <param name="act">Act (e.g. “Housing”)</param>
        /// <returns>Return financial limit for user</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string GetDelegationValues(int delegationFunctionId, string userName, string act);


        /// <summary>
        ///  Return financial limit/Retrieve Delegation values
        /// </summary>
        /// <param name="delegationFunctionName">Delegation function name (e.g. “Purchase Land”)</param>
        /// <param name="userName">Logged-in user (framework retrieves user’s branch / unit / subunit / grade )</param>
        /// <param name="act">Act (e.g. “Housing”)</param>
        /// <returns>Return financial limit for user</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        string GetDelegationValuesByName(string delegationFunctionName, string userName, string act);


        /// <summary>
        ///  Return list of users that have delegated financial rights to perform approval/rejection
        /// </summary>
        /// <param name="delegationFunctionId">Delegation function (e.g. “Purchase Land”)</param>      
        /// <param name="act">Act (e.g. “Housing”)</param>
        /// <param name="financialAmount"></param>
        /// <returns>Return list of users that have delegated financial rights to perform approval/rejection</returns>
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(ServiceException))]
        List<string> GetUsersByFinancialAmount(int delegationFunctionId, string act, double financialAmount);


    }
}

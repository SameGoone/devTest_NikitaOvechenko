using System;
using BPMSoft.Core.Factories;
using BPMSoft.Core;
using BPMSoft.Web.Common;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Reflection;
using Common.Logging;
using BPMSoft.Common;
using BPMSoft.Configuration;
using BPMSoft.Core.Entities;

[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class OvDevTestService : BaseService
{
    protected readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType.Name);

    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
    public virtual int GetAccountsWithNamePartCount(string namePart)
    {
        Argument.EnsureNotNullOrEmpty(namePart, nameof(namePart));

        var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, nameof(Account));
        esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Contain, nameof(Account.Name), namePart));
        var countColumn = esq.AddColumn(esq.CreateAggregationFunction(AggregationTypeStrict.Count, "Id"));

        var countEntity = esq.GetEntityCollection(UserConnection)[0];
        var count = countEntity.GetTypedColumnValue<int>(countColumn.Name);

        return count;
    }
}
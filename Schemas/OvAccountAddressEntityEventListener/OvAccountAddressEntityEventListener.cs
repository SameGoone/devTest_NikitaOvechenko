using BPMSoft.Common;
using BPMSoft.Configuration;
using BPMSoft.Core;
using BPMSoft.Core.Entities;
using BPMSoft.Core.Entities.Events;
using Common.Logging;
using System;
using System.Reflection;

[EntityEventListener(SchemaName = nameof(AccountAddress))]
public class AccountAddressEntityEventListener : BaseEntityEventListener
{
    protected UserConnection UserConnection;

    protected Entity Entity;

    protected readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType.Name);

    protected virtual void InitParameters(object sender)
    {
        Entity = (Entity)sender;
        UserConnection = Entity.UserConnection;
    }

    public override void OnSaving(object sender, EntityBeforeEventArgs e)
    {
        InitParameters(sender);

        // Перед записью в БД очищаем тип, если адрес с таким типом существует у Контрагента.
        if (CheckAddressWithTypeExists())
        {
            ClearAddressType();
        }

        base.OnSaving(sender, e);
    }

    protected virtual bool CheckAddressWithTypeExists()
    {
        var typeId = Entity.GetTypedColumnValue<Guid>(nameof(AccountAddress.AddressTypeId));
        var accountId = Entity.GetTypedColumnValue<Guid>(nameof(AccountAddress.AccountId));

        var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, nameof(AccountAddress));
        esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, nameof(AccountAddress.AddressType), typeId));
        esq.Filters.Add(esq.CreateFilterWithParameters(FilterComparisonType.Equal, nameof(AccountAddress.Account), accountId));
        var countColumn = esq.AddColumn(esq.CreateAggregationFunction(AggregationTypeStrict.Count, "Id"));

        var countEntity = esq.GetEntityCollection(UserConnection)[0];
        var count = countEntity.GetTypedColumnValue<int>(countColumn.Name);

        return count > 0;
    }

    protected virtual void ClearAddressType()
    {
        Entity.SetColumnValue(nameof(AccountAddress.AddressTypeId), null);
    }
}
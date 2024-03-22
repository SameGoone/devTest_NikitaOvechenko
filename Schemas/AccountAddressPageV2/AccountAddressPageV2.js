define("AccountAddressPageV2", [], function () {
	return {
		entitySchemaName: "AccountAddress",
		attributes: {},
		modules: /**SCHEMA_MODULES*/{}/**SCHEMA_MODULES*/,
		details: /**SCHEMA_DETAILS*/{}/**SCHEMA_DETAILS*/,
		businessRules: /**SCHEMA_BUSINESS_RULES*/{}/**SCHEMA_BUSINESS_RULES*/,
		methods: {
			/**
			 * Переопределение базового метода "save".
			 * @overridden
			 */
			// save: function () {
			// 	const parentMethod = this.getParentMethod(this, arguments);
			// 	this.ensureAddressWithTypeNotExists(parentMethod);
			// },

			/**
			 * Выполняет проверку, имеется ли адрес с таким же типом у Контрагента.
			 */
			ensureAddressWithTypeNotExists: function (callback) {
				const type = this.get("AddressType");
				const account = this.get("Account");

				const esq = this.Ext.create("BPMSoft.EntitySchemaQuery", {
					rootSchemaName: "AccountAddress",
					rowCount: 1
				});

				esq.addAggregationSchemaColumn(
					"Id", BPMSoft.AggregationType.COUNT, "AddressCount"
				);

				esq.filters.addItem(esq.createColumnFilterWithParameter(
					BPMSoft.ComparisonType.EQUAL,
					"Account",
					account?.value));

				esq.filters.addItem(esq.createColumnFilterWithParameter(
					BPMSoft.ComparisonType.EQUAL,
					"AddressType",
					type?.value));

				esq.getEntityCollection(function (response) {
					if (response && response.success) {
						const item = response.collection.getItems()[0];

						if (item.get("AddressCount") > 0) {
							const message = this.get("Resources.Strings.AddressWithTypeAlreadyExistsMessage");
							this.BPMSoft.showInformation(message);
						} else {
							callback.call();
						}
					}
				}, this);
			}
		},
		dataModels: /**SCHEMA_DATA_MODELS*/{}/**SCHEMA_DATA_MODELS*/,
		diff: /**SCHEMA_DIFF*/[]/**SCHEMA_DIFF*/
	};
});
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace MyPlugin
{
    public class CheckCustomerBalancePluginIPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // Verificar se o contexto contém o registro de destino
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity targetEntity = (Entity)context.InputParameters["Target"];

                if (targetEntity.LogicalName == "account")
                {
                    Guid accountId = targetEntity.Id;

                    QueryExpression query = new QueryExpression("account")
                    {
                        ColumnSet = new ColumnSet("name", "new_balance")
                    };
                    query.Criteria.AddCondition("accountid", ConditionOperator.Equal, accountId);

                    EntityCollection results = service.RetrieveMultiple(query);

                    if (results.Entities.Count > 0)
                    {
                        Entity account = results.Entities[0];
                        decimal balance = account.Contains("new_balance") ? (decimal)account["new_balance"] : 0;

                        context.OutputParameters["CustomerBalance"] = balance;
                    }
                }
            }
        }
    }
}
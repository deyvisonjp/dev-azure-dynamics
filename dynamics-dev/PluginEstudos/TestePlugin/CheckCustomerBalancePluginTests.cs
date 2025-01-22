using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using MyPlugin;

namespace TestePlugin;
public class CheckCustomerBalancePluginTests
{
    private Mock<IPluginExecutionContext> pluginContext;
    private Mock<IServiceProvider> serviceProvider;
    private Mock<IOrganizationService> organizationService;
    private Mock<IOrganizationServiceFactory> serviceFactory;
    private CheckCustomerBalancePluginIPlugin plugin;

    [SetUp]
    public void Setup()
    {
        pluginContext = new Mock<IPluginExecutionContext>();
        pluginContext.Setup(c => c.InputParameters).Returns(new ParameterCollection
            {
                { "Target", new Entity("account") { Id = Guid.NewGuid() } }
            });
        pluginContext.Setup(c => c.OutputParameters).Returns(new ParameterCollection());

        organizationService = new Mock<IOrganizationService>();
        serviceFactory = new Mock<IOrganizationServiceFactory>();
        serviceFactory.Setup(f => f.CreateOrganizationService(It.IsAny<Guid>())).Returns(organizationService.Object);

        serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(pluginContext.Object);
        serviceProvider.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(serviceFactory.Object);

        // Instanciando o plugin
        plugin = new CheckCustomerBalancePluginIPlugin();
    }

    [Test]
    public void TestCheckCustomerBalance()
    {
        // Configurar a resposta da consulta
        var account = new Entity("account")
        {
            Id = ((Entity)pluginContext.Object.InputParameters["Target"]).Id
        };
        account["new_balance"] = 1000m;

        organizationService.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
            .Returns(new EntityCollection(new[] { account }));

        plugin.Execute(serviceProvider.Object);

        Assert.AreEqual(1000m, pluginContext.Object.OutputParameters["CustomerBalance"]);
    }
}

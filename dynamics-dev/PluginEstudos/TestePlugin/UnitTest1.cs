using Moq;
using Microsoft.Xrm.Sdk;
using MyPlugin;

namespace TestePlugin
{
    public class Tests
    {
        private Mock<IPluginExecutionContext> pluginContext;
        private Mock<IServiceProvider> serviceProvider;
        private SimplePlugin plugin;

        [SetUp]
        public void Setup()
        {
            pluginContext = new Mock<IPluginExecutionContext>();
            pluginContext.Setup(c => c.InputParameters).Returns(new ParameterCollection
            {
                { "x", 3 },
                { "y", 4 },
                { "z", 5 }
            });

            pluginContext.Setup(c => c.OutputParameters).Returns(new ParameterCollection());

            serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(pluginContext.Object);

            plugin = new SimplePlugin();
        }

        [Test]
        public void TestSimplePlugin()
        {
            // Act
            plugin.Execute(serviceProvider.Object);

            // Assert
            var expectedJson = "{\"X\":3,\"Y\":4,\"Z\":5,\"Calculation\":\"Sum\",\"Result\":12}";
            Assert.AreEqual(expectedJson, pluginContext.Object.OutputParameters["ResultJson"]);
        }
    }
}

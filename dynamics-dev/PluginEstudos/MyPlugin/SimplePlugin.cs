using Microsoft.Xrm.Sdk;

namespace MyPlugin
{
    public class SimplePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("x") && context.InputParameters["x"] is int
            && context.InputParameters.Contains("y") && context.InputParameters["y"] is int
            && context.InputParameters.Contains("z") && context.InputParameters["z"] is int)
            {
                int x = (int)context.InputParameters["x"];
                int y = (int)context.InputParameters["y"];
                int z = (int)context.InputParameters["z"];

                int result = x + y + z; // Exemplo de cálculo: soma dos valores

                var jsonResponse = new
                {
                    X = x,
                    Y = y,
                    Z = z,
                    Calculation = "Sum",
                    Result = result
                };

                context.OutputParameters["ResultJson"] = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse);
            }
            else
            {
                throw new InvalidPluginExecutionException("Input parameters are missing or invalid.");
            }
        }
    }
}

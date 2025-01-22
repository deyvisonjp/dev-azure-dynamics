using AzFunctionsExample.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Refit;

//Extraido de:
//https://www.youtube.com/@DjesusNet/videos

namespace AzFunctionsExample
{
    public class FunctionHttpTestes
    {
        private readonly ILogger<FunctionHttpTestes> _logger;
        private IEstoqueService _estoqueService;

        public FunctionHttpTestes(ILogger<FunctionHttpTestes> logger)
        {
            _logger = logger;
            _estoqueService = RestService.For<IEstoqueService>("http://localhost:3333");
        }

        [Function("Get-Products")]
        public async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {
            _logger.LogInformation("Está sendo processado a function Get-products!");

            try
            {
                var products = await _estoqueService.GetProductsAsync().ConfigureAwait(false);
                return new OkObjectResult(products);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "erro ao chamar a função API");
                return new StatusCodeResult((int)ex.StatusCode);
            }
        }

        #region
        //Aqui poderia ser uma Timer Trigger que fizesse a chamada de 24h
        [Function("Get-NewProducts")]
        public async Task<IActionResult> GetNewProducts(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/produtosNovos")] HttpRequest req)
        {
            _logger.LogInformation("Está sendo processado a function Get-NewProducts!");

            try
            {
                var products = await _estoqueService.GetProductsAsync().ConfigureAwait(false);
                var now = DateTime.UtcNow;
                var lastHours = now.AddHours(-24);

                var novosProdutos = products.Where(product => product.created_at >= lastHours).ToList();

                return new OkObjectResult(novosProdutos);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Erro ao chamar a função API");
                return new StatusCodeResult((int)ex.StatusCode);
            }
        }
        #endregion
    }
}

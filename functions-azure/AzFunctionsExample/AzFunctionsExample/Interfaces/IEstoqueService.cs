using AzFunctionsExample.Models;
using Refit;

namespace AzFunctionsExample.Interfaces;

public interface IEstoqueService
{
    [Get("/products")]
    Task<IEnumerable<Product>>GetProductsAsync();
}

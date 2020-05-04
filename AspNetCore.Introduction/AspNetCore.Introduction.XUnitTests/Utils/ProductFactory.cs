using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Introduction.Models;
using AutoFixture;

namespace AspNetCore.Introduction.XUnitTests.Utils
{
    public static class ProductsFactory
    {
        private static readonly Fixture Fixture;

        static ProductsFactory()
        {
            Fixture = new Fixture();


            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));
        }

        public static Products Product()
        {
            var result = Fixture
                .Create<Products>();
            return result;
        }

        public static Task<Products> ProductAsync()
        {
            var result = Fixture
                .Create<Task<Products>>();
            return result;
        }

        public static async Task<IEnumerable<Products>> GetTwoProductsAsync()
        {
            var products = new List<Products>
            {
                await ProductAsync(),
                await ProductAsync()
            };

            return await Task.FromResult(products);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Introduction.Models;
using AutoFixture;

namespace AspNetCore.Introduction.XUnitTests.Utils
{
    public static class CategoryFactory
    {
        private static readonly Fixture Fixture;

        static CategoryFactory()
        {
            Fixture = new Fixture();


            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior(recursionDepth: 1));
        }

        public static Categories Category()
        {
            var result = Fixture
                .Create<Categories>();
            return result;
        }

        public static Task<Categories> CategoryAsync()
        {
            var result = Fixture
                .Create<Task<Categories>>();
            return result;
        }

        public static async Task<IEnumerable<Categories>> GetTwoCategoriesAsync()
        {
            var categories = new List<Categories>
            {
                await CategoryAsync(),
                await CategoryAsync()
            };

            return await Task.FromResult(categories);
        }
    }
}
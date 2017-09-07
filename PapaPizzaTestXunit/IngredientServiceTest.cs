using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using PapaPizza.Data;
using PapaPizza.Services;

namespace PapaPizzaTestXunit
{
    public class IngredientServiceTest
    {
        private readonly IServiceProvider _serviceProvider;

        public IngredientServiceTest()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b =>
            b.UseInMemoryDatabase("Pizzadatabas")
            .UseInternalServiceProvider(efServiceProvider));
            services.AddTransient<IngredientService>();

            _serviceProvider = services.BuildServiceProvider();

        }

        [Fact]
        public void AllAreSorted()
        {
            var _ingredients = _serviceProvider.GetService<IngredientService>();
            var ings = _ingredients.GetIngredients();
            Assert.Equal(ings.Count, 0);
        }
    }
}

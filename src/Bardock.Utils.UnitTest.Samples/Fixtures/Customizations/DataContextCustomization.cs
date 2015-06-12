﻿using Bardock.Utils.UnitTest.Data.EF.Effort.DataLoaders;
using Bardock.Utils.UnitTest.Samples.Fixtures.DataLoaders;
using Bardock.Utils.UnitTest.Samples.SUT.Infra;
using Ploeh.AutoFixture;

namespace Bardock.Utils.UnitTest.Samples.Fixtures.Customizations
{
    internal class DataContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var loader = new EntityObjectDataLoader(conf => conf
                .Add<CountriesDataLoader>()
                .Add<AddressesDataLoader>()
                .Add<CustomersDataLoader>());

            var connection = Effort.DbConnectionFactory.CreateTransient(loader);

            var db = new DataContext(connection);

            fixture.Register(() => db);
        }
    }
}
﻿using Bardock.Utils.Extensions;
using Bardock.Utils.UnitTest.AutoFixture.EF.Helpers;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System.Data.Entity;
using System.Reflection;

namespace Bardock.Utils.UnitTest.AutoFixture.EF.Customizations
{
    public class IgnoreEntityNavigationPropsCustomization<TDbContext> : ICustomization
        where TDbContext : DbContext
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreEntityNavigationPropsSpecimenBuilder<TDbContext>());
        }
    }

    public class IgnoreEntityNavigationPropsSpecimenBuilder<TDbContext> : ISpecimenBuilder
        where TDbContext : DbContext
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen(request);
            }

            if (!pi.PropertyType.IsPrimitive()
                && pi.GetGetMethod().IsVirtual
                && pi.DeclaringType.IsMappedEntity<TDbContext>())
            {
                return null;
            }
            return new NoSpecimen(request);
        }
    }
}
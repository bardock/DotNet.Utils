﻿using AutoMapper;
using Bardock.Utils.UnitTest.AutoFixture.Customizations;
using System;
using System.Linq;

namespace Bardock.Utils.UnitTest.AutoFixture.AutoMapper.Customizations
{
    public class AutoMapMembersCustomization : MapMembersCustomization
    {
        public AutoMapMembersCustomization(Type sourceType, Type destinationType)
            : base(
                sourceType,
                destinationType,
                mappings: Mapper.GetAllTypeMaps()
                            .Where(m => m.SourceType == sourceType)
                            .Where(m => m.DestinationType == destinationType)
                            .SelectMany(m => m.GetPropertyMaps())
                            .Select(p => new MemberMapping(p.SourceMember, p.DestinationProperty.MemberInfo)))
        { }
    }

    public class AutoMapMembersCustomization<TSource, TDestination> : AutoMapMembersCustomization
    {
        public AutoMapMembersCustomization()
            : base(typeof(TSource), typeof(TDestination))
        { }
    }
}
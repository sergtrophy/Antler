﻿// ReSharper disable InconsistentNaming

using System.Linq;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.SqlServer.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.EntityFramework.Configuration;
using SmartElk.Antler.Specs.Shared.EntityFramework.Mappings;
using SmartElk.Antler.Windsor;

namespace Antler.EntityFramework.SqlServer.Specs
{    
    public class DomainSpecs
    {                                                        
        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_one_employee : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_get_one_employee.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_all_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_all_teams()
            {
                CommonDomainSpecs.when_trying_to_get_all_teams.should_return_all_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_all_employees : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_all_employees()
            {
                CommonDomainSpecs.when_trying_to_get_all_employees.should_return_all_employees();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_employee_by_name : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_find_employee_by_name.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_modify_employees_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_modify_teams()
            {
                CommonDomainSpecs.when_trying_to_modify_employees_teams.should_modify_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_team_by_country_name : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_country()
            {
                CommonDomainSpecs.when_trying_to_find_team_by_country_name.should_return_country();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_delete_team_by_id : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_delete_team()
            {
                CommonDomainSpecs.when_trying_to_delete_team_by_id.should_delete_team();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_one_employee_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                SmartElk.Antler.Specs.Shared.EntityFramework.CommonSpecs.CommonDomainSpecs.when_trying_to_get_one_employee_without_lazy_loading.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_team_by_country_name_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public void should_return_country()
            {
                SmartElk.Antler.Specs.Shared.EntityFramework.CommonSpecs.CommonDomainSpecs.when_trying_to_find_team_by_country_name_without_lazy_loading.should_return_country();
            }
        }

        #region Configuration
        public class LazyLoading { }
        public class EagerLoading { }
        public class TestingScenario<T>
        {
            protected IAntlerConfigurator Configurator { get; set; }

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();

                const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True";
                var assemblyWithMappings = From.AssemblyWithType<CountryMap>().First();
                Configurator.UseWindsorContainer()
                            .UseStorage(typeof(T) == typeof(LazyLoading)
                                            ? EntityFrameworkPlusSqlServer.Use.WithConnectionString(connectionString)
                                                                      .WithMappings(assemblyWithMappings)
                                            : EntityFrameworkPlusSqlServer.Use.WithoutLazyLoading()
                                                                      .WithConnectionString(connectionString)
                                                                      .WithMappings(assemblyWithMappings));

                Configurator.ClearDatabase();                                
            }

            [TearDown]
            public void Dispose()
            {
                Configurator.UnUseWindsorContainer().UnUseStorage().Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming
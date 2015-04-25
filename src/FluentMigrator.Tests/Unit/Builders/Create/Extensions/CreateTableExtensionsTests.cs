using System;
using NUnit.Framework;
using FluentMigrator.Builders.Create.Table;
using Moq;
using FluentMigrator.Model;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner.Extensions.Redshift;
using System.Collections.Generic;

namespace FluentMigrator.Tests.Unit.Builders.Create.Extensions
{
    [TestFixture]
    public class CreateTableExtensionsTests
    {
        CreateTableExpressionBuilder Builder;      
        CreateTableExpression Expression;
        Mock<IMigrationContext> ContextMock;

        [SetUp]
        public void Setup()
        {
            Expression = new CreateTableExpression();
            ContextMock = new Mock<IMigrationContext>();

            Builder = new CreateTableExpressionBuilder(Expression, ContextMock.Object);

        }

        [Test]
        public void WithDistStyleAddsToAddtionalFeature()
        {
            Builder.WithDistStyle(DistStyle.All);

            Assert.That(Expression.AdditionalFeatures[RedshiftExtensions.DistStyleKey], Is.EqualTo(DistStyle.All)); 
        }
    }
}


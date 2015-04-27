using System;
using NUnit.Framework;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Create.Column;
using Moq;
using FluentMigrator.Model;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner.Extensions;
using System.Collections.Generic;

namespace FluentMigrator.Tests.Unit.Builders.Create.Extensions
{
    [TestFixture]
    public class RedshiftExtensionsTests
    {
        CreateTableExpressionBuilder TableBuilder;      
        CreateTableExpression TableExpression;
        Mock<IMigrationContext> ContextMock;

        [SetUp]
        public void Setup()
        {
            TableExpression = new CreateTableExpression();
            ContextMock = new Mock<IMigrationContext>();

            TableBuilder = new CreateTableExpressionBuilder(TableExpression, ContextMock.Object);

        }

        [Test]
        public void WithDistStyleAddsToAddtionalFeature()
        {
            TableBuilder.WithDistStyle(DistStyle.All);

            Assert.That(TableExpression.AdditionalFeatures[RedshiftExtensions.DistStyleKey], Is.EqualTo(DistStyle.All)); 
        }

        [Test]
        public void SortKeyAddsToAdditionalFeature()
        {
            TableBuilder.SortKey();

            Assert.That(TableBuilder.CurrentColumn.AdditionalFeatures[RedshiftExtensions.ColumnSortKeyKey], Is.EqualTo(true)); 
        }
    }
}


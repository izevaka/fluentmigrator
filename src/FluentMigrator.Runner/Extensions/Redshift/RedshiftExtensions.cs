using System;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Runner.Generators.Redshift;
using FluentMigrator.Infrastructure;

namespace FluentMigrator.Runner.Extensions.Redshift
{
    public enum DistStyle
    {
        Even,
        Key,
        All,
    }

    public static class RedshiftExtensions
    {
        public const string DistStyleKey = "DistStyle";

        public static ICreateTableWithColumnOrSchemaOrDescriptionSyntax WithDistStyle(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax expression, DistStyle style) 
        {
            var builder = expression as ISupportAdditionalFeatures;
            if (builder == null)
            {
                return expression;
            }

            builder.AddAdditionalFeature(DistStyleKey, style);
            return expression;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator.Expressions;
using FluentMigrator.Model;
using FluentMigrator.Runner.Generators.Generic;
using FluentMigrator.Runner.Generators.PostgresBase;
using FluentMigrator.Runner.Extensions;

namespace FluentMigrator.Runner.Generators.Redshift
{
    public class RedshiftGenerator : PostgresBaseGenerator
    {
        public RedshiftGenerator()
            : base(new RedshiftColumn(), new RedshiftQuoter(), new RedshiftDescriptionGenerator())
        {
        }

        public override string Generate(CreateIndexExpression expression)
        {
            return string.Empty;
        }

        public override string Generate(DeleteIndexExpression expression)
        {
            return string.Empty;
        }

        public override string Generate(CreateSequenceExpression expression)
        {
            return string.Empty;
        }

        public override string Generate(DeleteSequenceExpression expression)
        {
            return string.Empty;
        }

        public override string Generate(CreateTableExpression expression)
        {
            var createStatement = new StringBuilder();
            var tableName = Quoter.QuoteTableName(expression.TableName);
            createStatement.Append(string.Format("CREATE TABLE {0}.{1} ({2})", Quoter.QuoteSchemaName(expression.SchemaName), tableName, Column.Generate(expression.Columns, tableName)));

            object distStyleObj;
            if (expression.AdditionalFeatures.TryGetValue(RedshiftExtensions.DistStyleKey, out distStyleObj))
            {
                if (distStyleObj != null)
                {   
                    var distStyle = (DistStyle)distStyleObj;
                    createStatement.Append(string.Format(" DISTSTYLE {0}", distStyle.ToString().ToUpper())); 
                }
            }

            var descriptionStatement = DescriptionGenerator.GenerateDescriptionStatements(expression);
            if (descriptionStatement != null && descriptionStatement.Any())
            {
                createStatement.Append(";");
                createStatement.Append(string.Join(";", descriptionStatement.ToArray()));
            }
            return createStatement.ToString();
        }
    }
}

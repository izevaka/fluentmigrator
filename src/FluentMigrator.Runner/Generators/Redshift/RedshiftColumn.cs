﻿using System;
using System.Collections.Generic;
using System.Data;
using FluentMigrator.Model;
using FluentMigrator.Runner.Generators.Base;

namespace FluentMigrator.Runner.Generators.Redshift
{
    internal class RedshiftColumn : ColumnBase
    {
        public RedshiftColumn() : base(new RedshiftTypeMap(), new RedshiftQuoter())
        {
            AlterClauseOrder = new List<Func<ColumnDefinition, string>> { FormatAlterType, FormatAlterNullable };
        }

        public RedshiftColumn(ITypeMap typeMap, IQuoter quoter) : base (typeMap, quoter) 
        {
        }

        public string FormatAlterDefaultValue(string column, object defaultValue)
        {
            string formatDefaultValue = FormatDefaultValue(new ColumnDefinition { Name = column, DefaultValue = defaultValue});

            return string.Format("SET {0}", formatDefaultValue);
        }

        private string FormatAlterNullable(ColumnDefinition column)
        {
            if (!column.IsNullable.HasValue)
                return "";

            if (column.IsNullable.Value)
                return "DROP NOT NULL";

            return "SET NOT NULL";
        }

        private string FormatAlterType(ColumnDefinition column)
        {
            return string.Format("TYPE {0}", GetColumnType(column));
        }

        protected IList<Func<ColumnDefinition, string>> AlterClauseOrder { get; set; }

        public string GenerateAlterClauses(ColumnDefinition column)
        {
            var clauses = new List<string>();
            foreach (var action in AlterClauseOrder)
            {
                string columnClause = action(column);
                if (!string.IsNullOrEmpty(columnClause))
                    clauses.Add(string.Format("ALTER {0} {1}", Quoter.QuoteColumnName(column.Name), columnClause));
            }

            return string.Join(", ", clauses.ToArray());
        }

        protected override string FormatIdentity(ColumnDefinition column)
        {
            return string.Empty;
        }

        public override string AddPrimaryKeyConstraint(string tableName, IEnumerable<ColumnDefinition> primaryKeyColumns)
        {
            string pkName = GetPrimaryKeyConstraintName(primaryKeyColumns, tableName);

            string cols = string.Empty;
            bool first = true;
            foreach (var col in primaryKeyColumns)
            {
                if (first)
                    first = false;
                else
                    cols += ",";
                cols += Quoter.QuoteColumnName(col.Name);
            }

            if (string.IsNullOrEmpty(pkName))
                return string.Format(", PRIMARY KEY ({0})", cols);

            return string.Format(", {0}PRIMARY KEY ({1})", pkName, cols);
        }

        protected override string FormatSystemMethods(SystemMethods systemMethod)
        {
            switch (systemMethod)
            {
            case SystemMethods.CurrentDateTime:
                return "now()";
            case SystemMethods.CurrentUTCDateTime:
                return "(now() at time zone 'UTC')";
            case SystemMethods.CurrentUser:
                return "current_user";
            }

            throw new NotImplementedException(string.Format("System method {0} is not implemented.", systemMethod));
        }

        public string GetColumnType(ColumnDefinition column)
        {
            return FormatType(column);
        }
    }
}
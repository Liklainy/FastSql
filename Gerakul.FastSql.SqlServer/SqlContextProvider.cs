﻿using Gerakul.FastSql.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gerakul.FastSql.SqlServer
{
    public class SqlContextProvider : ContextProvider
    {
        public static SqlContextProvider Instance { get; } = new SqlContextProvider();

        private SqlContextProvider()
        {
        }

        public override CommandTextGenerator CommandTextGenerator { get; } = new SqlCommandTextGenerator();
        public override QueryOptions DefaultQueryOptions { get; } = new SqlQueryOptions();
        public override BulkOptions DefaultBulkOptions { get; } = new SqlBulkOptions();

        public override ConnectionStringContext CreateConnectionStringContext(string connectionString)
        {
            return new SqlConnectionStringContext(this, connectionString);
        }

        public override ConnectionContext CreateConnectionContext(DbConnection connection)
        {
            return new SqlConnectionContext(this, (SqlConnection)connection);
        }

        public override TransactionContext CreateTransactionContext(DbTransaction transaction)
        {
            return new SqlTransactionContext(this, (SqlTransaction)transaction);
        }

        //protected override void ApplyQueryOptions(DbCommand cmd, QueryOptions queryOptions)
        //{
        //    base.ApplyQueryOptions(cmd, queryOptions);
        //}

        protected override DbParameter AddParamWithValue(DbCommand cmd, string paramName, object value)
        {
            return ((SqlCommand)cmd).Parameters.AddWithValue(paramName, value);
        }

        protected override object GetDbNull(Type type)
        {
            if (type.Equals(typeof(byte[])))
            {
                return System.Data.SqlTypes.SqlBytes.Null;
            }

            return DBNull.Value;
        }

        protected override string GetSqlParameterName(string name)
        {
            return name[0] == '@' ? name.Substring(1) : name;
        }

        private static Regex regName = new Regex("(([^@]@)|(^@))(?<Name>([a-z]|[A-Z]|[0-9]|[$#_])+)", RegexOptions.Multiline);
        protected override string[] ParseCommandText(string commandText)
        {
            return regName.Matches(commandText).Cast<Match>()
              .Select(x => "@" + x.Groups["Name"].ToString().ToLowerInvariant())
              .Distinct().ToArray();
        }

        public static SqlConnectionStringContext FromConnectionString(string connectionString)
        {
            return new SqlConnectionStringContext(Instance, connectionString);
        }

        public static SqlConnectionContext FromConnection(SqlConnection connection)
        {
            return new SqlConnectionContext(Instance, connection);
        }

        public static SqlTransactionContext FromTransaction(SqlTransaction transaction)
        {
            return new SqlTransactionContext(Instance, transaction);
        }

        protected override BulkCopy GetBulkCopy(ScopedContext context, BulkOptions bulkOptions, string destinationTable, DbDataReader reader, params string[] fields)
        {
            return new MsSqlBulkCopy(context, SqlBulkOptions.FromBulkOptions(bulkOptions), destinationTable, reader, fields);
        }
    }
}

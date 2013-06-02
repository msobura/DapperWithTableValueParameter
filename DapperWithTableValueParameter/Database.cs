using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.SqlServer.Server;

namespace DapperWithTableValueParameter
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<int> GetInts(IEnumerable<int> numbers)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var dynamicParam = new IntListDynamicParam("@idList", numbers);

                var list = conn
                    .Query<int>("TestProcedure", dynamicParam, commandType: CommandType.StoredProcedure)
                    .ToList();

                return list;
            }
        }

        // NOTE:
        // Currently there no possibility to mix standard parameters with user defined table type. It can be one of them at a time.

        private class IntListDynamicParam : SqlMapper.IDynamicParameters
        {
            private readonly SqlMetaData[] definition =
            {
                new SqlMetaData("Id", SqlDbType.Int),
            };

            private readonly string parameterName;
            private readonly IEnumerable<int> numbers;

            public IntListDynamicParam(string parameterName, IEnumerable<int> numbers)
            {
                this.parameterName = parameterName;
                this.numbers = new List<int>(numbers);
            }

            public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
            {
                var records = new List<SqlDataRecord>(numbers.Count());
                foreach (var number in numbers)
                {
                    var record = new SqlDataRecord(definition);
                    record.SetInt32(0, number);
                    records.Add(record);
                }

                var sqlCommand = (SqlCommand)command;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                var parameter = sqlCommand.Parameters.Add(parameterName, SqlDbType.Structured);
                parameter.Direction = ParameterDirection.Input;
                parameter.TypeName = "udttIdList";
                parameter.Value = records;
            }
        }
    }
}
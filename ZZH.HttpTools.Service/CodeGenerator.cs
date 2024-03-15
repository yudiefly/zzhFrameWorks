using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Data.SqlClient;

namespace ZZH.HttpTools.Service
{

    /// <summary>
    /// 代码生成类
    /// </summary>
    public class CodeGenerator
    {
        private readonly string _connectionString;

        public CodeGenerator(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GenerateEntityClass(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public class {tableName}");
            sb.AppendLine("{");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            int? maxLength = reader["CHARACTER_MAXIMUM_LENGTH"] as int?;

                            // Map SQL Server data types to C# types  
                            string csharpType = MapDataTypeToCSharp(dataType, maxLength);

                            sb.AppendLine($"    public {csharpType} {columnName} {{ get; set; }}");
                        }
                    }
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }


        private string MapDataTypeToCSharp(string sqlType, int? maxLength)
        {
            switch (sqlType.ToLower())
            {
                case "int":
                    return "int";
                case "bigint":
                    return "long";
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    return maxLength.HasValue && maxLength <= 4000 ? "string" : "string"; // For simplicity, assuming varchar(max) and similar types are still string in C#  
                case "bit":
                    return "bool";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "datetime":
                case "smalldatetime":
                    return "DateTime";
                // Add more mappings as needed...  
                default:
                    throw new NotImplementedException($"Data type {sqlType} is not mapped to a C# type.");
            }
        }

    }

    public class ColumnInfo
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public string ColumnComment { get; set; }
    }
}

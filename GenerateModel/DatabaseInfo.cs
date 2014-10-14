using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateModel
{
    public class DatabaseInfo
    {
        #region SQL Command
        private string _sqlGetTableNames = @"Select Name FROM SysObjects Where XType='U' ORDER BY Name ";
        private string _sqlGetColumnNames = @"Select Name FROM SysColumns Where id=Object_Id(@TableName) ";
        private string _sqlGenerateModel = @"
declare @result varchar(max) = 'public class ' + @TableName + '
{'

select @result = @result + '
    public ' + ColumnType + ' ' + ColumnName + ' { get; set; }
'
from
(
    select 
        replace(col.name, ' ', '_') ColumnName,
        column_id,
        case typ.name 
            when 'bigint' then 'long'
            when 'binary' then 'byte[]'
            when 'bit' then 'bool'
            when 'char' then 'string'
            when 'date' then 'DateTime'
            when 'datetime' then 'DateTime'
            when 'datetime2' then 'DateTime'
            when 'datetimeoffset' then 'DateTimeOffset'
            when 'decimal' then 'decimal'
            when 'float' then 'float'
            when 'image' then 'byte[]'
            when 'int' then 'int'
            when 'money' then 'decimal'
            when 'nchar' then 'char'
            when 'ntext' then 'string'
            when 'numeric' then 'decimal'
            when 'nvarchar' then 'string'
            when 'real' then 'double'
            when 'smalldatetime' then 'DateTime'
            when 'smallint' then 'short'
            when 'smallmoney' then 'decimal'
            when 'text' then 'string'
            when 'time' then 'TimeSpan'
            when 'timestamp' then 'DateTime'
            when 'tinyint' then 'byte'
            when 'uniqueidentifier' then 'Guid'
            when 'varbinary' then 'byte[]'
            when 'varchar' then 'string'
            else 'UNKNOWN_' + typ.name
        end ColumnType
    from sys.columns col
        join sys.types typ on
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
    where object_id = object_id(@TableName)
) t
order by column_id

set @result = @result  + '
}'

select @result
";
        private string _sqlColumnInfo = @"
 select 
        replace(col.name, ' ', '_') ColumnName,
        column_id ColumnId,
        case typ.name 
            when 'bigint' then 'long'
            when 'binary' then 'byte[]'
            when 'bit' then 'bool'
            when 'char' then 'string'
            when 'date' then 'DateTime'
            when 'datetime' then 'DateTime'
            when 'datetime2' then 'DateTime'
            when 'datetimeoffset' then 'DateTimeOffset'
            when 'decimal' then 'decimal'
            when 'float' then 'float'
            when 'image' then 'byte[]'
            when 'int' then 'int'
            when 'money' then 'decimal'
            when 'nchar' then 'char'
            when 'ntext' then 'string'
            when 'numeric' then 'decimal'
            when 'nvarchar' then 'string'
            when 'real' then 'double'
            when 'smalldatetime' then 'DateTime'
            when 'smallint' then 'short'
            when 'smallmoney' then 'decimal'
            when 'text' then 'string'
            when 'time' then 'TimeSpan'
            when 'timestamp' then 'DateTime'
            when 'tinyint' then 'byte'
            when 'uniqueidentifier' then 'Guid'
            when 'varbinary' then 'byte[]'
            when 'varchar' then 'string'
            else 'UNKNOWN_' + typ.name
        end ColumnType
    from sys.columns col
        join sys.types typ on
            col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
    where object_id = object_id(@TableName)";
        #endregion
        public string DatabaseName { get; set; }

        public IEnumerable<string> TableNames
        {
            get
            {
                return GetTableNames();
            }
        }

        private IEnumerable<string> GetTableNames()
        {
            return DbUtil.Default.Query<string>(_sqlGetTableNames, null);
        }

        public IEnumerable<string> GetColumnsName(string tableName)
        {
            return DbUtil.Default.Query<string>(_sqlGetColumnNames, new {TableName = tableName});
        }

        public string GetModelString(string tableName)
        {
            return DbUtil.Default.Query<string>(_sqlGenerateModel, new { TableName = tableName }).FirstOrDefault();
        }

        public IEnumerable<ColumnInfo> GetColumnInfo(string tableName)
        {
            return DbUtil.Default.Query<ColumnInfo>(_sqlColumnInfo, new {TableName = tableName});
        }
        public class ColumnInfo
        {
            public int ColumnId { get; set; }
            public string ColumnName { get; set; }

            public string ColumnType { get; set; }
        }
    }
}

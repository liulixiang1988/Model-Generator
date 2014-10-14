using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RazorEngine;

namespace GenerateModel
{
    public class ModelGenerator
    {
        public static string ModelString(string namespaceStr, string modelName, IEnumerable<DatabaseInfo.ColumnInfo> columns)
        {
            var templateStr = File.ReadAllText(@"Templates\base.cshtml");
            return Razor.Parse(templateStr, new { NameSpace = namespaceStr, ModelName = modelName, ColumnInfo = columns});
        }
    }
}

using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GenerateModel;

namespace TestApp
{
    public partial class GeneratorModelApp : Form
    {
        private string _conStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        public GeneratorModelApp()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            var nameSpaceStr = txtNameSpace.Text.Trim();
            var dictionaryPath = txtDirectory.Text.Trim();
            DbUtil.Default.ConnectionString = txtConnectionString.Text.Trim();
            DbUtil.Default.DatabaseType = DBTypes.SqlServer;
            var dbInfo = new DatabaseInfo();
            foreach (var tableName in dbInfo.TableNames)
            {
                var columnInfo = dbInfo.GetColumnInfo(tableName);
                string name = tableName;
                File.WriteAllText(dictionaryPath + "\\" + name + ".cs",
                    ModelGenerator.ModelString(nameSpaceStr, name,
                        columnInfo));
            }
            MessageBox.Show("成功生成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDirecoty_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            var result = fbd.ShowDialog();
            if (result != DialogResult.OK)
                return;
            txtDirectory.Text = fbd.SelectedPath;
        }
    }
}

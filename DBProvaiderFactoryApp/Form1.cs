using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBProvaiderFactoryApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*для асинхронної вставки*/
        async Task PerformDBOperationsUsingProviderModel(string connectionString, string providerName)
        {
            IList<User> users = new List<User>();
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                await connection.OpenAsync();

                DbCommand command = connection.CreateCommand();
                command.CommandText = "InsertUser";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                DbParameter parameter1 = command.CreateParameter();
                parameter1.ParameterName = "@login";
                parameter1.DbType = System.Data.DbType.String;
                parameter1.Value = loginTextBox.Text;
                DbParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "@password";
                parameter2.DbType = System.Data.DbType.String;
                string passwordHash = PasswordHasher.GetHash(passwordTextBox.Text, null, null);
                parameter2.Value = passwordHash;
                DbParameter[] parameters = new DbParameter[2];
                command.Parameters.Add(parameter1);
                command.Parameters.Add(parameter2);

                DbDataReader reader = await command.ExecuteReaderAsync();
            }
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            /*без асинхронної вставки*/
            /*string login = loginTextBox.Text;
            string passwordHash = PasswordHasher.GetHash(passwordTextBox.Text,null,null);
            User user = new User();
            user.Login = login;
            user.Password = passwordHash;
            UserDB userDB = new UserDB();
            userDB.InsertUser(user);*/
            string connectionString = ConfigurationManager.ConnectionStrings["UsersSqlExpressDB"].ConnectionString;
            string factory = ConfigurationManager.AppSettings["factory"];
            Task task = PerformDBOperationsUsingProviderModel(connectionString, factory);            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UserDB userDB = new UserDB();
            IList<User> users = userDB.GetUsers();
            userListGridView.DataSource = users;
        }

        private void refreshBrn_Click(object sender, EventArgs e)
        {
            UserDB userDB = new UserDB();
            IList<User> users = userDB.GetUsers();
            userListGridView.DataSource = users;
        }
    }
}

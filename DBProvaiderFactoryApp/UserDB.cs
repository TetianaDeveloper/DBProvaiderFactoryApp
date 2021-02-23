using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;
using System.Threading.Tasks;

namespace DBProvaiderFactoryApp
{
    public class UserDB
    {
        private DbProviderFactory provider;
        private DbConnection con;

        public UserDB()
        {
            // Получить фабрику
            string factory = ConfigurationManager.AppSettings["factory"];
            provider = DbProviderFactories.GetFactory(factory);

            // Использовать фабрику для получения соединения
            con = provider.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["UsersSqlExpressDB"].ConnectionString;
        }

        /*без асинхронної вставки*/
        public void InsertUser(User user)
        {
            DbCommand cmd = provider.CreateCommand();
            cmd.CommandText = "InsertUser";
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DbParameter parameter1 = cmd.CreateParameter();
            parameter1.ParameterName = "@login";
            parameter1.DbType = System.Data.DbType.String;
            parameter1.Value = user.Login;
            DbParameter parameter2 = cmd.CreateParameter();
            parameter2.ParameterName = "@password";
            parameter2.DbType = System.Data.DbType.String;
            parameter2.Value = user.Password;
            DbParameter[] parameters = new DbParameter[2];           
            cmd.Parameters.Add(parameter1);
            cmd.Parameters.Add(parameter2);
            try
            {
                using (con)
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (DbException)
            {
                // Замените эту ошибку чем-то более специфичным. 
                // Здесь также можно протоколировать ошибки
                throw new ApplicationException("Ошибка данных");
            }
            finally
            {
                con.Close();
            }
        }
        public IList<User> GetUsers()
        {
            IList<User> users = new List<User>();
            // Создать команду
            DbCommand cmd = provider.CreateCommand();
            cmd.CommandText = "GetAllUsers";
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            // Открыть соединение и получить данные
            using (con)
            {
                con.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User();
                        user.Id = int.Parse(reader["Id"].ToString());
                        user.Login = reader["Login"].ToString();
                        user.Password = reader["Password"].ToString();
                        users.Add(user);
                    }
                }
            }
            
            return users;
        }

    }
}

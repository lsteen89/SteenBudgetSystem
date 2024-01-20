using Dapper;
using SteenBudgetSystemLib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Media.Animation;

namespace SteenBudgetSystemLib.DataAccess
{
    public class SqlExecutor
    {
        public string getUser()
        {
            using (var connection = GlobalConfig.GetConnection())
            {
                string sqlQuery = "Select * from Users";
                var user = connection.QueryFirstOrDefault<User>(sqlQuery);
                return user.PasswordSalt; 
            }
        }
        public bool CreateUser(User user)
        {
            using (var connection = GlobalConfig.GetConnection())
            {
                string sqlQuery = @"INSERT INTO Users (Persoid, Firstname, Lastname, Email, EmailConfirmed, Password, PasswordSalt, CreatedBy, CreatedTime)
                            VALUES (@Persoid, @Firstname, @Lastname, @Email, 0, @Password, @PasswordSalt, 'System', GETDATE())";
                IDbTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();

                    user.Persoid = Guid.NewGuid().ToString();

                    connection.Execute(sqlQuery, user, transaction);

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public User GetUserByEmail(string email)
        {
            using (var connection = GlobalConfig.GetConnection())
            {
                string sqlQuery = "SELECT * FROM Users WHERE Email = @Email";
                User user = connection.QuerySingleOrDefault<User>(sqlQuery, new { Email = email });
                return user; 
            }
        }
    }
}

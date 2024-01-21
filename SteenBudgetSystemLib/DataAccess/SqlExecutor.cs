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
using SteenBudgetSystemLib.Helpers;

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
                string sqlQuery = @"INSERT INTO Users (Persoid, Firstname, Lastname, Email, EmailConfirmed, Password, PasswordSalt, Roles, CreatedBy, CreatedTime)
                    VALUES (@Persoid, @Firstname, @Lastname, @Email, 0, @Password, @PasswordSalt, '1', 'System', GETDATE())";

                IDbTransaction transaction = null;
                try
                {
                    transaction = connection.BeginTransaction();

                    user.PersoId = Guid.NewGuid().ToString();

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
        public bool CreatePartner(string Username, string partnerMainIncome, string partnerOtherIncome, string partnerName, decimal userRatio, bool firstTime)
        {
            Decimal partnerMainIncomeDec = SteenBudgetSystemLib.Helpers.SteenBudgetSystemHelper.ConvertToDecimal(partnerMainIncome);
            Decimal partnerOtherIncomeDec = SteenBudgetSystemLib.Helpers.SteenBudgetSystemHelper.ConvertToDecimal(partnerOtherIncome);
            if (firstTime)
            {
                using (var connection = GlobalConfig.GetConnection())
                {
                    string userSqlQuery = "SELECT PersoId FROM Users WHERE Email = @Email";
                    var userId = connection.QuerySingleOrDefault<string>(userSqlQuery, new { Email = Username });

                    if (userId != null)
                    {
                        string partnerSqlQuery = "INSERT INTO Partner (PersoId, PartnerId, Name, CreatedBy, CreatedTime, LastUpdatedTime) VALUES (@PersoId, @PartnerId, @Name, @CreatedBy, @CreatedTime, @LastUpdatedTime)";
                        string partnerIncomeSqlQuery = "INSERT INTO PartnerIncome (PartnerId, MainIncome, SideIncome, CreatedBy, CreatedTime, LastUpdatedTime) VALUES (@PartnerId, @MainIncome, @SideIncome, @CreatedBy, @CreatedTime, @LastUpdatedTime)";
                        string userExpenseRatioSqlQuery = "INSERT INTO UserExpenseRatio (PersoId, PartnerId, Ratio, CreatedBy, CreatedTime, LastUpdatedTime) VALUES (@PersoId, @PartnerId, @Ratio, @CreatedBy, @CreatedTime, @LastUpdatedTime)";

                        try
                        {
                            using (IDbTransaction transaction = connection.BeginTransaction())
                            {
                                string partnerId = Guid.NewGuid().ToString();
                                var partnerParams = new { PersoId = userId, PartnerId = partnerId, Name = partnerName.Trim(), CreatedBy = Username, CreatedTime = DateTime.UtcNow, LastUpdatedTime = DateTime.UtcNow };
                                connection.Execute(partnerSqlQuery, partnerParams, transaction);

                                var partnerIncomeParams = new { PartnerId = partnerId, MainIncome = partnerMainIncomeDec, SideIncome = partnerOtherIncomeDec, CreatedBy = Username, CreatedTime = DateTime.UtcNow, LastUpdatedTime = DateTime.UtcNow };
                                connection.Execute(partnerIncomeSqlQuery, partnerIncomeParams, transaction);

                                var userExpenseRatioParams = new { PersoId = userId, PartnerId = partnerId, Ratio = userRatio, CreatedBy = Username, CreatedTime = DateTime.UtcNow, LastUpdatedTime = DateTime.UtcNow };
                                connection.Execute(userExpenseRatioSqlQuery, userExpenseRatioParams, transaction);

                                transaction.Commit();
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            return false;
                        }
                    }
                }
            }
            return false;
        }


    }
}

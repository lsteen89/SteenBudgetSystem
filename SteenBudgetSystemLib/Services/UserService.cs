using SteenBudgetSystemLib.DataAccess;
using SteenBudgetSystemLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Services
{
    public class UserService
    {
        public bool CreateUser(string firstName, string lastName, string email, string password)
        {
            int saltSize = 16;
            byte[] salt = PasswordHasher.GenerateSalt(saltSize);

            User user = new User
            {
                Firstname = firstName,
                LastName = lastName,
                Email = email,
                Password = PasswordHasher.HashPasswordWithSalt(password, salt),
                PasswordSalt = Convert.ToBase64String(salt)
            };

            try
            {
                SqlExecutor sqlExecutor = new SqlExecutor();
                return sqlExecutor.CreateUser(user);
            }
            catch (Exception)
            {
                
                return false;
            }
        }
        public bool CreateOrUpdatePartner(string username, string partnerMainIncome, string partnerOtherIncome, string partnerName, decimal userRatio, bool isFirstLogin)
        {
            SqlExecutor sqlExecutor = new SqlExecutor();
            return sqlExecutor.CreatePartner(username, partnerMainIncome, partnerOtherIncome, partnerName, userRatio, isFirstLogin);
        }
    }

}

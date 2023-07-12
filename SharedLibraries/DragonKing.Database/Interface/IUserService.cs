using DragonKing.Database.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonKing.Database.Interface
{
    public interface IUserService
    {
        List<User> GetUsers();
        bool AddUser(User user);
        void RemoverUser(User user);
        bool UpdateUser(User user);
        bool UpdateUser();
        bool UpdatePassword(User user, string strPassword, bool isReset);
        bool CheckPassword(string name);
        User GetUserById(int id);
        User GetUserByName(string name);
    }
}

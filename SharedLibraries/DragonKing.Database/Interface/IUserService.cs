using DragonKing.Database.EntityModel;
using System.Collections.Generic;

namespace DragonKing.Database.Interface
{
    public interface IUserService
    {
        List<User> GetUsers();
        bool AddUser(User user);
        bool RemoverUser(User user);
        bool UpdateUser(User user);
        bool UpdateUser();
        bool UpdatePassword(User user, string strPassword, bool isReset);
        bool CheckPassword(string name);
        User GetUserById(int id);
        User GetUserByName(string name);
    }
}

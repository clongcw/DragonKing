using DragonKing.Database.Constants;
using DragonKing.Database.EntityModel;
using DragonKing.Log.Interface;
using SqlSugar;

namespace DragonKing.Database.DbContext
{
    public class UserDbContext
    {
        private readonly ILog _log;

        public SqlSugarScope Db;
        public UserDbContext(ILog log)
        {
            _log = log;
            Db = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = Environments.UserConnectionString,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                AopEvents = new AopEvents()
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        //_log.Debug(sql);
                    }
                }
            });

            Db.DbMaintenance.CreateDatabase();
            Db.CodeFirst.InitTables<Role, User, Privilege>();

            /*Role role = new Role()
            {
                Id = 1,
                RoleName = "管理员",
                Privileges = new List<Privilege>()
                {
                    new Privilege() { Id = 1, Name = "Lis", RoleId = 1 },
                    new Privilege() { Id = 2, Name = "Report", RoleId = 1 },
                    new Privilege() { Id = 3, Name = "Setting", RoleId = 1 }
                }
            };

            RoleDb.Context.InsertNav(role)
                .Include(z => z.Privileges)
                 .ExecuteCommand();


            UserDb.Insert(new User
            {
                Id = 1,
                RoleId = 1,
                Name = "Admin",
                Password = "123456",
                IsDefault = true,
                Creator = "clongc",
                CreateDate = DateTime.Now
            });*/
        }

        public SimpleClient<Role> RoleDb => new SimpleClient<Role>(Db);
        public SimpleClient<User> UserDb => new SimpleClient<User>(Db);
        public SimpleClient<Privilege> PrivilegeDb => new SimpleClient<Privilege>(Db);

    }
}

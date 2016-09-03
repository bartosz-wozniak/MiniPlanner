using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Converters;
using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Logic
{
    public class UserLogic
    {
        public MiniPlannerDbEntities Context { get; set; }

        public UserLogic()
        {
            Context = new MiniPlannerDbEntities();
        }

        public async Task<ObservableCollection<DtoUser>> GetUsers(string filter = null)
        {
            var ret = new ObservableCollection<DtoUser>();
            using (var data = Context)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    foreach (var item in await (from item in data.Users where item.login.Contains(filter) select item).ToListAsync())
                        ret.Add(UserConverter.DataAccsessToDto(item));
                else
                    foreach (var item in await (from item in data.Users select item).ToListAsync())
                        ret.Add(UserConverter.DataAccsessToDto(item));
                return ret;
            }
        }

        public async Task<DtoUser> GetUser(int userId)
        {
            try
            {
                using (var data = Context)
                    return UserConverter.DataAccsessToDto(await (from item in data.Users where item.id == userId select item).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DtoUser> GetUser(string login)
        {
            try
            {
                using (var data = Context)
                    return UserConverter.DataAccsessToDto(await (from item in data.Users where item.login == login select item).FirstOrDefaultAsync());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SaveUser(DtoUser user)
        {
            try
            {
                using (var data = Context)
                {
                    var u = await (from item in data.Users where user.Id == item.id select item).FirstOrDefaultAsync();
                    // Updating user
                    if (u != null)
                    {
                        u.login = user.Login;
                        u.cardId = user.CardId;
                        u.password = AuthenticationLogic.HashPassword(user.Password, user.Login);
                        //u.isAdmin = user.IsAdmin;
                        u.averageScore = user.AverageScore;
                    }
                    // Adding new user
                    else
                    {
                        data.Users.Add(UserConverter.DtoToDataAccess(user));
                    }
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveUser(int userId)
        {
            try
            {
                using (var data = Context)
                {
                    var u = await (from item in data.Users where item.id == userId select item).FirstAsync();
                    data.Users.Remove(u);
                    await data.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

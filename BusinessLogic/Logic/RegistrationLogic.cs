using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Converters;
using DataAccess;

namespace BusinessLogic.Logic
{
    public class RegistrationLogic
    {
        public MiniPlannerDbEntities Context { get; set; }

        public RegistrationLogic()
        {
            Context = new MiniPlannerDbEntities();
        }

        public async Task<string> GetStatus()
        {
            try
            {
                using (var data = Context)
                    return RegistrationConverter.DataAccsessToDto(await (from item in data.Registration select item).FirstOrDefaultAsync()).Status;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateStatus(string status)
        {
            try
            {
                using (var data = Context)
                {
                    var r = await (from item in data.Registration select item).FirstOrDefaultAsync();
                    // Updating
                    if (r != null)
                    {
                        r.status = status;
                        await data.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

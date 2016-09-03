using BusinessLogic.DtoObjects;
using DataAccess;

namespace BusinessLogic.Converters
{
    public class RegistrationConverter
    {
        public static DtoRegistration DataAccsessToDto(Registration d)
        {
            return new DtoRegistration
            {
                Status = d.status
            };
        }

        public static Registration DtoToDataAccess(DtoRegistration d)
        {
            return new Registration
            {
                status = d.Status
            };
        }
    }
}


using BusinessLogic.DtoObjects;
using BusinessLogic.Logic;
using DataAccess;

namespace BusinessLogic.Converters
{
    public class UserConverter
    {
        public static DtoUser DataAccsessToDto(Users d)
        {
            return new DtoUser
            {
                Id = d.id,
                Login = d.login,
                CardId = d.cardId,
                Password = d.password,
                AverageScore = d.averageScore,
                IsAdmin = d.isAdmin
            };
        }

        public static Users DtoToDataAccess(DtoUser d)
        {
            return new Users
            {
                id = d.Id,
                login = d.Login,
                cardId = d.CardId,
                password = AuthenticationLogic.HashPassword(d.Password, d.Login),
                averageScore = d.AverageScore,
                isAdmin = d.IsAdmin
            };
        }
    }
}

using System;

namespace BusinessLogic.DtoObjects
{
    public class DtoUser : IEquatable<DtoUser>
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string CardId { get; set; }

        public string Password { get; set; }

        public double AverageScore { get; set; }

        public bool IsAdmin { get; set; }


        public bool Equals(DtoUser other)
        {
            if (Id == other.Id && Login == other.Login && CardId == other.CardId && Password == other.Password && IsAdmin == other.IsAdmin)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashId = Id.GetHashCode();
            int hashLogin = Login?.GetHashCode() ?? 0;
            return hashId ^ hashLogin;
        }
    }
}

namespace BusinessLogic.DtoObjects
{
    public class DtoSchedule
    {
        public int Id { get; set; }

        public DtoUser User { get; set; }

        public DtoCourse Course { get; set; }

        public int ScheduleId { get; set; }
    }
}

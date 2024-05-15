namespace TaskMgmtApi.Model
{
    public class StaffCreateBody
    {
        public string? FullName { get; set; }
        public string? ShortName { get; set; }
    }
    public class StaffUpdateBody : StaffCreateBody
    {
        public int Id { get; set; }
    }
}

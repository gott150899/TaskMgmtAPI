namespace TaskMgmtApi.Model
{
    public class TaskCreateBody
    {
        public int? ParentId { get; set; }
        public string? Label { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? Duration { get; set; }
        public double? Progress { get; set; }
        public bool? IsUnscheduled { get; set; }
        public List<int> StaffIds { get; set; } = new List<int>();
    }
    public class TaskUpdateBody : TaskCreateBody
    {
        public int Id { get; set; }
    }
}

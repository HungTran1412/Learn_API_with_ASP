using System;
using System.Collections.Generic;

namespace BaiTest.Models;

public partial class ExamRoom
{
    public int Id { get; set; }

    public string? RoomCode { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<ExamAssignment> ExamAssignments { get; set; } = new List<ExamAssignment>();
}

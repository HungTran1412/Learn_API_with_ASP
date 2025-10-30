using System;
using System.Collections.Generic;

namespace BaiTest.Models;

public partial class ExamAssignment
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? RoomId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public virtual ExamRoom? Room { get; set; }

    public virtual Student? Student { get; set; }
}

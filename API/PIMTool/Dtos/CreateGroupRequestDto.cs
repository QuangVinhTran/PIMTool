﻿using PIMTool.Core.Domain.Entities;

namespace PIMTool.Dtos
{
    public class CreateGroupRequestDto
    {
        public int LeaderId { get; set; }
        public ICollection<int> EmployeeIds { get; set; }
    }
}

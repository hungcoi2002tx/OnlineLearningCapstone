﻿using Microsoft.AspNetCore.Http;
using Share.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class ExamRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string TeacherId { get; set; }
        public string ClassroomId { get; set; }
        public string Status { get; set; }
        public IFormFile? Attachments { get; set; }
    }
}

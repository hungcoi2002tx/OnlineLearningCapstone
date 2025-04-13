using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.RequestModel
{
    public class UpdateAttachmentRequestModel
    {
        public IFormFile? Attachment { get; set; }
    }
}

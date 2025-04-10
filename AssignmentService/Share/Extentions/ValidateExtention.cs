using Share.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Share.Extentions
{
    public static class ValidateExtention
    {
        public static List<string> Validate(this ExamRequestModel model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                errors.Add("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                errors.Add("Description is required.");
            }

            if (model.Deadline == default)
            {
                errors.Add("Deadline must be a valid date.");
            }
            else if (model.Deadline < DateTime.Now)
            {
                errors.Add("Deadline cannot be in the past.");
            }

            if (string.IsNullOrWhiteSpace(model.TeacherId))
            {
                errors.Add("TeacherId is required.");
            }

            if (string.IsNullOrWhiteSpace(model.ClassroomId))
            {
                errors.Add("ClassroomId is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Status))
            {
                errors.Add("Status is required.");
            }

            // Kiểm tra tệp đính kèm (nếu cần)
            if (model.Attachments != null && model.Attachments.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var extension = System.IO.Path.GetExtension(model.Attachments.FileName).ToLower();

                if (!Array.Exists(allowedExtensions, e => e == extension))
                {
                    errors.Add("Attachment must be a PDF, DOCX, or XLSX file.");
                }
            }

            return errors;
        }
    }
}

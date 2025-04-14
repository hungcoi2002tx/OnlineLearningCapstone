using Share.Other;
using Share.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Share.Other.SearchModel;

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

            if (!Enum.TryParse<AssignmentStatus>(model.Status, true, out var _))
            {
                errors.Add("Status must be one of the defined AssignmentStatus enum values.");
            }

            // Kiểm tra tệp đính kèm (nếu cần)
            if (model.Attachments != null && model.Attachments.Length > 0)
            {
                //var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var extension = System.IO.Path.GetExtension(model.Attachments.FileName).ToLower();

                //if (!Array.Exists(allowedExtensions, e => e == extension))
                //{
                //    errors.Add("Attachment must be a PDF, DOCX, or XLSX file.");
                //}
            }

            return errors;
        }

        public static bool IsValid(this UpdateExamRequestModel model, out List<string> errors)
        {
            errors = new List<string>();
            if (string.IsNullOrWhiteSpace(model.Title))
                errors.Add("Title is required.");
            if (string.IsNullOrWhiteSpace(model.Description))
                errors.Add("Description is required.");
            if (model.Deadline == default)
                errors.Add("Deadline must be a valid date.");
            else if (model.Deadline < DateTime.Now)
                errors.Add("Deadline cannot be in the past.");
            if (string.IsNullOrWhiteSpace(model.Status))
                errors.Add("Status is required.");
            else if (!Enum.TryParse<AssignmentStatus>(model.Status, true, out var _))
                errors.Add("Status must be one of the defined AssignmentStatus enum values.");
            return !errors.Any();
        }

        public static bool IsValid(this QuizRequestDto dto, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                errors.Add("Description is required.");

            if (!Enum.TryParse<AssignmentStatus>(dto.Status, true, out var _))
            {
                errors.Add("Status must be one of the defined AssignmentStatus enum values.");
            }

            if (string.IsNullOrWhiteSpace(dto.Status))
                errors.Add("Type is required.");

            if (dto.Deadline <= DateTime.UtcNow)
                errors.Add("Deadline must be in the future.");

            if (string.IsNullOrWhiteSpace(dto.TeacherId) || dto.TeacherId == "0")
                errors.Add("TeacherId is required and must be valid.");

            if (string.IsNullOrWhiteSpace(dto.ClassroomId) || dto.ClassroomId == "0")
                errors.Add("ClassroomId is required and must be valid.");

            if (dto.Questions == null || !dto.Questions.Any())
            {
                errors.Add("At least one question is required.");
            }
            else
            {
                for (int i = 0; i < dto.Questions.Count; i++)
                {
                    var question = dto.Questions[i];

                    if (string.IsNullOrWhiteSpace(question.Content))
                        errors.Add($"Question {i + 1} content is required.");

                    if (question.Answers == null || question.Answers.Count < 2)
                        errors.Add($"Question {i + 1} must have at least 2 answers.");

                    if (!question.Answers.Any(a => a.IsCorrect))
                        errors.Add($"Question {i + 1} must have at least one correct answer.");
                }
            }

            return !errors.Any();
        }

        public static bool IsValid(this AssignmentSearch model, out List<string> errors)
        {
            errors = new List<string>();

            if (model.Status != null && !Enum.TryParse<AssignmentStatus>(model.Status, true, out var _))
            {
                errors.Add("Status must be one of the defined AssignmentStatus enum values.");
            }
            if (model.IsAll != true && model.Page == null)
            {
                errors.Add("Page is required unless All is true.");
            }

            if (model.Page != null)
            {
                if (model.Page.Index <= 0 || model.Page.Size <= 0)
                {
                    errors.Add("Page index and size must be greater than 0.");
                }
            }
            return !errors.Any();
        }

        public static bool IsValid(this UpdateAssignmentRequestModel model, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                errors.Add("Title is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                errors.Add("Description is required.");
            }

            if (model.Deadline < DateTime.Now)
            {
                errors.Add("Deadline must be in the future.");
            }

            if (string.IsNullOrWhiteSpace(model.Status))
            {
                errors.Add("Status is required.");
            }
            else if (!Enum.TryParse(typeof(AssignmentStatus), model.Status, true, out _))
            {
                var allowedValues = string.Join(", ", Enum.GetNames(typeof(AssignmentStatus)));
                errors.Add($"Status must be one of: {allowedValues}");
            }

            return !errors.Any();
        }

        public static bool IsValid(this UpdateAttachmentRequestModel model, out List<string> errors)
        {
            errors = new List<string>();
            return !errors.Any();
        }

        public static bool IsValid(this CreateQuestionRequestModel model, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Content))
                errors.Add("Question content is required.");

            if (model.Answers == null || model.Answers.Count < 2)
                errors.Add("At least two answers are required.");

            if (model.Answers != null && !model.Answers.Any(a => a.IsCorrect))
                errors.Add("At least one answer must be marked as correct.");

            if (model.Answers != null)
            {
                for (int i = 0; i < model.Answers.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(model.Answers[i].Content))
                        errors.Add($"Answer #{i + 1} content is required.");
                }
            }

            return !errors.Any();
        }

        public static bool IsValid(this UpdateQuestionRequestModel model, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Content))
                errors.Add("Question content is required.");

            if (model.Answers == null || model.Answers.Count < 2)
                errors.Add("At least two answers are required.");

            if (model.Answers != null && !model.Answers.Any(a => a.IsCorrect))
                errors.Add("At least one answer must be marked as correct.");

            if (model.Answers != null)
            {
                for (int i = 0; i < model.Answers.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(model.Answers[i].Content))
                        errors.Add($"Answer #{i + 1} content is required.");
                }
            }

            return !errors.Any();
        }

        public static bool IsValid(this CreateQuizSubmissionRequest model, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.AssignmentId))
                errors.Add("AssignmentId is required.");

            if (string.IsNullOrWhiteSpace(model.StudentId))
                errors.Add("StudentId is required.");

            if (model.SubmitDate == default)
                errors.Add("SubmitDate is required.");

            if (model.QuizAnswers == null || !model.QuizAnswers.Any())
                errors.Add("At least one quiz answer is required.");
            else
            {
                foreach (var answer in model.QuizAnswers)
                {
                    if (string.IsNullOrWhiteSpace(answer.QuestionId))
                        errors.Add("QuestionId in QuizAnswers is required.");

                    if (string.IsNullOrWhiteSpace(answer.AnswerId))
                        errors.Add("AnswerId in QuizAnswers is required.");
                }
            }

            return !errors.Any();
        }

        public static bool IsValid(this CreateExamSubmissionRequest model, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.AssignmentId))
                errors.Add("AssignmentId is required.");

            if (string.IsNullOrWhiteSpace(model.StudentId))
                errors.Add("StudentId is required.");

            if (model.SubmitDate == default)
                errors.Add("SubmitDate is required.");

            return !errors.Any();
        }

        public static bool IsValid(this SubmissionSearch model, out List<string> errors)
        {
            errors = new List<string>();

            if (model.Status != null && !Enum.TryParse<SubmissionStatus>(model.Status, true, out var _))
            {
                errors.Add("Status must be one of the defined SubmissionStatus enum values.");
            }
            if (model.IsAll != true && model.Page == null)
            {
                errors.Add("Page is required unless All is true.");
            }

            if (model.Page != null)
            {
                if (model.Page.Index <= 0 || model.Page.Size <= 0)
                {
                    errors.Add("Page index and size must be greater than 0.");
                }
            }
            return !errors.Any();
        }
    }
}

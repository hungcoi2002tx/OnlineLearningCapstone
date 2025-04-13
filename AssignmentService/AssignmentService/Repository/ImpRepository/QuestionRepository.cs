using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Share.Model;
using Share.Other;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Repository.ImpRepository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        readonly IMapper _mapper;
        public QuestionRepository(AssignmentDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Question model)
        {
			try
			{
                OpenTransaction();
                await _context.Questions.AddAsync(model);
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
				throw;
			}
        }

        public async Task<List<Question>> GetAllByFilterAsync(QuestionSearch search)
        {
            try
            {
                OpenTransaction();
                var query = _context.Questions.AsQueryable();
                if (!String.IsNullOrEmpty(search.Id))
                {
                    query = query.Where(a => a.Id == search.Id);
                }
                if (!String.IsNullOrEmpty(search.AssignmentId))
                {
                    query = query.Where(a => a.AssignmentId == search.AssignmentId);
                }
                if (!String.IsNullOrEmpty(search.Content))
                {
                    query = query.Where(a => a.Content.Contains(search.Content));
                }
                if (search.Ids?.Any() == true)
                {
                    query = query.Where(x => search.Ids.Contains(x.Id));
                }
                var result = await query.Include(x => x.Answers).ToListAsync();
                await CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Question question, UpdateQuestionRequestModel model)
        {
            try
            {
                OpenTransaction();
                _context.Answers.RemoveRange(question.Answers);
                _mapper.Map(model, question);
                _context.Attach(question).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

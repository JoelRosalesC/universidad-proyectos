using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Database;
using Microsoft.EntityFrameworkCore;

namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<EmailTemplate> _dbSet;

        public EmailTemplateRepository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<EmailTemplate>();
        }
        public async Task<EmailTemplate> AddAsync(EmailTemplate emailTemplate)
        {
            _dbSet.Add(emailTemplate);
            await _context.SaveChangesAsync();
            return emailTemplate;
        }
        public async Task<EmailTemplate> UpdateAsync(EmailTemplate emailTemplate)
        {
            _dbSet.Update(emailTemplate);
            await _context.SaveChangesAsync();
            return emailTemplate;
        }
        public async Task DeleteByIdAsync(int emailTemplateId)
        {
            EmailTemplate emailTemplate = await GetByIdAsync(emailTemplateId);
            _dbSet.Remove(emailTemplate);
            await _context.SaveChangesAsync();
        }
        public async Task<EmailTemplate> GetByIdAsync(int id)
        {
            var emailTemplate = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if(emailTemplate == null)
                throw new ApiException(new ApiErrorResponse("The email template is not already registered in the database."));
            return emailTemplate;
        }
        public async Task<EmailTemplate> GetByNameAsync(string name)
        {
            var emailTemplate = await _dbSet.FirstOrDefaultAsync(e => e.Name == name);
            if(emailTemplate == null)
                throw new ApiException(new ApiErrorResponse("The email template is not already registered in the database."));
            return emailTemplate;
        }        
        public async Task<List<EmailTemplate>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

    }
}
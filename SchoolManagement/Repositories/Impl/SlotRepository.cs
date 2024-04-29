using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;

namespace SchoolManagement.Repositories.Impl
{
    public class SlotRepository : GenericRepository<Slot>, ISlotRepository
    {
        private readonly SchoolManageDbContext _context;

        public SlotRepository(SchoolManageDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public override async Task<Slot> AddEntity(Slot entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;

            }
            catch (Exception ex)
            {
                throw new BadRequestException("Cannot add slot");
            }
        }

        public override async Task<bool> DeleteEntity(int id)
        {
            var existdata = await DbSet.FirstOrDefaultAsync(item => item.SlotId == id);
            if (existdata != null)
            {
                DbSet.Remove(existdata);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override async Task<List<Slot>> GetAllAsync()
        {
            return await DbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public override async Task<Slot> GetAsync(int id)
        {
            return await DbSet.Where(x => !x.IsDeleted).SingleOrDefaultAsync(item => item.SlotId == id);
        }

        public override async Task<Slot> UpdateEntity(Slot entity)
        {
            try
            {
                DbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            } catch (Exception ex) 
            {
                throw new BadRequestException("Cannot update slot");
            }
            
        }

    }
}

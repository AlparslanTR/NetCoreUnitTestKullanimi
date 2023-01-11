using Microsoft.EntityFrameworkCore;
using ProductMvc.Models;

namespace ProductMvc.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MvcUnitTestContext _comtext;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(MvcUnitTestContext comtext)
        {
            _comtext = comtext;
            _dbSet = _comtext.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _comtext.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _comtext.SaveChanges();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _comtext.Update(entity).State=EntityState.Modified;
            _dbSet.Update(entity);
            _comtext.SaveChanges();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class HelpdeskRepository<T> : IRepository<T> where T : HelpdeskEntity
    {
        readonly private HelpdeskContext _db;

        public HelpdeskRepository(HelpdeskContext context = null) => _db = context ?? new HelpdeskContext();
        public async Task<List<T>> GetAll() => await _db.Set<T>().ToListAsync();
        public async Task<List<T>> GetSome(Expression<Func<T, bool>> match) => await _db.Set<T>().Where(match).ToListAsync();
        public async Task<T> GetOne(Expression<Func<T, bool>> match) => await _db.Set<T>().FirstOrDefaultAsync(match);

        public async Task<T> Add(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<UpdateStatus> Update(T updated)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                T current = await GetOne(ent => ent.Id == updated.Id);
                _db.Entry(current).OriginalValues["Timer"] = updated.Timer;
                _db.Entry(current).CurrentValues.SetValues(updated);

                if (await _db.SaveChangesAsync() == 1)
                    status = UpdateStatus.Ok;
            }
            catch (DbUpdateConcurrencyException dbx)
            {
                status = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + ex.Message);
            }
            return status;
        }

        public async Task<int> Delete(int id)
        {
            T current = await GetOne(ent => ent.Id == id);
            _db.Set<T>().Remove(current);
            return _db.SaveChanges();
        }
    }
}

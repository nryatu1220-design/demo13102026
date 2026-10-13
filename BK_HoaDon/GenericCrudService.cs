using Microsoft.EntityFrameworkCore;
using TheLastTerm_Draft.Data;
using System.Collections.Generic;
using System.Linq;

namespace TheLastTerm_Draft.Service
{
    public class GenericCrudService<T> where T : class
    {
        private readonly HeThongKhoContext _context;
        private readonly DbSet<T> _table;

        public GenericCrudService(HeThongKhoContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        // =========================
        // LẤY TẤT CẢ
        // =========================
        public List<T> LayTatCa()
        {
            return _table.AsNoTracking().ToList();
        }

        // =========================
        // TÌM THEO KHÓA CHÍNH
        // =========================
        public T? TimTheoId(object id)
        {
            return _table.Find(id);
        }

        // =========================
        // THÊM
        // =========================
        public void Them(T entity)
        {
            _context.ChangeTracker.Clear();
            _table.Add(entity);
            _context.SaveChanges();
        }

        // =========================
        // SỬA
        // =========================
        public void Sua(T entity)
        {
            //_table.Update(entity);
            //_context.SaveChanges();
            _context.ChangeTracker.Clear();
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        // =========================
        // XÓA
        // =========================
        public void Xoa(T entity)
        {
            _context.ChangeTracker.Clear();
            _table.Remove(entity);
            _context.SaveChanges();
        }
    }
}

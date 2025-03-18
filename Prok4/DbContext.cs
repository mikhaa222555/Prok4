using Prok4.TaskManagerApp.Models;

namespace TaskManagerApp.Data
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

        public DbSet<Клиент> Клиенты { get; set; }
        public DbSet<Задачи> Задачи { get; set; }
        public DbSet<Отдел> Отделы { get; set; }
        public DbSet<Приоритеты> Приоритеты { get; set; }
        public DbSet<Статусы> Статусы { get; set; }
    }
}
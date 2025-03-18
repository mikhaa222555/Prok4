namespace TaskManagerApp.Data
{
    public class DbContext
    {
        private DbContextOptions<TaskManagerDbContext> options;

        public DbContext(DbContextOptions<TaskManagerDbContext> options)
        {
            this.options = options;
        }
    }
}
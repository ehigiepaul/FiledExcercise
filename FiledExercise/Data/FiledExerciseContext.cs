using FiledExercise.Models;
using Microsoft.EntityFrameworkCore;

namespace FiledExercise.Data
{
    public class FiledExerciseContext : DbContext
    {
        public FiledExerciseContext (DbContextOptions<FiledExerciseContext> context)
            : base(context)
        {
        }

        public DbSet<CardDetail> CardDetail { get; set; }

        public DbSet<PaymentState> PaymentState { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace ASP_P42.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.UserData> UsersData { get; set; }
        public DbSet<Entities.UserRole> UsersRoles { get; set; }
        public DbSet<Entities.UserAccess> UserAccess { get; set; }

        // Конструирования контекста настраиваются из Program.cs
        // соответственно, на время проекторивания делегируется 
        // конструктор с параметрами подключения 
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // настройки, которые выполняются во время первой загрузки 
            // контекста данных, кроме того связи между таблицами,
            // уникальность, и т.д

            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(ua => ua.Login)
                .IsUnique();

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserData)
                .WithMany(ud => ud.Accesses)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<Entities.UserAccess>()
                .HasOne(ua => ua.UserRole)
                .WithMany()
                .HasForeignKey(ua => ua.RoleId);

            // зернование - внесение начальных данных,
            // кроме того базовые роли и корневой администратор


            modelBuilder.Entity<Entities.UserRole>()
                .HasData([
                    new() {
                        Id = Guid.Parse("21F7C25A-629B-4BEB-9339-0C37AC9A8444"),
                        Name = "Admin",
                        Description = "Корневой администратор",
                        CreateLevel = 10,
                        ReadLevel = 10,
                        UpdateLevel = 10,
                        DeleteLevel = 10,
                    },
                    new() {
                        Id = Guid.Parse("C741DF27-DA81-4D54-B61B-C4C9A2AE7A73"),
                        Name = "User",
                        Description = "Самозарегистрировавшийся пошльзователь",
                        CreateLevel = 0,
                        ReadLevel = 0,
                        UpdateLevel = 0,
                        DeleteLevel = 0,
                    }
                    ]);
            modelBuilder.Entity<Entities.UserData>()
                .HasData([
                    new() {
                        Id = Guid.Parse(""),
                        FullName = "Администратор системы",
                        BirthDate = DateTime.UnixEpoch,
                        Email = "CHANGE@ME",
                        Phone = "CHANGE_ME",
                        RegisteredAt = DateTime.UnixEpoch,
                    }
                ]);

            modelBuilder.Entity<Entities.UserAccess>()
                .HasData([
                    new() {   
                        Id = Guid.Parse("96DCBBBA-9AEE-44A2-8835-72DFE4E1A710"),
                        // ID - як в Адміністратора   
                       // ID - як в ролі Адміністратора    
                        Login = "Admin",
                        Salt = "96DCBBBA-9AEE-44A2-8835-72DFE4E1A710",
                        Dk = "",
                     }]);
        }
    }
}

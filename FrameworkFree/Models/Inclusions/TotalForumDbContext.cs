using Microsoft.EntityFrameworkCore;
using Own.Database;
using Own.Storage;
namespace Inclusions
{
    public partial class TotalForumDbContext : DbContext
    {
        public TotalForumDbContext()
        {
        }

        public TotalForumDbContext(DbContextOptions<TotalForumDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Endpoint> Endpoint { get; set; }
        public virtual DbSet<Forum> Forum { get; set; }
        public virtual DbSet<Msg> Msg { get; set; }
        public virtual DbSet<PrivateMessage> PrivateMessage { get; set; }
        public virtual DbSet<Thread> Thread { get; set; }
        public virtual DbSet<LoginLog> LoginLog { get; set; }
        public virtual DbSet<BlockedIpHash> BlockedIpHash { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            { }
            else
                optionsBuilder.UseSqlServer(Fast.GetConnectionStringLocked());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Nick)
                    .HasDatabaseName("GetNicks")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Nick })
                    .HasDatabaseName("GetNick");

                entity.HasIndex(e => new { e.Identifier, e.Passphrase })
                    .HasDatabaseName("UQ_Account_LoginPassword")
                    .IsUnique();

                entity.HasIndex(e => new { e.Nick })
                    .HasDatabaseName("UQ_Account_Nick")
                    .IsUnique();

                entity.HasIndex(e => new { e.Nick, e.SecretHash })
                    .HasDatabaseName("UQ_Account_NickSecretHash")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Identifier, e.Passphrase })
                    .HasDatabaseName("GetAccounts");
            });

            modelBuilder.Entity<Endpoint>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.Name, e.ForumId })
                    .HasDatabaseName("GetEndpointsTop5");
            });

            modelBuilder.Entity<Forum>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("UQ_Forum_Name")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.Name })
                    .HasDatabaseName("GetForums");
            });

            modelBuilder.Entity<Msg>(entity =>
            {
                entity.HasIndex(e => new { e.ThreadId })
                    .HasDatabaseName("GetMessagesCount");

                entity.HasIndex(e => new { e.Id, e.ThreadId, e.AccountId })
                    .HasDatabaseName("GetMessages");
            });

            modelBuilder.Entity<PrivateMessage>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.SenderAccountId, e.AcceptorAccountId })
                    .HasDatabaseName("GetPrivateMessagesCount");
            });

            modelBuilder.Entity<Thread>(entity =>
            {
                entity.HasIndex(e => e.EndpointId)
                    .HasDatabaseName("ThreadsCountByEndpointId");

                entity.HasIndex(e => e.Name)
                    .HasDatabaseName("UQ_Thread_Name")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.EndpointId })
                    .HasDatabaseName("GetThreadsCount");

                entity.HasIndex(e => new { e.Id, e.Name })
                    .HasDatabaseName("GetThreadName");

                entity.HasIndex(e => new { e.Id, e.Name, e.EndpointId })
                    .HasDatabaseName("GetThreads");
            });

            modelBuilder.Entity<LoginLog>(entity =>
            {
                entity.HasIndex(e => new { e.AccountIdentifier, e.IpHash })
                    .HasDatabaseName("UQ_LoginLog_AccountIdentifierIpHash")
                    .IsUnique();
            });

            modelBuilder.Entity<BlockedIpHash>(entity =>
            {
                entity.HasIndex(e => new { e.IpHash })
                    .HasDatabaseName("UQ_BlockedIpHash_IpHash")
                    .IsUnique();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasIndex(e => new { e.AccountId })
                    .HasDatabaseName("UQ_Profile_AccountId")
                    .IsUnique();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

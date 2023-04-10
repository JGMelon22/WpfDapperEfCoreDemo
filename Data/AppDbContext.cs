using WpfDapperEfCoreDemo.Models;

namespace WpfDapperEfCoreDemo.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	public DbSet<Pessoa> Pessoas { get; set; }
	public DbSet<Telefone> Telefones { get; set; }
	public DbSet<Detalhe> Detalhes { get; set; }
}

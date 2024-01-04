using BlazingPizzaSite.Model;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizzaSite.Data;

public class PizzaStoreContext : DbContext
{
	public PizzaStoreContext(DbContextOptions<PizzaStoreContext> options)
		: base(options)
	{
	}

	public DbSet<PizzaSpecial> Specials { get; set; }

}

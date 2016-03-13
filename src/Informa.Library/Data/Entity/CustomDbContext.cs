namespace Informa.Library.Data.Entity
{
	public class CustomDbContext : System.Data.Entity.DbContext
	{
		public CustomDbContext()
			: base("name=custom")
		{

		}
	}
}

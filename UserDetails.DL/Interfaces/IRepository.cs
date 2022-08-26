namespace UserDetailsDL.Interfaces
{
	public interface IRepository<T>
	{
		public abstract Task<bool> Save(T obj);
		public abstract Task<bool> ValidateAndCreateDataSource(string fileName);
	}
}


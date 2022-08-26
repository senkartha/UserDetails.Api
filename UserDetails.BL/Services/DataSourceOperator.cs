using UserDetailsBL.Interfaces;
using UserDetailsDL.Interfaces;

namespace UserDetailsBL.Services
{
	public class DataSourceOperator<T>: IDataSourceOperator<T>
    {
		IRepository<T> _repository;
		public DataSourceOperator(IRepository<T> repo)
		{
			this._repository = repo;
        }

        public async Task<bool> Save(T obj)
        {
            return await this._repository.Save(obj);
        }
    }
}


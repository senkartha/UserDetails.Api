namespace UserDetailsBL.Interfaces
{
    public interface IDataSourceOperator<T>
    {
        public Task<Boolean> Save(T obj);
    }
}

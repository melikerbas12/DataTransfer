using System.Collections.Generic;
using DataTransfer.Console.Entities;
using Npgsql;

namespace DataTransfer.Console.Service.Process
{
    public interface IService<T> where T : class
    {
        EntityResult<T> Insert(List<T> entities);
        EntityResult<T> Update(T entities);
        EntityResult<T> Delete(T entities);
        EntityResult<T> GetList();

    }
}
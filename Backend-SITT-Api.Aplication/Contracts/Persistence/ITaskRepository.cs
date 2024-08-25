using Backend_SITT_Api.Domain.Entityes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Contracts.Persistence
{
    public interface ITaskRepository
    {
        Task<Tasks> GetById(int id);
        Task<IEnumerable<Tasks>> ListTask();
        Task<bool> AddTask(Tasks objTask);
        Task<bool> UpdateTask(Tasks objTask);
        Task<bool> DeleteTask(Tasks objTask);
    }
}

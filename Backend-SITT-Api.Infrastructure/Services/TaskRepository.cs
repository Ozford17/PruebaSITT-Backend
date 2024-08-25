using Backend_SITT_Api.Aplication.Contracts.Persistence;
using Backend_SITT_Api.Domain.Entityes;
using Backend_SITT_Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Infrastructure.Services
{
    public class TaskRepository : ITaskRepository
    {
        public readonly BackendSittDbContext _context;
        public readonly DbSet<Tasks> tasks;

        public TaskRepository(BackendSittDbContext context)
        {
            _context = context;
            this.tasks = _context.Set<Tasks>();
        }

        public async Task<Tasks> GetById(int id)
            => this.tasks.Where(x=> x.Id.Equals(id)).FirstOrDefault();

        public async Task<IEnumerable<Tasks>> ListTask()
            => this.tasks.Where(x=>x.IsActive== true).ToList(); 

        public async Task<bool> AddTask (Tasks objTask)
        {
            try
            {
                this.tasks.Add(objTask);
                await this._context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateTask (Tasks objTask)
        {
           

            var obj = this.tasks.Where(x => x.Id == objTask.Id).SingleOrDefault();
            if (obj == null)
                return false;
            obj.Name = objTask.Name;
            obj.Description = objTask.Description;
            obj.Completed = objTask.Completed;
            obj.UserId = objTask.UserId;

            await this._context.SaveChangesAsync();
            return true; 
        }

        public async Task<bool> DeleteTask(Tasks objTask)
        {


            var obj = this.tasks.Where(x => x.Id == objTask.Id && x.UserId .Equals(objTask.UserId)).SingleOrDefault();
            if (obj == null)
                return false;
            obj.IsActive= false;
            
            await this._context.SaveChangesAsync();
            return true;



        }
    }
}

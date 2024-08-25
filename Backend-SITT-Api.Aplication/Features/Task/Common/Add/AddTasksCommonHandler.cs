using Backend_SITT_Api.Aplication.Contracts.Persistence;
using Backend_SITT_Api.Domain.Entityes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Common.Add
{
    public class AddTasksCommonHandler : IRequestHandler<AddTasksCommon, bool>
    {
        public readonly ITaskRepository _taskRepository;

        public AddTasksCommonHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async  Task<bool> Handle(AddTasksCommon request, CancellationToken cancellationToken)
        {
            
            Tasks objtask = new Tasks { 
                Name= request.Name,
                Description= request.Description,
                Completed = request.Completed,
                UserId = request.UserId
            };
            var ress = await this._taskRepository.AddTask(objtask);

            return ress;


        }
    }
}

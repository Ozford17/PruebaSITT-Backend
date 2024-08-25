using Backend_SITT_Api.Aplication.Contracts.Persistence;
using Backend_SITT_Api.Domain.Entityes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Common.Update
{
    public  class UpdateTasksCommonHandler: IRequestHandler<UpdateTasksCommon , bool>
    {
        public readonly ITaskRepository _taskRepository;

        public UpdateTasksCommonHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(UpdateTasksCommon request, CancellationToken cancellationToken)
        {
            Tasks objtask = new Tasks
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Completed = request.Completed,  
                UserId = request.UserId
            };

            var ress = await this._taskRepository.UpdateTask(objtask);
            
            return true;
        }
    }
}

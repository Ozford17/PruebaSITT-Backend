using Backend_SITT_Api.Aplication.Contracts.Persistence;
using Backend_SITT_Api.Domain.Entityes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Common.Delete
{
    public class DeleteTaskCommonHandler : IRequestHandler<DeleteTaskCommon, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommonHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<bool> Handle(DeleteTaskCommon request, CancellationToken cancellationToken)
        {
            var objTask = new Tasks
            {
                Id = request.Id,
                UserId = Guid.Parse(request.UserId)
            };
            var resp= this._taskRepository.DeleteTask(objTask);
            return resp;
        }
    }
}

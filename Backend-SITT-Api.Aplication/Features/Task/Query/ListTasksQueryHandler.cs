using Backend_SITT_Api.Aplication.Contracts.Identity;
using Backend_SITT_Api.Aplication.Contracts.Persistence;
using Backend_SITT_Api.Aplication.Models.ViewModel;
using Backend_SITT_Api.Domain.Entityes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Query
{
    public  class ListTasksQueryHandler : IRequestHandler<ListTasksQuery, IEnumerable<TasksVM> >
    {
        public readonly ITaskRepository _taskRepository;
        public readonly IAuthService _authService;

        public ListTasksQueryHandler(ITaskRepository taskRepository, IAuthService authService)
        {
            _taskRepository = taskRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<TasksVM>> Handle(ListTasksQuery request, CancellationToken cancellationToken)
        {
            var listTask = await this._taskRepository.ListTask();

            var List = new List<TasksVM>();
            foreach (var task in listTask)
            {
                var user =  await (this._authService.GetUserById(task.UserId));
                List.Add(new TasksVM
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Completed = task.Completed,
                    UserName = user.ToString(),
                    UserId = task.UserId.ToString(),

                });
            }
            return List;
        }
    }
}

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
    public  class ListTasksQuery : IRequest<IEnumerable<TasksVM>>
    {
    }
}

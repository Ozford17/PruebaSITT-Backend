using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Common.Add
{
    public  class AddTasksCommon: IRequest<bool>
    {
        public string  Name { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public Guid UserId {  get; set; }

    }
}

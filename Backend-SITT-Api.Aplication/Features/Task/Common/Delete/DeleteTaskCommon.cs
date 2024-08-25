using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Features.Task.Common.Delete
{
    public class DeleteTaskCommon : IRequest<bool>
    {
        public int Id { get; set; }
        public string UserId{ get; set; }
    }
}

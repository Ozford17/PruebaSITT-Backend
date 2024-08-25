using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Backend_SITT_Api.Aplication.Features
{
    public class AuditProps
    {

        [JsonIgnore]
        public string? CurrentUserName { get; set; }
        [JsonIgnore]
        public Guid? CurrentUserId { get; set; }
        /// <summary>
        /// Retieve CurrentUser Email to build email flows
        /// </summary>
        [JsonIgnore]
        public string? CurrentUserEmail { get; set; }
    }
}

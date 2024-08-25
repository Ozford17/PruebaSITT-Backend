using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Backend_SITT_Api.Aplication.Features;

namespace Backend_SITT_Api.Aplication.Models.Identity
{
     
    public class RegistrationRequest : AuditProps
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Password { get; set; }   
        public string Email { get; set; }
             

    }
        
}

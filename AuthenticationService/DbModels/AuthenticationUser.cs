using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AuthenticationService.DbModels
{
    public class AuthenticationUser
    {
        [Key]
        public string username { get; set; }

        public byte[] password { get; set; }

        public string salt { get; set; }

        public Guid temp_token { get; set; }

        public DateTime temp_token_date { get; set; }
    }
}

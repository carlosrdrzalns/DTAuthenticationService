using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AuthenticationService.DbModels
{
    public class AuthenticationDevice
    {
        [Key]
        public Guid id { get; set; }

        public string DeviceName { get; set; }

        public byte[] DeviceKey { get; set; }

        public string Salt { get; set; }

    }
}

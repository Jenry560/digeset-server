using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace digeset_server.Core.contracts
{
    public class LoginRequest
    {
        public string Cedula { get; set; }

        public string Clave {  get; set; }
    }
}

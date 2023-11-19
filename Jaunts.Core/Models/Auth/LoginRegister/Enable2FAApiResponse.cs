using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaunts.Core.Models.Auth.LoginRegister
{
    public class Enable2FAApiResponse<T>
    {

        public bool Successful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
    }
}

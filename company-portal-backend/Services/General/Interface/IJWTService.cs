using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.General.Interface
{
    public interface IJWTService
    {
        public string GenerateToken(DAL.Models.Company model);
    }
}

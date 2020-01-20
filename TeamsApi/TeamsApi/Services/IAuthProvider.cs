using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApi.Models;

namespace TeamsApi.Repository
{
    public interface IAuthProvider
    {
        Task<string> GetAccessTokenAsync();
    }
}

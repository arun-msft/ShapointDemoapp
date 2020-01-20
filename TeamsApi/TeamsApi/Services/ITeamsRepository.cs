using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsApi.Models;

namespace TeamsApi.Repository
{
    public interface ITeamsRepository
    {
        
        //Task<Team> CreateGroup<Team>(string TeamName);
        Task<Team> CreateTeamandChannel(string TeamName, string Channels);
        Task<string> AddMember(string EmailId,string groupid);
    }
}

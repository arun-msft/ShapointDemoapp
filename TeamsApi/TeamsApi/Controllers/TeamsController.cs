using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using TeamsApi.Models;
using TeamsApi.Repository;

namespace TeamsApi.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    [EnableCors("AllowAll")]
    public class TeamsController : Controller
    {

        private readonly ITeamsRepository _repo;
        private readonly IConfiguration config;

        public TeamsController(ITeamsRepository repositoty, IConfiguration iconfig)
        {
            _repo = repositoty;
            config = iconfig;
        }

        [Route("Team/Create")]
        [HttpGet]
        public async Task<Team> CreateTeamandChannel(string TeamName,string Channels)
        {
            Team data = await _repo.CreateTeamandChannel(TeamName, Channels);
          
            return data;
        }
        [Route("Team/AddMember")]
        [HttpGet]
        public async Task<string> AddMember(string EmailId,string groupid)
        {
            string data = await _repo.AddMember(EmailId, groupid);

            return data;
        }
    }
}
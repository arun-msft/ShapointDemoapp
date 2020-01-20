using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeamsApi.Models;

namespace TeamsApi.Repository
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly IAuthProvider _authProvider;
        private readonly string _ownerId;
        private readonly string _graphUrl;
        private readonly IConfiguration _configuration;
        private string accessToken;
        
        public TeamsRepository(IAuthProvider authProvider, IConfiguration configuration)
        {
            _authProvider = authProvider;
            _configuration = configuration;
            _graphUrl = configuration["GraphUrl"];
            _ownerId = configuration["ownerId"];
        }

        public TeamsRepository()
        {
        }

        //to get users id 
        public async Task<string> GetMyId(string EmailId)
        {
            string endpoint = _graphUrl+"users/"+ EmailId;
            string queryParameter = "?$select=id";
            String userId = "";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (accessToken == null)
            {
                accessToken=await _authProvider.GetAccessTokenAsync();
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(endpoint+queryParameter);
            var responseString = await response.Content.ReadAsStringAsync();
              if (responseString != null && response.IsSuccessStatusCode)
            {
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                userId = json.GetValue("id").ToString();
            }
            return userId?.Trim();
        }

        //create group before creating channel
        public async Task<Group> CreateGroup<Group>(string TeamName)
        {
            try
            {
                string createGroupurl = _graphUrl + "groups";
                GroupRequest body = new GroupRequest();
                body.description = "Self help community for library";
                body.displayName = TeamName + "_Group1";
                var groupTypes = new string[1];
                groupTypes[0] = "Unified";
                body.groupTypes = groupTypes;
                body.mailEnabled = true;
                body.mailNickname = TeamName + "_Group12";
                body.securityEnabled = false;
                string me = await GetMyId(_ownerId);
                string payload = $"{{ 'owners@odata.bind': '{_graphUrl}users/{me}' }}";
                if (accessToken == null)
                {
                    accessToken=await _authProvider.GetAccessTokenAsync();
                }
                HttpClient httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, createGroupurl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = (new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Group>(responseString);



                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        //Create team and channels
        public async Task<Team> CreateTeamandChannel(string TeamName, string Channels)
        {
            try
            {
                Group createdGroup = await CreateGroup<Group>(TeamName);
                await AddOwner(createdGroup.id);
                string createTeamUrl = _graphUrl+ "groups/"+ createdGroup.id + "/team";
                TeamRequest body = new TeamRequest();
                body.memberSettings = new Membersettings();
                body.memberSettings.allowCreateUpdateChannels=  true;
                body.messagingSettings = new Messagingsettings();
                body.messagingSettings.allowUserDeleteMessages = true;
                body.messagingSettings.allowUserEditMessages = true;
                body.funSettings = new Funsettings();
                body.funSettings.allowGiphy = true;
                body.funSettings.giphyContentRating = "strict";
                //for adding channels with teams not functioning helpful in future
                //Channel channel1 = new Channel();
                //channel1.displayName = "Channel1";
                //channel1.IsFavoriteByDefault = false;
                //body.Channels = new List<Channel>();
                //body.Channels.Add(channel1);
                string me = await GetMyId(_ownerId);
                string templatepayload = $"{{ 'template@odata.bind': '{_graphUrl}teamsTemplates('healthcareHospital')' }}";
                string userpayload = $"{{ 'owners@odata.bind': '{_graphUrl}users/{me}' }}";
                if (accessToken == null)
                {
                    accessToken=await _authProvider.GetAccessTokenAsync();
                }
                HttpClient httpClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, createTeamUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = (new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken );

                var response = await httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Team>(responseString);
                var channels = Channels.Split(',');
                foreach(var channel in channels)
                {
                    string channelrquesturi = _graphUrl + "teams/" + result.id + "/channels";
                    var channelrequest = new ChannelRequest();
                    channelrequest.displayName = channel;
                    channelrequest.description = "description for channel " + channel;
                    channelrequest.membershipType = "standard";
                    HttpClient httpClientchannel = new HttpClient();
                    var clientchannelrequest = new HttpRequestMessage(HttpMethod.Post, channelrquesturi);
                    httpClientchannel.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    clientchannelrequest.Content = (new StringContent(JsonConvert.SerializeObject(channelrequest), Encoding.UTF8, "application/json"));
                    httpClientchannel.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var channelresponse = await httpClient.SendAsync(clientchannelrequest);

                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        //Add owner to group
        public async Task AddOwner(string groupid)
        {   
            string addownerurl=_graphUrl+ "groups/"+ groupid +"/owners/$ref";
            string me = await GetMyId(_ownerId);
            string userpayload = $"{{ '@odata.id': '{_graphUrl}users/{me}' }}";

            if (accessToken == null)
            {
                accessToken=await _authProvider.GetAccessTokenAsync();
            }

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, addownerurl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = (new StringContent(userpayload, Encoding.UTF8, "application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Team>(responseString);
           
        }

        public async Task<string> AddMember(string EmailId,string groupid)
        {
            string me = await GetMyId(EmailId);
            if (me == "")
            {
                me = await inviteUser(EmailId);
            }
            string addownerurl = _graphUrl + "groups/" + groupid + "/members/$ref";
            string userpayload = $"{{ '@odata.id': '{_graphUrl}directoryObjects/{me}' }}";
            if (accessToken == null)
            {
                accessToken = await _authProvider.GetAccessTokenAsync();
            }
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, addownerurl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = (new StringContent(userpayload, Encoding.UTF8, "application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Team>(responseString);
            return me;
        }

        public async Task<string> inviteUser(string EmailId)
        {
            string addownerurl = _graphUrl + "invitations";

            JObject userpayload = new JObject();
            userpayload["invitedUserEmailAddress"] = EmailId;
            userpayload["inviteRedirectUrl"] = "https://teams.microsoft.com";
            userpayload["sendInvitationMessage"] = true;
            if (accessToken == null)
            {
                accessToken=await _authProvider.GetAccessTokenAsync();
            }
            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, addownerurl);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = (new StringContent(JsonConvert.SerializeObject(userpayload), Encoding.UTF8, "application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JObject>(responseString);
            return result["invitedUser"]["id"].ToString();
        }


    }
}
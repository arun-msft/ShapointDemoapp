using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TeamsApi.Models
{
    public class TeamRequest
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }

        //public List<Channel> Channels { get; set; }

        public Membersettings memberSettings { get; set; }
        public Messagingsettings messagingSettings { get; set; }
        public Funsettings funSettings { get; set; }
    }
    public class Membersettings
    {
        public bool allowCreateUpdateChannels { get; set; }
    }

    public class Messagingsettings
    {
        public bool allowUserEditMessages { get; set; }
        public bool allowUserDeleteMessages { get; set; }
    }

    public class Funsettings
    {
        public bool allowGiphy { get; set; }
        public string giphyContentRating { get; set; }
    }

   

    public class Channel
    {
        public string displayName { get; set; }
        public bool IsFavoriteByDefault { get; set; }
    }

    public class GroupRequest
    {
        public string description { get; set; }
        public string displayName { get; set; }
        public string[] groupTypes { get; set; }
        public bool mailEnabled { get; set; }
        public string mailNickname { get; set; }
        public bool securityEnabled { get; set; }
    }


}

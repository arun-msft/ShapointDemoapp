using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamsApi.Models
{
    public class Group
    {
        public string id { get; set; }
        public object deletedDateTime { get; set; }
        public object classification { get; set; }
        public DateTime createdDateTime { get; set; }
        public object[] creationOptions { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
        public string[] groupTypes { get; set; }
        public string mail { get; set; }
        public bool mailEnabled { get; set; }
        public string mailNickname { get; set; }
        public object onPremisesLastSyncDateTime { get; set; }
        public object onPremisesSecurityIdentifier { get; set; }
        public object onPremisesSyncEnabled { get; set; }
        public string preferredDataLocation { get; set; }
        public string[] proxyAddresses { get; set; }
        public DateTime renewedDateTime { get; set; }
        public object[] resourceBehaviorOptions { get; set; }
        public object[] resourceProvisioningOptions { get; set; }
        public bool securityEnabled { get; set; }
        public string visibility { get; set; }
        public object[] onPremisesProvisioningErrors { get; set; }
    }



    public class Team
    {
        public string odatacontext { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string internalId { get; set; }
        public object classification { get; set; }
        public object specialization { get; set; }
        public object visibility { get; set; }
        public string webUrl { get; set; }
        public object isArchived { get; set; }
        public object discoverySettings { get; set; }
        public GroupMembersettings memberSettings { get; set; }
        public GroupGuestsettings guestSettings { get; set; }
        public GroupMessagingsettings messagingSettings { get; set; }
        public GroupFunsettings funSettings { get; set; }
    }

    public class GroupMembersettings
    {
        public bool allowCreateUpdateChannels { get; set; }
        public bool allowDeleteChannels { get; set; }
        public bool allowAddRemoveApps { get; set; }
        public bool allowCreateUpdateRemoveTabs { get; set; }
        public bool allowCreateUpdateRemoveConnectors { get; set; }
    }

    public class GroupGuestsettings
    {
        public bool allowCreateUpdateChannels { get; set; }
        public bool allowDeleteChannels { get; set; }
    }

    public class GroupMessagingsettings
    {
        public bool allowUserEditMessages { get; set; }
        public bool allowUserDeleteMessages { get; set; }
        public bool allowOwnerDeleteMessages { get; set; }
        public bool allowTeamMentions { get; set; }
        public bool allowChannelMentions { get; set; }
    }

    public class GroupFunsettings
    {
        public bool allowGiphy { get; set; }
        public string giphyContentRating { get; set; }
        public bool allowStickersAndMemes { get; set; }
        public bool allowCustomMemes { get; set; }
    }


    public class ChannelRequest
    {
        public string displayName { get; set; }
        public string description { get; set; }
        public string membershipType { get; set; }
    }




}

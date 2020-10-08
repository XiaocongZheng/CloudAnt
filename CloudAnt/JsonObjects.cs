using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAnt
{
    class LgObject
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string access_token { get; set; }
    }

    public class ChannelItem
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string MemberShipType { get; set; }
        public string isFavoriteByDefault { get; set; }
        public string webUrl { get; set; }
        public string email { get; set; }
    }


    public class TeamItem
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string internalId { get; set; }
        public string classification { get; set; }
        public string specialization { get; set; }
        public string visibility { get; set; }
        public string webUrl { get; set; }
        public string isArchived { get; set; }
        public string memberSettings { get; set; }
        public string guestSettings { get; set; }
        public string messagingSettings { get; set; }
        public string funSettings { get; set; }
        public string discoverySettings { get; set; }

    }
    public class GetTeamItem
    {
        public string id { get; set; }
        public string displayName { get; set; }
    }

    class MyTeamObject
    {
        public string context { get; set; }
        public string count { get; set; }
        public List<TeamItem> value { get; set; }

    }

    class MyChannelObject
    {
        public List<ChannelItem> value { get; set; }

    }


    class MyChanMesageObject
    {
        public List<MesageItem> value { get; set; }

    }

    public class MyUserObject
    {
        public List<UserInfoItem> value { get; set; }

    }

    public class UserInfoItem
    {
        public string[] businessPhones { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mobilePhone { get; set; }
        public string officeLocation { get; set; }
        public string preferredLanguage { get; set; }
        public string surname { get; set; }
        public string userPrincipalName { get; set; }
        public string id { get; set; }
    }


    public class MesageItem
    {
        public Int64 id { get; set; }
        public string replyToId { get; set; }
        public string etag { get; set; }
        public string messageType { get; set; }
        public string createdDateTime { get; set; }
        public string lastModifiedDateTime { get; set; }
        public string deletedDateTime { get; set; }
        public string subject { get; set; }
        public string summary { get; set; }
        public string importance { get; set; }
        public string locale { get; set; }
        public string policyViolation { get; set; }
        public FromItem From { get; set; }
        public BodyItem body { get; set; }
        public List<AttaItem> attachments { get; set; }
        public List<MenItem> mentions { get; set; }
        public List<ReactItem> reactions { get; set; }
    }


    public class FromItem
    {
        public string application { get; set; }
        public string device { get; set; }
        public string conversation { get; set; }
        public UserItem user { get; set; }

    }

    public class UserItem
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string userIdentityType { get; set; }
    }


    public class BodyItem
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class AttaItem
    {
        public string id { get; set; }
        public string contentType { get; set; }
        public string contentUrl { get; set; }
        public ContentItem content { get; set; }
        public string name { get; set; }
        public string thumbnailUrl { get; set; }
    }

    public class ContentItem
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string text { get; set; }
        public string buttons { get; set; }
    }

    public class MenItem
    {
        public string id { get; set; }
        public string mentionText { get; set; }
        public FromItem mentioned { get; set; }
    }

    public class ReactItem
    {
        public string reactionType { get; set; }
        public string createdDateTime { get; set; }
        public FromItem user { get; set; }
    }

    public class MapMenUser
    {
        public string fid { get; set; }
        public string fdisname { get; set; }
        public string femail { get; set; }
        public string tid { get; set; }
        public string tdisname { get; set; }
        public string temail { get; set; }
    }

    public class MapUser
    {
        public string femail { get; set; }
        public string temail { get; set; }
    }

}

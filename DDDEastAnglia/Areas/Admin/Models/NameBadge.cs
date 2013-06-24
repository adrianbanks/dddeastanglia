namespace DDDEastAnglia.Areas.Admin.Models
{
    public class NameBadge
    {
        public string TicketId{get;set;}
        public string FullName{get;set;} 
        public bool HasTwitterInfo{get {return !string.IsNullOrWhiteSpace(TwitterHandle);}}
        public string TwitterHandle{get;set;}
        public string TwitterImageUrl{get;set;}
        public bool FirstTimer{get;set;}
    }
}

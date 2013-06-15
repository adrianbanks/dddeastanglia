using System.Collections.Generic;

namespace DDDEastAnglia.Areas.Admin.Models.Eventbrite
{
    public class AttendeeList
    {
        public List<AttendeeContainer> attendees{get;set;}
    }

    public class AttendeeContainer
    {
        public Attendee attendee{get;set;}
    }

    public class Attendee
    {
        public string first_name{get;set;}
        public string last_name{get;set;}
        public string order_type{get;set;}
        public string created{get;set;}
        public long order_id{get;set;}
        public string amount_paid{get;set;}
        public string barcode{get;set;}
        public string modified{get;set;}
        public List<AnswerContainer> answers{get;set;}
        public string email{get;set;}
        public string currency{get;set;}
        public string affiliate{get;set;}
        public long ticket_id{get;set;}
        public long event_id{get;set;}
        public string event_date{get;set;}
        public string discount{get;set;}
        public long id{get;set;}
        public int quantity{get;set;}
    }

    public class AnswerContainer
    {
        public Answer answer{get;set;}
    }
    public class Answer
    {
        public string answer_text{get;set;}
        public string question{get;set;}
        public string question_type{get;set;}
        public long question_id{get;set;}
    }
}

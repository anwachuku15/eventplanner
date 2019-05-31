using System;
using System.ComponentModel.DataAnnotations;

namespace CsharpBelt.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public int UserId { set; get; }
        public int ActivityId { set; get; }
        public User User { get; set; }
        public Activityy Activity { get; set; }
    }
    
}
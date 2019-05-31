using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CsharpBelt.Models
{
    public class Activityy
    {
        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value != null && (DateTime)value > DateTime.Now;
            }
        }

        [Key]
        public int ActivityId {get;set;}

        [Required(ErrorMessage="Title required")]
        [Display(Name="Title:")]
        public string Name {get;set;}

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage="Date should be in the future.")]
        public DateTime Date {get;set;}

        [Required]
        public string Duration {get;set;}
        
        [Required]
        public string Description {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int CoordinatorId {get;set;}

        public List<Participant> EventInfo {get;set;}
        

    }
}
using System;

namespace DataTransfer.Console.Entities
{
    public class AdminAuthorityLanguage
    {
        public int Id { get; set; }        
        public int AdminAuthorityId { get; set; } 
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }    
        public string CreatedBy { get; set; }    
    }
}
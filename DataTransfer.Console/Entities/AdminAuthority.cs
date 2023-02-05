using System;
using System.Collections.Generic;

namespace DataTransfer.Console.Entities
{
    public class AdminAuthority
    {
        public int Id { get; set; }    
        public string Name { get; set; }    
        public string Code { get; set; }
        public int ParentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public List<AdminAuthorityLanguage> AdminAuthorityLanguages { get; set; } = new List<AdminAuthorityLanguage>();
    }
}
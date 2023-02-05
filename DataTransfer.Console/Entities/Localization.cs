using System;

namespace DataTransfer.Console.Entities
{
   public class Localization
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsHide { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
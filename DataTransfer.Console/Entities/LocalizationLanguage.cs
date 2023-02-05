using System;

namespace DataTransfer.Console.Entities
{
    public class LocalizationLanguage
    {
        public int Id { get; set; }
        public string LocalizationCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
}
using DataTransfer.Console.Entities;

namespace DataTransfer.Console.Service.Process
{
    public interface ILocalizationLanguageService : IService<LocalizationLanguage>
    {
        bool GetByKey(int languageId, string localizationCode);
    }
}
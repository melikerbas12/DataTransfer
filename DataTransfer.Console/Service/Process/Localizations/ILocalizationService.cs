using DataTransfer.Console.Entities;
using DataTransfer.Console.Service.Process;

namespace DataTransfer.Console.Service.Process
{
    public interface ILocalizationService : IService<Localization>
    {
        bool GetByKey(string key);
    }
}
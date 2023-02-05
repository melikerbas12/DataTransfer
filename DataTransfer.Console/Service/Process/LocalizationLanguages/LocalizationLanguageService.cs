using System;
using System.Collections.Generic;
using DataTransfer.Console.Entities;
using Npgsql;
namespace DataTransfer.Console.Service.Process
{
    public class LocalizationLanguageService : StateConnection, ILocalizationLanguageService
    {
        public EntityResult<LocalizationLanguage> Delete(LocalizationLanguage entities)
        {
            throw new NotImplementedException();
        }

        public bool GetByKey(int languageId, string localizationCode)
        {
            var command = new NpgsqlCommand("SELECT * FROM \"LocalizationLanguage\" WHERE \"LocalizationCode\" = @localizationCode and \"LanguageId\" = @languageId", DestinationConnectionOpen());
            command.Parameters.AddWithValue("@languageId", languageId);
            command.Parameters.AddWithValue("@localizationCode", localizationCode);

            var count = command.ExecuteReader().HasRows;

            DestinationConnectionClosed();

            return count;
        }

        public EntityResult<LocalizationLanguage> GetList()
        {
            var result = new EntityResult<LocalizationLanguage>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                var localizationLanguages = new List<LocalizationLanguage>();
                var command = new NpgsqlCommand("SELECT * FROM \"LocalizationLanguage\"", SourceConnectionOpen());
                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    var localizationLanguage = new LocalizationLanguage();
                    localizationLanguage.Id = (int)reader["Id"];
                    localizationLanguage.LocalizationCode = reader["LocalizationCode"] != DBNull.Value ? (string)reader["LocalizationCode"] : "";
                    localizationLanguage.IsActive = (bool)reader["IsActive"];
                    localizationLanguage.LanguageId = (int)reader["LanguageId"];
                    localizationLanguage.Text = reader["Text"] != DBNull.Value ? (string)reader["Text"] : "";

                    localizationLanguages.Add(localizationLanguage);

                }

                result.Objects = localizationLanguages;

                SourceConnectionClosed();
            }

            catch (Exception ex)
            {

                result.Result = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        public EntityResult<LocalizationLanguage> Insert(List<LocalizationLanguage> entities)
        {
            var sayac = 0;
            var result = new EntityResult<LocalizationLanguage>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                foreach (var item in entities)
                {

                    var reader = GetByKey(item.LanguageId, item.LocalizationCode);

                    if (!reader)
                    {
                        sayac++;
                        var command = new NpgsqlCommand("INSERT INTO \"LocalizationLanguage\" (\"LocalizationCode\",\"CreatedOn\",\"IsActive\",\"LanguageId\",\"Text\") VALUES (@localizationCode,@createdOn,@isActive,@languageId,@text)", DestinationConnectionOpen());
                        command.Parameters.AddWithValue("@localizationCode", item.LocalizationCode);
                        command.Parameters.AddWithValue("@createdOn", item.CreatedOn);
                        command.Parameters.AddWithValue("@isActive", item.IsActive);
                        command.Parameters.AddWithValue("@languageId", item.LanguageId);
                        command.Parameters.AddWithValue("@text", item.Text);

                        var response = command.ExecuteNonQuery();

                        if (response > 0)
                        {
                            System.Console.WriteLine("" + item.LocalizationCode + ""+item.LanguageId+" numaralı id'ye sahip dil ile başırılı bir şekilde Localization Language tablosuna eklenmiştir.");
                        }

                        else
                        {
                            System.Console.WriteLine("" + item.LocalizationCode  + ""+item.LanguageId+" numaralı id'ye sahip dil ile Localization Language tablosuna eklenirken hata oluştu.");
                        }

                    }
                }
                
                if (sayac == 0)
                {
                    System.Console.WriteLine("Eklenecek yeni data yoktur.");
                }

                DestinationConnectionClosed();
            }

            catch (Exception ex)
            {

                result.Result = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        public EntityResult<LocalizationLanguage> Update(LocalizationLanguage entities)
        {
            throw new NotImplementedException();
        }
    }
}
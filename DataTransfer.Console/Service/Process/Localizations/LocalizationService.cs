using System;
using System.Collections.Generic;
using DataTransfer.Console.Entities;
using Npgsql;

namespace DataTransfer.Console.Service.Process
{

    public class LocalizationService : StateConnection, ILocalizationService
    {
        public EntityResult<Localization> Delete(Localization entities)
        {
            throw new System.NotImplementedException();
        }

        public bool GetByKey(string key)
        {
            var command = new NpgsqlCommand("SELECT * FROM \"Localization\" WHERE \"Key\" = @key", DestinationConnectionOpen());
            command.Parameters.AddWithValue("@key", key);

            var count = command.ExecuteReader().HasRows;
            DestinationConnectionClosed();

            return count;
        }

        public EntityResult<Localization> GetList()
        {
            var result = new EntityResult<Localization>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                var localizations = new List<Localization>();
                var command = new NpgsqlCommand("SELECT * FROM \"Localization\"", SourceConnectionOpen());
                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    var localization = new Localization();
                    localization.Key = reader["Key"] != DBNull.Value ? (string)reader["Key"] : "";
                    localization.Value = reader["Value"] != DBNull.Value ? (string)reader["Value"] : "";
                    localization.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : "";
                    localization.IsHide = (bool)reader["IsHide"];
                    localization.CreatedOn = DateTime.Now;
                    localization.IsActive = (bool)reader["IsActive"];

                    localizations.Add(localization);

                }

                result.Objects = localizations;

                SourceConnectionClosed();
            }

            catch (Exception ex)
            {

                result.Result = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        public EntityResult<Localization> Insert(List<Localization> entities)
        {
            var sayac = 0;
            var result = new EntityResult<Localization>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                foreach (var item in entities)
                {
                    var reader = GetByKey(item.Key);

                    if (!reader)
                    {
                        sayac++;
                        var command = new NpgsqlCommand("INSERT INTO \"Localization\" (\"Key\",\"Value\",\"Description\",\"IsHide\",\"CreatedOn\",\"IsActive\") VALUES (@key,@value,@description,@isHide,@createdOn,@isActive)", DestinationConnectionOpen());
                        command.Parameters.AddWithValue("@key", item.Key);
                        command.Parameters.AddWithValue("@value", item.Value);
                        command.Parameters.AddWithValue("@description", item.Description);
                        command.Parameters.AddWithValue("@isHide", item.IsHide);
                        command.Parameters.AddWithValue("@createdOn", item.CreatedOn);
                        command.Parameters.AddWithValue("@isActive", item.IsActive);

                        var response = command.ExecuteNonQuery();

                        if (response > 0)
                        {
                            System.Console.WriteLine("" + item.Key + " başırılı bir şekilde Localization tablosuna eklenmiştir.");
                        }

                        else
                        {
                            System.Console.WriteLine("" + item.Key + " Localization tablosuna eklenirken hata oluştu.");
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

        public EntityResult<Localization> Update(Localization entities)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using DataTransfer.Console.Entities;
using Npgsql;

namespace DataTransfer.Console.Service.Process
{
    public class AdminAuthorityLanguageService : StateConnection, IAdminAuthorityLanguageService
    {
        public EntityResult<AdminAuthorityLanguage> Delete(AdminAuthorityLanguage entities)
        {
            throw new System.NotImplementedException();
        }

        public EntityResult<AdminAuthorityLanguage> GetList()
        {
            var result = new EntityResult<AdminAuthorityLanguage>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                var adminAuthorityLanguages = new List<AdminAuthorityLanguage>();
                var command = new NpgsqlCommand("SELECT * FROM \"AdminAuthorityLanguage\" where \"LanguageId\" between 1 and 2", SourceConnectionOpen());

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var adminAuthorityLanguage = new AdminAuthorityLanguage();
                    adminAuthorityLanguage.Id = (int)reader["Id"];
                    adminAuthorityLanguage.AdminAuthorityId = (int)reader["AdminAuthorityId"];
                    adminAuthorityLanguage.CreatedOn = DateTime.Now;
                    adminAuthorityLanguage.IsActive = (bool)reader["IsActive"];
                    adminAuthorityLanguage.LanguageId = (int)reader["LanguageId"];
                    adminAuthorityLanguage.Text = reader["Text"] != DBNull.Value ? (string)reader["Text"] : "";
                    adminAuthorityLanguage.CreatedBy = reader["CreatedBy"] != DBNull.Value ? (string)reader["CreatedBy"] : "";

                    adminAuthorityLanguages.Add(adminAuthorityLanguage);
                }

                result.Objects = adminAuthorityLanguages;

                SourceConnectionClosed();
            }

            catch (Exception ex)
            {

                result.Result = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        public EntityResult<AdminAuthorityLanguage> Insert(List<AdminAuthorityLanguage> entities)
        {
            var result = new EntityResult<AdminAuthorityLanguage>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                if (entities.Count > 0)
                {

                    foreach (var item in entities)
                    {

                        var command = new NpgsqlCommand("INSERT INTO \"AdminAuthorityLanguage\" (\"AdminAuthorityId\",\"CreatedOn\",\"IsActive\",\"LanguageId\",\"Text\",\"CreatedBy\") VALUES (@adminAuthorityId,@createdOn,@isActive,@languageId,@text,@createdBy)", DestinationConnectionOpen());
                        command.Parameters.AddWithValue("@adminAuthorityId", item.AdminAuthorityId);
                        command.Parameters.AddWithValue("@createdOn", item.CreatedOn);
                        command.Parameters.AddWithValue("@isActive", item.IsActive);
                        command.Parameters.AddWithValue("@languageId", item.LanguageId);
                        command.Parameters.AddWithValue("@text", item.Text);
                        command.Parameters.AddWithValue("@createdBy", item.CreatedBy);
                        command.ExecuteNonQuery();

                    }
                }
                else
                {
                    System.Console.WriteLine("Admin Authority language' e ait data yoktur..");
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

        public EntityResult<AdminAuthorityLanguage> Update(AdminAuthorityLanguage entities)
        {
            throw new System.NotImplementedException();
        }

        private bool CountValidate()
        {
            var sourceCommand = new NpgsqlCommand("SELECT Count(*) FROM \"AdminAuthorityLanguage\" rows;", SourceConnectionOpen());
            var sourceCount = (long)sourceCommand.ExecuteScalar();
            SourceConnectionClosed();

            var destinationCommand = new NpgsqlCommand("SELECT Count(*) FROM \"AdminAuthorityLanguage\" rows;", DestinationConnectionOpen());
            var destinationCount = (long)destinationCommand.ExecuteScalar();
            DestinationConnectionClosed();

            if (sourceCount > destinationCount) return true;
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using DataTransfer.Console.Entities;
using Npgsql;

namespace DataTransfer.Console.Service.Process
{
    public class AdminAuthorityService : StateConnection, IAdminAuthorityService
    {
        private readonly IAdminAuthorityLanguageService _adminAuthorityLanguageService;
        public AdminAuthorityService(IAdminAuthorityLanguageService adminAuthorityLanguageService)
        {
            _adminAuthorityLanguageService = adminAuthorityLanguageService;
        }
        public Entities.EntityResult<AdminAuthority> Delete(AdminAuthority entities)
        {
            throw new System.NotImplementedException();
        }

        public Entities.EntityResult<AdminAuthority> GetList()
        {
            var result = new EntityResult<AdminAuthority>();
            result.Result = true;
            result.ErrorText = "success";

            try
            {
                var adminAuthorities = new List<AdminAuthority>();
                var command = new NpgsqlCommand("SELECT * FROM \"AdminAuthority\"", SourceConnectionOpen());

                var adminAuthorityLanguages = _adminAuthorityLanguageService.GetList().Objects;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var adminAuthority = new AdminAuthority();
                    adminAuthority.Id = (int)reader["Id"];
                    adminAuthority.Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : "";//reader.IsDBNull(1) ? "" : reader.GetString(1);
                    adminAuthority.Code = reader["Code"] != DBNull.Value ? (string)reader["Code"] : "";//reader.IsDBNull(2) ? "" : reader.GetString(2);
                    adminAuthority.ParentId = (int)reader["ParentId"];
                    adminAuthority.CreatedOn = DateTime.Now;
                    adminAuthority.IsActive = (bool)reader["IsActive"];
                    adminAuthority.CreatedBy = reader["CreatedBy"] != DBNull.Value ? (string)reader["CreatedBy"] : "";//reader.IsDBNull(9) ? "" : reader.GetString(9);

                    var data = adminAuthorityLanguages.Where(a => a.AdminAuthorityId == adminAuthority.Id);
                    adminAuthority.AdminAuthorityLanguages.AddRange(data);
                    adminAuthorities.Add(adminAuthority);
                }

                result.Objects = adminAuthorities;
                SourceConnectionClosed();
            }

            catch (Exception ex)
            {

                result.Result = false;
                result.ErrorText = ex.Message;
            }

            return result;
        }

        public Entities.EntityResult<AdminAuthority> Insert(System.Collections.Generic.List<AdminAuthority> entities)
        {
            var sayac = 0;
            var result = new EntityResult<AdminAuthority>();
            result.Result = true;
            result.ErrorText = "success";
            try
            {
                var validationResult = Validation(entities);

                if (validationResult == 0) // Insert
                {
                    if (CountValidate())
                    {

                        foreach (var item in entities)
                        {
                            if (!IsThereAuthorityByCode(item.Code))
                            {
                                sayac++;
                                var command = new NpgsqlCommand("INSERT INTO \"AdminAuthority\" (\"Name\",\"Code\",\"ParentId\",\"CreatedOn\",\"IsActive\",\"CreatedBy\") VALUES (@name,@code,@parentId,@createdOn,@isActive,@createdBy) returning \"Id\"", DestinationConnectionOpen());
                                command.Parameters.AddWithValue("@name", item.Name);
                                command.Parameters.AddWithValue("@code", item.Code);
                                command.Parameters.AddWithValue("@parentId", item.ParentId);
                                command.Parameters.AddWithValue("@createdOn", item.CreatedOn);
                                command.Parameters.AddWithValue("@isActive", item.IsActive);
                                command.Parameters.AddWithValue("@createdBy", item.CreatedBy);
                                var reader = command.ExecuteReader();

                                while (reader.Read())
                                {
                                    var authorityId = reader.GetInt32(0);

                                    if (authorityId > 0)
                                    {
                                        System.Console.WriteLine("" + item.Code + " başırılı bir şekilde Admin Authority tablosuna eklenmiştir.");
                                        item.AdminAuthorityLanguages.ForEach(x => x.AdminAuthorityId = authorityId);
                                        _adminAuthorityLanguageService.Insert(item.AdminAuthorityLanguages);
                                    }

                                    else
                                    {
                                        System.Console.WriteLine("" + item.Code + " Admin Authority tablosuna eklenirken hata oluştu.");
                                    }
                                }
                            }
                        }
                        if (sayac == 0)
                        {
                            System.Console.WriteLine("Eklenecek yeni data yoktur.");
                        }

                    }
                    else
                    {
                        System.Console.WriteLine("Localinizdeki data sayısı test ortamından daha az olduğu için AdminAuthorityLanguage tablosuna ekleme yapılamadı..");
                    }

                }
                else
                {
                    System.Console.WriteLine("Localinizde " + validationResult + " adet eşleşmeyen data vardır");
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

        public Entities.EntityResult<AdminAuthority> Update(AdminAuthority entities)
        {
            throw new System.NotImplementedException();
        }
        private bool IsThereAuthorityByCode(string code)
        {
            var command = new NpgsqlCommand("SELECT * FROM \"AdminAuthority\" WHERE \"Code\" = @code", DestinationConnectionOpen());
            command.Parameters.AddWithValue("@code", code);

            var hasRows = command.ExecuteReader().HasRows;
            DestinationConnectionClosed();

            return hasRows;
        }
        private bool IsThereAuthorityByCodeAndParentId(string code, int parentId)
        {
            var command = new NpgsqlCommand("SELECT * FROM \"AdminAuthority\" WHERE \"Code\" = @code and \"ParentId\" = @parentId", DestinationConnectionOpen());
            command.Parameters.AddWithValue("@code", code);
            command.Parameters.AddWithValue("@parentId", parentId);

            var hasRows = command.ExecuteReader().HasRows;
            DestinationConnectionClosed();

            return hasRows;
        }
        private int Validation(List<AdminAuthority> entities)
        {
            int sayac = 0;

            foreach (var item in entities)
            {
                if (IsThereAuthorityByCode(item.Code) && !IsThereAuthorityByCodeAndParentId(item.Code, item.ParentId))
                {
                    System.Console.WriteLine("Localinizde bulunan " + item.Code + " isimli code'un ParentId' si test ortamı ile eşleşmiyor..");
                    sayac++;
                }
            }

            return sayac;
        }
        private bool CountValidate()
        {
            var sourceCommand = new NpgsqlCommand("SELECT Count(*) FROM \"AdminAuthority\" rows;", SourceConnectionOpen());
            var sourceCount = (long)sourceCommand.ExecuteScalar();
            SourceConnectionClosed();

            var destinationCommand = new NpgsqlCommand("SELECT Count(*) FROM \"AdminAuthority\" rows;", DestinationConnectionOpen());
            var destinationCount = (long)destinationCommand.ExecuteScalar();
            DestinationConnectionClosed();

            if (sourceCount > destinationCount) return true;
            return false;
        }
    }
}
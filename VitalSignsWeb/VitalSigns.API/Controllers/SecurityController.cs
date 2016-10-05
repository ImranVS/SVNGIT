using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq.Expressions;
using MongoDB.Bson;


namespace VitalSigns.API.Controllers
{
    public class SecurityController : BaseController
    {
        private IRepository<MaintainUser> maintainUsersRepository;
        [HttpGet("maintain_users")]
        public APIResponse GetAllMaintainUsers()
        {
            maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);
            var result = maintainUsersRepository.All().Select(x => new MaintainUsersModel
            {

                Id = x.Id,
                LoginName = x.LoginName,
                FullName = x.FullName,
                Email = x.Email,
                Status = x.Status,
                SuperAdmin = x.SuperAdmin,
                ConfiguratorAccess = x.ConfiguratorAccess,
                ConsoleCommandAccess = x.ConsoleCommandAccess

            }).ToList();
            Response = Common.CreateResponse(result);
            return Response;
        }

        [HttpPut("save_maintain_users")]
        public APIResponse UpdateMaintainUsers([FromBody]MaintainUsersModel maintainuser)
        {
            try
            {
                maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);


                if (string.IsNullOrEmpty(maintainuser.Id))
                {
                    MaintainUser maintainUsers = new MaintainUser { LoginName = maintainuser.LoginName, FullName = maintainuser.FullName, Email = maintainuser.Email, Status = maintainuser.Status, SuperAdmin = maintainuser.SuperAdmin, ConfiguratorAccess = maintainuser.ConfiguratorAccess, ConsoleCommandAccess = maintainuser.ConsoleCommandAccess };
                    maintainUsersRepository.Insert(maintainUsers);
                    Response = Common.CreateResponse(true, "OK", "Maintain Users inserted successfully");
                }
                else
                {
                    FilterDefinition<MaintainUser> filterDefination = Builders<MaintainUser>.Filter.Where(p => p.Id == maintainuser.Id);
                    var updateDefination = maintainUsersRepository.Updater.Set(p => p.LoginName, maintainuser.LoginName)
                                                             .Set(p => p.FullName, maintainuser.FullName)
                                                             .Set(p => p.Email, maintainuser.Email)
                                                             .Set(p => p.Status, maintainuser.Status)
                                                             .Set(p => p.SuperAdmin, maintainuser.SuperAdmin)
                                                             .Set(p => p.ConfiguratorAccess, maintainuser.ConfiguratorAccess)
                                                             .Set(p => p.ConsoleCommandAccess, maintainuser.ConsoleCommandAccess);
                                                             
                    var result = maintainUsersRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "Maintain Users updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Maintain Users falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_maintain_users/{id}")]
        public void DeleteBusinessHours(string id)
        {
            maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);
            Expression<Func<MaintainUser, bool>> expression = (p => p.Id == id);
            maintainUsersRepository.Delete(expression);
        }

    }
}
            
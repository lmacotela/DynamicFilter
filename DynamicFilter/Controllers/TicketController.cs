using DynamicFilter.DTO;
using DynamicFilter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DynamicFilter.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class TicketController : ApiController
    {
        private DataContext db = new DataContext();

        public List<Models.Type> Get()
        {
            return db.Types.Where(x => x.Enable == true).ToList();
        }

        public async Task<Ticket_Response_v1> Post([FromBody]Ticket ticket)
        {
            var response = new Ticket_Response_v1();

            try
            {
                var user = await ValidateAndInsertOrUpdateUser(ticket.User);
                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Error al validar usuario";
                    return response;
                }

                ticket.Filter.CreatedBy = user.UserID;
                if (!await createTicket(ticket.Filter))
                {
                    response.IsSuccess = false;
                    response.Message = "Error al registrar consulta";
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "Consulta registrada correctamente";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
        private async Task<bool> createTicket(Models.Filter filter)
        {
            try
            {
                filter.Enable = true;
                filter.CreatedOn = DateTime.Today;
                filter.CategoryID = 2;
                filter.StateID = 1;
                db.Filters.Add(filter);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<User> ValidateAndInsertOrUpdateUser(Models.User user)
        {
            try
            {
                var proveedor = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                if (proveedor == null)
                {
                    user.Enable = true;
                    user.Password = string.Empty;
                    user.RoleID = 2;
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return user;
                }
                else
                {
                    Models.User usuario = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                    db.Entry(usuario).State = EntityState.Modified;
                    usuario.UserName = user.UserName;
                    usuario.ProveedorID = user.ProveedorID;
                    usuario.ProviderName = user.ProviderName;
                    usuario.ContactPerson = user.ContactPerson;
                    await db.SaveChangesAsync();
                    return usuario;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

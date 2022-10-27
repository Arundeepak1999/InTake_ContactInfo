using IntakeData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntakeData;
using Newtonsoft.Json;
using System.IO;

namespace InTake
{
    public class ContactMethod : ControllerBase
    {
        private readonly IConfiguration _configuration;
        ContactInformation per = new ContactInformation();
        SqlConnection con;
        public ContactMethod(IConfiguration configuration)
        {
            _configuration = configuration;
            con = new SqlConnection(_configuration.GetValue<string>("ConnectionStrings:IntakeDB"));
        }
        [FunctionName("GetContactDetails")]
        public async Task<object> GetContactdetails(
           [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Get), Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                ContactData Cont = new ContactData(_configuration);
                string dt = Cont.GetContact();
                return Ok(dt);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
        [FunctionName("PostContactDetails")]
        public async Task<IActionResult> PostContactdetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Post), Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ContactInformation>(requestBody);
            try
            {
                ContactData pcd = new ContactData(_configuration);
                string dt = pcd.PostContact(input);
                return Ok(dt);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
        [FunctionName("UpdateContactDetails")]
        public async Task<IActionResult> UpdateContactdetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Put), Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ContactInformation>(requestBody);
            try
            {
                ContactData pud = new ContactData(_configuration);
                string dt = pud.UpdateContact(input);
                return Ok(dt);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
        [FunctionName("DeleteContactDetails")]
        public async Task<IActionResult> DeleteContactdetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Delete), Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ContactInformation>(requestBody);
            try
            {
                ContactData pdd = new ContactData(_configuration);
                string dt = pdd.DeleteContact(input);
                return Ok(dt);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using IntakeData.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace InTake
{
    public class PatientMethod
    {
        private readonly IConfiguration _configuration;
        public string _connectionString = null;
        SqlConnection con;

        public PatientMethod(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionStrings:InTakeDB").Value;
        }
        
        [FunctionName("GetPatientDetails")]
        public async Task<List<PersonalInformation>> GetDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Get), Route = null)] HttpRequest req, ILogger log)
        {
            List<PersonalInformation> tasks = new List<PersonalInformation>();

            try
            {
                DataTable dt = new DataTable();
                string query = ("PersonalInfoGetAll");
                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(dt);
                }
                var data = JsonConvert.SerializeObject(dt);
                tasks = JsonConvert.DeserializeObject<List<PersonalInformation>>(data);
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return tasks;
        }
        [FunctionName("PostPatientDetails")]
        public async Task<IActionResult> InsertDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Post), Route = null)] HttpRequest req, ILogger log)
        {
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<PersonalInformation>(requestBody);
                try
                {

                    if (input != null)
                    {


                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            string msg = "";
                            SqlCommand cmd = new SqlCommand("PersonalInfoInsert", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@FirstName", input.FirstName));
                            cmd.Parameters.Add(new SqlParameter("@PreferredName", input.PreferredName));
                            cmd.Parameters.Add(new SqlParameter("@MiddleName", input.MiddleName));
                            cmd.Parameters.Add(new SqlParameter("@LastName", input.LastName));
                            cmd.Parameters.Add(new SqlParameter("@Suffiix", input.Suffiix));
                            cmd.Parameters.Add(new SqlParameter("@SSN", input.SSN));
                            cmd.Parameters.Add(new SqlParameter("@DateofBirth", input.DateofBirth));
                            cmd.Parameters.Add(new SqlParameter("@BirthSex", input.BirthSex));
                            cmd.Parameters.Add(new SqlParameter("@UpdatedBy", input.UpdatedBy));
                            cmd.Parameters.Add(new SqlParameter("@CreatedDate", input.CreatedDate));
                            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", input.UpdatedDate));
                            int i = cmd.ExecuteNonQuery();
                            connection.Close();
                            if (i > 0)
                            {
                                msg = "Data has been inserted";
                            }
                            else
                            {
                                msg = "Error";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                    return new BadRequestResult();
                }
                return new OkResult();
            }
        }
        [FunctionName("UpdatePatientDetails")]
        public async Task<IActionResult> UpdateDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Put), Route = null)] HttpRequest req, ILogger log)
        {
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<PersonalInformation>(requestBody);
                try
                {

                    if (input != null)
                    {


                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            string msg = "";
                            SqlCommand cmd = new SqlCommand("PersonalInfoUpdate", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Id", input.Id));
                            cmd.Parameters.Add(new SqlParameter("@FirstName", input.FirstName));
                            cmd.Parameters.Add(new SqlParameter("@PreferredName", input.PreferredName));
                            cmd.Parameters.Add(new SqlParameter("@MiddleName", input.MiddleName));
                            cmd.Parameters.Add(new SqlParameter("@LastName", input.LastName));
                            cmd.Parameters.Add(new SqlParameter("@Suffiix", input.Suffiix));
                            cmd.Parameters.Add(new SqlParameter("@SSN", input.SSN));
                            cmd.Parameters.Add(new SqlParameter("@DateofBirth", input.DateofBirth));
                            cmd.Parameters.Add(new SqlParameter("@BirthSex", input.BirthSex));
                            cmd.Parameters.Add(new SqlParameter("@UpdatedBy", input.UpdatedBy));
                            cmd.Parameters.Add(new SqlParameter("@CreatedDate", input.CreatedDate));
                            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", input.UpdatedDate));
                            int i = cmd.ExecuteNonQuery();
                            connection.Close();
                            if (i > 0)
                            {
                                msg = "Data has been inserted";
                            }
                            else
                            {
                                msg = "Error";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                    return new BadRequestResult();
                }
                return new OkResult();
            }
        }
        [FunctionName("DeletePatientDetails")]
        public async Task<IActionResult> DeleteDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, nameof(HttpMethods.Delete), Route = null)] HttpRequest req, ILogger log)
        {
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var input = JsonConvert.DeserializeObject<PersonalInformation>(requestBody);
                try
                {

                    if (input != null)
                    {
                        string msg = "";

                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            SqlCommand cmd = new SqlCommand("PersonalInfoDelete", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@Id", input.Id));
                            int i = cmd.ExecuteNonQuery();
                            connection.Close();
                            if (i > 0)
                            {
                                msg = "Data has been Deleted";
                            }
                            else
                            {
                                msg = "Error";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError(e.ToString());
                    return new BadRequestResult();
                }
                return new OkResult();
            }
        }
    }
}

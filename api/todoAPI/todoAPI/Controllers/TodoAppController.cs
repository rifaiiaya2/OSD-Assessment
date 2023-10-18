using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using todoAPI.Models;
namespace todoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        private IConfiguration _configuration;
        public TodoAppController( IConfiguration configuration) { 
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetUser")]
        public JsonResult GetUser()
        {
            string query = "SELECT * FROM dbo.Login";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand (query, myCon))
                {
                    myReader=myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        [Route("AddUser")]
        public JsonResult AddUser([FromBody] Login newUser)
        {
            string query = "INSERT INTO dbo.Login (Email, Password) VALUES (@Email, @Password)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Email", newUser.Email);
                    myCommand.Parameters.AddWithValue("@Password", newUser.Password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();

                }
            }
            return new JsonResult ("Added Successfully");
        }
    }
}

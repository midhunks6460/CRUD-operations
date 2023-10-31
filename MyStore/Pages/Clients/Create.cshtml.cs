using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo=new ClientInfo();
        public String errormessage = "";
        public String successmessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email= Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errormessage = "All the fields are required";
                //and return to the beggining
                return;
            }
            //or else we have to save the data to database and clear all fields

            //add to database
            try 
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                        "(name,email,phone,address)VALUES" +
                        "(@name,@email,@phone,@address);";
                    //now replace the vlues in @ position with the values we got from the form
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        //execute the query after replacing the values
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) 
            {
                errormessage=ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successmessage = "New client added correctly";

            Response.Redirect("/Clients/Index");
        }
    }
}

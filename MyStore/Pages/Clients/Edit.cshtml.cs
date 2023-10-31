using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errormessage = "";
        public String successmessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //New connection starts from here
                    connection.Open();
                    //read all data from the database
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    //this command allow us to execute the sql query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    //lets execute the command and obtain sql data reader
                    {
                        command.Parameters.AddWithValue("@id", id);
                        //obtain sql data reader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //loop to read all the data
                            if (reader.Read())
                            {
       
                                //id is of type string but in database its of type integer
                                //so empty string is created
                                clientInfo.id = " " + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
            }
        }

        public void OnPost() 
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.id.Length==0 || clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errormessage = "All the fields are required";
                //and return to the beggining
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " +
                                 "SET name=@name, email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";
                    //now replace the vlues in @ position with the values we got from the form
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);
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
            successmessage = "Client details edited";

            Response.Redirect("/Clients/Index");
        }    
    }
}

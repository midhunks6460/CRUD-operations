using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients=new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //New connection starts from here
                    connection.Open();
                    //read all data from the database
                    String sql = "SELECT * FROM clients";
                    //this command allow us to execute the sql query
                    using (SqlCommand command = new SqlCommand(sql,connection))
                    //lets execute the command and obtain sql data reader
                    {
                        //obtain sql data reader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //loop to read all the data
                            while (reader.Read())
                            { 
                                //save data into clientinfo objects
                                ClientInfo clientInfo = new ClientInfo();
                                //id is of type string but in database its of type integer
                                //so empty string is created
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at=reader.GetDateTime(5).ToString();


                                listClients.Add(clientInfo);


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ClientInfo
    { 
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }
}

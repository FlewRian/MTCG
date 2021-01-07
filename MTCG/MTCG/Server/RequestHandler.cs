using Newtonsoft.Json;
using Npgsql;

namespace MTCG
{
    internal class RequestHandler
    {
        private RequestContext requestContext;
        static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";

        public RequestHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
        }

        public string ExecuteRequest()
        {
            switch (requestContext.HttpVerb.ToUpper())
            {
                case "GET":
                    {

                        return CreateResponse("400 Bad Request", "text/plain", "1 unkown HTTTP-Verb");
                    }
                case "POST":
                    {
                        switch (requestContext.MessagePath)
                        {
                            case "/users":
                                {
                                    return RegisterUser();
                                }
                        }
                        return CreateResponse("400 Bad Request", "text/plain", "2 unkown HTTTP-Verb");
                    }
                case "PUT":
                    {
                        return CreateResponse("400 Bad Request", "text/plain", "3 unkown HTTTP-Verb");
                    }
                case "DELETE":
                    {
                        return CreateResponse("400 Bad Request", "text/plain", "4 unkown HTTTP-Verb");
                    }
                default:
                    {
                        return CreateResponse("400 Bad Request", "text/plain", "5 unkown HTTTP-Verb");
                    }

            }
        }

        private string RegisterUser()
        {
            Player player = new Player();
            try
            {
                player = JsonConvert.DeserializeObject<Player>(requestContext.Content);
            }
            catch (JsonSerializationException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Could not deserialize Json Object");
            }
            string token = "Basic " + player.Username + "-mtcgToken";

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            
            var cmd = new NpgsqlCommand("INSERT INTO credentials (username, password, token) VALUES (@username, @password, @token)", conn);
            cmd.Parameters.AddWithValue("username", player.Username);
            cmd.Parameters.AddWithValue("password", player.Password);
            cmd.Parameters.AddWithValue("token", token);
            cmd.Prepare();

            var cmd2 = new NpgsqlCommand("INSERT INTO player (username, coins, elo) VALUES (@username, @coins, @elo)", conn);
            cmd2.Parameters.AddWithValue("username", player.Username);
            cmd2.Parameters.AddWithValue("coins", 20);
            cmd2.Parameters.AddWithValue("elo", 100);
            cmd2.Prepare();

            try
            {
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                return CreateResponse("200 OK", "text/plain", "User created");
            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "User exists already");
            }
        }

        private string CreateResponse(string status, string contentType, string content)
        {
            var erg = "";
            erg += "HTTP/1.1 " + status + "\r\n" +
                   "Server: MTCG-Server\r\n" +
                   "Content-Type: " + contentType + "\r\n" +
                   "Content-Length: " + content.Length + "\r\n" +
                   "\r\n" +
                   content;
            return erg;
        }
    }
}


//"HTTP/1.1 200 OK\r\nServer: MTCG-Server\r\n";
//"HTTP/1.1 400 Bad Request\r\nServer: MTCG-Server\r\n";
//"HTTP/1.1 404 Not Found\r\nServer: MTCG-Server\r\n";
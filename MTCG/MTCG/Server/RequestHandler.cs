using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;

namespace MTCG
{
    internal class RequestHandler
    {
        private RequestContext requestContext;
        static readonly string ConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=mtcg";
        private string responsType;

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
                        switch (requestContext.MessagePath)
                        {
                            case "/cards":
                                {
                                    return ShowOwnedCards();
                                }
                            case "/deck":
                                {
                                    responsType = "json";
                                    return ShowDeck();
                                }
                            case "/deck?format=plain":
                                {
                                    responsType = "text";
                                    return ShowDeck();
                                }
                        }
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
                            case "/sessions":
                                {
                                    string token = LoginUser();
                                    if (token is null)
                                    {
                                        return CreateResponse("400 Bad Request", "text/plain", "User or password is incorrect!");
                                    }
                                    return CreateResponse("200 OK", "text/plain", "Login was successfuls");
                                }
                            case "/packages":
                                {
                                    return CreatePackage();
                                }
                            case "/transactions/packages":
                                {
                                    return BuyPackage();
                                }
                        }
                        return CreateResponse("400 Bad Request", "text/plain", "2 unkown HTTTP-Verb");
                    }
                case "PUT":
                    {
                        switch (requestContext.MessagePath)
                        {

                        }
                        return CreateResponse("400 Bad Request", "text/plain", "3 unkown HTTTP-Verb");
                    }
                case "DELETE":
                    {
                        switch (requestContext.MessagePath)
                        {

                        }
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
            string token = " Basic " + player.Username + "-mtcgToken";

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

        public string LoginUser()
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

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT token FROM credentials where username = @username and password = @password", conn);
            cmd.Parameters.AddWithValue("username", player.Username);
            cmd.Parameters.AddWithValue("password", player.Password);
            cmd.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return (string)reader[0];
                }

                return null;
            }
            catch (PostgresException)
            {
                return null;
            }
        }

        public string CreatePackage()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Unauthorized command!");

            if (!token.Equals(" Basic admin-mtcgToken"))
                return CreateResponse("400 Bad Request", "text/plain", "Unauthorized command!");

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO packages (cards_json) VALUES (@json)", conn);
            cmd.Parameters.AddWithValue("json", requestContext.Content);
            cmd.Prepare();
            try
            {
                cmd.ExecuteNonQuery();
                return CreateResponse("200 OK", "text/plain", "Package created");
            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Unauthorized command!");
            }
        }

        public string BuyPackage()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Token Null Unauthorized command!");

            if (GetUsernameFromToken(token) == null)
                return CreateResponse("400 Bad Request", "text/plain", "UserToken null Unauthorized command!");

            string erg;
            int coins;
            string username = GetUsernameFromToken(token);
            List<BaseCard> realCards = new List<BaseCard>();

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT coins FROM player where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            var cmd0 = new NpgsqlCommand("SELECT cards_json FROM packages where id IN (SELECT id from packages ORDER BY id asc LIMIT 1)", conn);
            cmd0.Prepare();

            try
            {
                coins = (int)cmd.ExecuteScalar();

                if (coins < 5)
                    return CreateResponse("400 Bad Request", "text/plain", "not enought Coins");

                coins -= 5;

                erg = (string)cmd0.ExecuteScalar();

                if (erg == null)
                    return CreateResponse("400 Bad Request", "text/plain", "erg null Unauthorized command!");

                List<JsonCard> cards = JsonConvert.DeserializeObject<List<JsonCard>>(erg);

                foreach (var card in cards)
                {
                    realCards.Add(card.ConvertToCard());
                }

            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Postgres Error Unauthorized command!");
            }

            cmd0 = new NpgsqlCommand("UPDATE player SET coins = @coins where username = @username", conn);
            cmd0.Parameters.AddWithValue("username", username);
            cmd0.Parameters.AddWithValue("coins", coins);
            cmd0.Prepare();

            var cmd1 = new NpgsqlCommand("DELETE FROM packages where id IN (SELECT id from packages ORDER BY id asc LIMIT 1)", conn);
            cmd1.Prepare();

            try
            {
                int check = cmd0.ExecuteNonQuery();
                if (check == -1)
                    return CreateResponse("400 Bad Request", "text/plain", "??? Unauthorized command!");

                cmd1.ExecuteNonQuery();
            }

            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Postgres Error2 Unauthorized command!");
            }

            foreach (var card in realCards)
            {
                var cmd2 = new NpgsqlCommand("INSERT INTO cards (id, name, typ, element, damage) VALUES (@id, @name, @typ, @element, @damage)", conn);
                cmd2.Parameters.AddWithValue("id", card.cardId);
                cmd2.Parameters.AddWithValue("name", card.name);
                cmd2.Parameters.AddWithValue("typ", card.GetType().FullName);
                cmd2.Parameters.AddWithValue("element", card.element.ToString());
                cmd2.Parameters.AddWithValue("damage", card.damage);
                cmd2.Prepare();

                var cmd3 = new NpgsqlCommand("INSERT INTO player_cards (username, card_id) VALUES (@username, @id)", conn);
                cmd3.Parameters.AddWithValue("id", card.cardId);
                cmd3.Parameters.AddWithValue("username", username);
                cmd3.Prepare();

                try
                {
                    cmd2.ExecuteNonQuery();
                    cmd3.ExecuteNonQuery();
                }
                catch (PostgresException)
                {
                    return CreateResponse("400 Bad Request", "text/plain", "Postgres Error3 Unauthorized command!");
                }
            }
            if (erg != null)
                return CreateResponse("200 OK", "application/json", JsonConvert.SerializeObject(erg));
            return CreateResponse("400 Bad Request", "text/plain", "Unauthorized command!");
        }

        public string ShowOwnedCards()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Token Null Unauthorized command!");

            if (GetUsernameFromToken(token) == null)
                return CreateResponse("400 Bad Request", "text/plain", "UserToken null Unauthorized command!");

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            string username = GetUsernameFromToken(token);
            List<JsonCard> cards = new List<JsonCard>();

            var cmd = new NpgsqlCommand("Select cards.id, name, damage from cards inner join player_cards on cards.id = player_cards.card_id where player_cards.username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var temp = new JsonCard();
                        temp.Id = (string)reader[0];
                        temp.Name = (string)reader[1];
                        temp.Damage = (double)((decimal)reader[2]);
                        cards.Add(temp);
                    }
                    if (cards != null)
                        return CreateResponse("200 OK", "application/json", JsonConvert.SerializeObject(cards));
                }
                return CreateResponse("400 Bad Request", "text/plain", "No cards found!");
            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "No cards found!");
            }
        }


        public string ShowDeck()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Token Null Unauthorized command!");

            string username = GetUsernameFromToken(token);

            if (username == null)
                return CreateResponse("400 Bad Request", "text/plain", "UserToken null Unauthorized command!");

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("Select card_id from player_deck where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            List<string> deck = new List<string>();

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    List<string> cards = new List<string>();
                    while (reader.Read())
                    {
                        var temp = (string)reader[0];
                        cards.Add(temp);
                    }
                    deck = cards;
                    if (deck != null)
                    {
                        if (this.responsType == "json")
                            return CreateResponse("200 OK", "application/json", JsonConvert.SerializeObject(deck));
                        return CreateResponse("200 OK", "text/plain", string.Join("\n", deck));
                    }
                }
                return CreateResponse("400 Bad Request", "text/plain", "No Cards in Deck");
            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Postgres Error");
            }
        }
        public string GetUsernameFromToken(string token)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT username FROM credentials where token = @token", conn);
            cmd.Parameters.AddWithValue("token", token);
            cmd.Prepare();

            try
            {
                return (string)cmd.ExecuteScalar();
            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Postgres Error4 Unauthorized command!");
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
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        switch (requestContext.MessagePath.ToLower())
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
                        switch (requestContext.MessagePath.ToLower())
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
                            case "/battles":
                                {
                                    return Battle();
                                }
                        }
                        return CreateResponse("400 Bad Request", "text/plain", "2 unkown HTTTP-Verb");
                    }
                case "PUT":
                    {
                        switch (requestContext.MessagePath.ToLower())
                        {
                            case "/deck":
                                {
                                    return ConfigureDeck();
                                }
                        }
                        return CreateResponse("400 Bad Request", "text/plain", "3 unkown HTTTP-Verb");
                    }
                case "DELETE":
                    {
                        switch (requestContext.MessagePath.ToLower())
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

        public string ConfigureDeck()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Token Null Unauthorized command!");

            string username = GetUsernameFromToken(token);

            if (username == null)
                return CreateResponse("400 Bad Request", "text/plain", "UserToken null Unauthorized command!");

            try
            {
                List<string> cards = JsonConvert.DeserializeObject<List<string>>(requestContext.Content);
                if (cards.Count != 4)
                    return CreateResponse("400 Bad Request", "text/plain", "A deck is only allowed to contain exactly 4 cards!");

                var check = cards.Distinct().ToList();

                if (check.Count != 4)
                    return CreateResponse("400 Bad Request", "text/plain", "You cannot add multiple copies of the exact same card into your deck!");

                using var conn = new NpgsqlConnection(ConnectionString);
                conn.Open();

                try
                {
                    foreach (var card in cards)
                    {
                        //Check if cards are owned
                        var cmd = new NpgsqlCommand("Select * from player_cards where username = @username and card_id = @card_id", conn);
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("card_id", card);
                        cmd.Prepare();

                        var reader = cmd.ExecuteReader();
                        if (!reader.HasRows)
                            return CreateResponse("400 Bad Request", "text/plain", "Cards are not found in the users stack!");

                        reader.Close();
                    }

                    //Delete current deck
                    var cmd1 = new NpgsqlCommand("DELETE FROM player_deck where username = @username", conn);
                    cmd1.Parameters.AddWithValue("username", username);
                    cmd1.Prepare();


                    cmd1.ExecuteNonQuery();


                    foreach (var card in cards)
                    {
                        //Add cards to player_deck
                        var cmd3 = new NpgsqlCommand("INSERT INTO player_deck (username, card_id) VALUES (@username, @card_id)", conn);
                        cmd3.Parameters.AddWithValue("username", username);
                        cmd3.Parameters.AddWithValue("card_id", card);
                        cmd3.Prepare();

                        cmd3.ExecuteNonQuery();
                    }
                }
                catch (PostgresException)
                {
                    return CreateResponse("400 Bad Request", "text/plain", "Postgres Error");
                }

                return CreateResponse("200 OK", "text/plain", "Deck successfully configured!");

            }
            catch (JsonSerializationException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "Could not deserialize json data!");
            }
        }

        public string Battle()
        {
            string token = requestContext.AuthorizationToken;
            if (token == null)
                return CreateResponse("400 Bad Request", "text/plain", "Token Null Unauthorized command!");

            string username = GetUsernameFromToken(token);

            if (username == null)
                return CreateResponse("400 Bad Request", "text/plain", "UserToken null Unauthorized command!");

            /////////////////////////////////////////////////////////////////////////////////////////////////////

            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            //Check if user has deck
            var cmd = new NpgsqlCommand("Select * from player_deck where username = @username", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();

            //Check if Battle entry without user exists
            var cmd2 = new NpgsqlCommand("Select id from battle where player1 != @username and player2 is null", conn);
            cmd2.Parameters.AddWithValue("username", username);
            cmd2.Prepare();

            //Create Battle entry if necessary
            var cmd3 = new NpgsqlCommand("INSERT INTO battle (player1) VALUES (@username)", conn);
            cmd3.Parameters.AddWithValue("username", username);
            cmd3.Prepare();

            var cmdUpdate = new NpgsqlCommand("UPDATE battle SET player2 = @username where id = @id", conn);

            //Get created BattleID
            var cmdBattle = new NpgsqlCommand("Select id from battle where player1 = @username", conn);
            cmdBattle.Parameters.AddWithValue("username", username);
            cmdBattle.Prepare();

            //If Battle entry exists already execute battle
            var cmd4 = new NpgsqlCommand("Select player1 from battle where id = @id", conn);
            var cmd5 = new NpgsqlCommand("Select id, name, damage from cards inner join player_deck on cards.id = player_deck.card_id where username = @username", conn);
            var cmd6 = new NpgsqlCommand("Select id, name, damage from cards inner join player_deck on cards.id = player_deck.card_id where username = @username", conn);

            //reduce Elo
            var cmdLoser = new NpgsqlCommand("UPDATE player SET elo = (elo-5) where username = @username", conn);
            var cmdWinner = new NpgsqlCommand("UPDATE player SET elo = (elo+3) where username = @username", conn);

            //Save Battle in BattleLog
            var cmd9 = new NpgsqlCommand("INSERT INTO battle_log (id, battle_text, winner) VALUES (@id, @battle_text, @winner)", conn);

            //Loop until BattleLog exists
            var cmd10 = new NpgsqlCommand("Select battle_text from battle_log where id = @id", conn);

            try
            {
                //Check if user has deck
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return CreateResponse("400 Bad Request", "text/plain", "No deck found!");

                reader.Close();

                //Check if Battle entry without user exists
                var reader2 = cmd2.ExecuteReader();
                //If Battle entry exists already -> execute battle
                int battleId;
                if (reader2.HasRows)
                {
                    reader2.Read();
                    battleId = (int)reader2[0];
                    reader2.Close();

                    cmd4.Parameters.AddWithValue("id", battleId);
                    cmd4.Prepare();

                    var reader4 = cmd4.ExecuteReader();
                    if (!reader4.HasRows)
                        return CreateResponse("400 Bad Request", "text/plain", "Unknown Error");
                    reader4.Read();
                    var player1 = (string)reader4[0];
                    var player2 = username;

                    reader4.Close();

                    cmdUpdate.Parameters.AddWithValue("username", username);
                    cmdUpdate.Parameters.AddWithValue("id", battleId);
                    cmdUpdate.Prepare();

                    cmdUpdate.ExecuteNonQuery();

                    cmd5.Parameters.AddWithValue("username", player1);
                    cmd5.Prepare();

                    cmd6.Parameters.AddWithValue("username", player2);
                    cmd6.Prepare();

                    var cardList1 = new List<BaseCard>();
                    var cardList2 = new List<BaseCard>();

                    var reader5 = cmd5.ExecuteReader();
                    if (!reader5.HasRows)
                        return CreateResponse("400 Bad Request", "text/plain", "Unknown Error");
                    while (reader5.Read())
                    {
                        var card = new JsonCard();
                        card.Id = (string)reader5[0];
                        card.Name = (string)reader5[1];
                        card.Damage = (double)((decimal)reader5[2]);
                        cardList1.Add(card.ConvertToCard());
                    }

                    reader5.Close();

                    var reader6 = cmd6.ExecuteReader();
                    if (!reader6.HasRows)
                        return CreateResponse("400 Bad Request", "text/plain", "Unknown Error");
                    while (reader6.Read())
                    {
                        var card = new JsonCard();
                        card.Id = (string)reader6[0];
                        card.Name = (string)reader6[1];
                        card.Damage = (double)((decimal)reader6[2]);
                        cardList2.Add(card.ConvertToCard());
                    }

                    reader6.Close();


                    //DB inputs
                    var deck1 = new List<BaseCard>();
                    var deck2 = new List<BaseCard>();
                    var user1 = new Player();
                    var user2 = new Player();

                    deck1 = cardList1;
                    deck2 = cardList2;
                    user1.CurrentDeck = deck1;
                    user2.CurrentDeck = deck2;
                    user1.Username = player1;
                    user2.Username = player2;

                    Arena arena = new Arena();
                    var log = arena.Fight(user1, user2, arena);

                    //reduce elo
                    if (log.Winner != null)
                    {
                        cmdWinner.Parameters.AddWithValue("username", log.Winner.Username);
                        cmdWinner.Prepare();

                        cmdLoser.Parameters.AddWithValue("username",
                            player1.Equals(log.Winner.Username) ? player2 : player1);
                        cmdLoser.Prepare();

                        cmdLoser.ExecuteNonQuery();
                        cmdWinner.ExecuteNonQuery();
                    }

                    //Save Battle in BattleLog
                    cmd9.Parameters.AddWithValue("id", battleId);
                    cmd9.Parameters.AddWithValue("battle_text", log.ArenaText);
                    if (log.Winner != null)
                        cmd9.Parameters.AddWithValue("winner", log.Winner.Username);
                    else
                        cmd9.Parameters.AddWithValue("winner", "");
                    cmd9.Prepare();
                    cmd9.ExecuteNonQuery();

                }

                //Create Battle entry if necessary
                else
                {
                    reader2.Close();

                    var reader3 = cmd3.ExecuteNonQuery();
                    if (reader3 == -1)
                        return CreateResponse("400 Bad Request", "text/plain", "Unknown Error");

                    //Get created battleID
                    battleId = (int)cmdBattle.ExecuteScalar();
                }


                //Loop until ArenaLog exists

                cmd10.Parameters.AddWithValue("id", battleId);
                cmd10.Prepare();

                while (true)
                {
                    var reader10 = cmd10.ExecuteReader();
                    if (reader10.HasRows)
                    {
                        reader10.Read();
                        return CreateResponse("200 OK", "text/plain", (string)reader10[0]);
                    }
                    reader10.Close();

                    System.Threading.Thread.Sleep(1000);
                }

            }
            catch (PostgresException)
            {
                return CreateResponse("400 Bad Request", "text/plain", "PostgresError");
            }


            /////////////////////////////////////////////////////////////////////////////////////////////////////

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
CREATE DATABASE mtcg;

\c mtcg

CREATE TABLE IF NOT EXISTS credentials (
username VARCHAR(255) PRIMARY KEY, 
password VARCHAR(255) NOT NULL, 
token VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS cards (
id VARCHAR(255) PRIMARY KEY, 
name VARCHAR(255) NOT NULL,
typ VARCHAR(255) NOT NULL, 
element VARCHAR(255) NOT NULL,
damage DECIMAL NOT NULL
);

CREATE TABLE IF NOT EXISTS player_cards (
card_id VARCHAR(255) PRIMARY KEY,
username VARCHAR(255) NOT NULL,
CONSTRAINT fk_user_cards
    FOREIGN KEY(username) REFERENCES credentials(username),
	FOREIGN KEY(card_id) REFERENCES cards(id)
);

CREATE TABLE IF NOT EXISTS packages (
id serial PRIMARY KEY,
cards_json VARCHAR(2048) NOT NULL
);

CREATE TABLE IF NOT EXISTS player(
username VARCHAR(255) PRIMARY KEY,
name VARCHAR(255),
coins INT NOT NULL,
image VARCHAR(255),
bio VARCHAR(255),
elo INT NOT NULL,
CONSTRAINT fk_player
	FOREIGN KEY(username) REFERENCES credentials(username)
);

CREATE TABLE IF NOT EXISTS trade(
id VARCHAR(255) PRIMARY KEY,
username VARCHAR(255) NOT NULL,
card_id VARCHAR(255) NOT NULL,
typ VARCHAR(255),
element VARCHAR(255),
damage DECIMAL NOT NULL,
CONSTRAINT fk_trade
	FOREIGN KEY(username) REFERENCES credentials(username),
	FOREIGN KEY(card_id) REFERENCES player_cards(card_id)
);

CREATE TABLE IF NOT EXISTS player_deck(
card_id VARCHAR(255) PRIMARY KEY,
username VARCHAR(255) NOT NULL,
CONSTRAINT fk_player_deck
	FOREIGN KEY(username) REFERENCES player(username),
	FOREIGN KEY(card_id) REFERENCES player_cards(card_id)
);

CREATE TABLE IF NOT EXISTS battle(
id serial PRIMARY KEY,
player1 VARCHAR(255) NOT NULL,
player2 VARCHAR(255),
CONSTRAINT fk_battle
	FOREIGN KEY(player1) REFERENCES player(username),
	FOREIGN KEY(player2) REFERENCES player(username)
);

CREATE TABLE IF NOT EXISTS battle_log(
id INT PRIMARY KEY,
battle_text VARCHAR(102400) NOT NULL,
winner VARCHAR(255),
CONSTRAINT fk_battle
	FOREIGN KEY(id) REFERENCES battle(id)
);
--
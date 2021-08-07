using HoanGames.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoanGames
{
    public class PlayerRepository
    {
        SQLiteAsyncConnection conn;
        public string StatusMessage { get; set; }

        public PlayerRepository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Player>().Wait();
        }

        public async Task AddPlayer(string name)
        {
            int result = 0;

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Valid name, please.");
                }
                result = await conn.InsertAsync(new Player() { Name = name });

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        public async Task RemovePlayer(string name)
        {
            int result = 0;

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Valid name, please.");
                }
                List<Player> playerList = await conn.Table<Player>().ToListAsync();

                result = await conn.DeleteAsync(playerList.Find(x => x.Name == name));

                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
            }
        }

        public async Task<List<Player>> GetAllPlayers()
        {
            try
            {
                return await conn.Table<Player>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
            }

            return new List<Player>();
        }
    }
}

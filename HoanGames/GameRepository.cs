using HoanGames.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoanGames
{
    public class GameRepository
    {
        private readonly SQLiteAsyncConnection conn;
        public GameRepository(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Game>().Wait();
        }
        public async Task AddGame(int width = 6, int height = 6, int mines = 6)
        {
            await conn.InsertAsync(new Game()
            {
                BoardWidth = width,
                BoardHeight = height,
                BoardMines = mines
            }).ConfigureAwait(false);
        }
        public async Task UpdateGame(int width, int height, int mines)
        {
            Game game = await conn.Table<Game>().FirstOrDefaultAsync().ConfigureAwait(false);

            if (game != null)
            {
                game.BoardWidth = width;
                game.BoardHeight = height;
                game.BoardMines = mines;
                await conn.UpdateAsync(game).ConfigureAwait(false);
            }
        }
        public async Task<Game> GetGame()
        {
            return await conn.Table<Game>().FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }
}

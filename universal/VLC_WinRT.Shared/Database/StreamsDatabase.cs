﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using VLC_WinRT.Model.Stream;
using VLC_WinRT.Utils;

namespace VLC_WinRT.Database
{
    public class StreamsDatabase : IDatabase
    {
        private static readonly string DbPath = Strings.VideoDatabase;

        public StreamsDatabase()
        {
            Task.Run(() => Initialize());
        }

        public void Initialize()
        {
            var db = new SQLiteConnection(DbPath);
            db.CreateTable<StreamMedia>();
        }

        public void Drop()
        {
            using (var db = new SQLiteConnection(DbPath))
            {
                db.DropTable<StreamMedia>();
            }
        }
        
        public async Task<List<StreamMedia>> Load()
        {
            var connection = new SQLiteAsyncConnection(DbPath);
            return await connection.Table<StreamMedia>().ToListAsync();
        }

        public async Task Insert(StreamMedia stream)
        {
            var connection = new SQLiteAsyncConnection(DbPath);
            await connection.InsertAsync(stream);
        }

        public Task Update(StreamMedia stream)
        {
            var connection = new SQLiteAsyncConnection(DbPath);
            return connection.UpdateAsync(stream);
        }
    }
}

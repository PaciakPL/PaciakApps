﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Paciak
{
    public class SongRepository : ISongRepository
    {
        private const string DbName = "forume_apps";
        private const string CollectionName = "songs";
        private readonly IMongoDatabase db;

        public SongRepository(IDbProvider dbProvider)
        {
            db = dbProvider.GetDatabase(DbName);
        }

        public async Task<bool> Exists(Song song)
        {
            var songsCollection = GetSongsCollection();
            var existsFilter = Builders<BsonDocument>.Filter.Eq("_id", song.VideoId);
            using var songs = await songsCollection.FindAsync<Song>(existsFilter);
            var result = await songs.AnyAsync();

            return result;
        }

        public async Task AddSong(Song song)
        {
            var songsCollection = GetSongsCollection();

            await songsCollection.InsertOneAsync(song.ToBsonDocument());
        }

        public async Task<bool> UpdateSong(Song song)
        {
            var songsCollection = GetSongsCollection();
            var result = await songsCollection.UpdateOneAsync(
                Builders<BsonDocument>.Filter.Eq("_id", song.VideoId),
                Builders<BsonDocument>.Update.Set(nameof(Song.PlaylistId), song.PlaylistId));

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<IEnumerable<Song>> GetOrphanedSongs()
        {
            var songsCollection = GetSongsCollection();
            var result =
                await songsCollection.FindAsync<Song>(Builders<BsonDocument>.Filter.Eq(nameof(Song.PlaylistId), BsonNull.Value));

            return result.ToList();
        }

        private IMongoCollection<BsonDocument> GetSongsCollection()
        {
            return db.GetCollection<BsonDocument>(CollectionName);
        }
    }
}
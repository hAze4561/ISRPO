using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MusicPlayer.Models;

namespace MusicPlayer
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string serverName = "HAZE\\SQLEXPRESS", string databaseName = "MusicPlayerDB")
        {
            connectionString = $@"Server={serverName};Database={databaseName};Integrated Security=True;";
        }

        // Проверка подключения
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // Получить все треки
        public List<MusicPlayer.Models.MusicTrack> GetAllTracks()
        {
            List<MusicPlayer.Models.MusicTrack> tracks = new List<MusicPlayer.Models.MusicTrack>();
            string query = @"SELECT Id, Title, Artist, Album, Duration, FilePath, PlayCount, DateAdded 
                            FROM MusicTracks ORDER BY Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MusicPlayer.Models.MusicTrack track = new MusicPlayer.Models.MusicTrack();
                    track.Id = reader.GetInt32(0);
                    track.Title = reader.GetString(1);
                    track.Artist = reader.GetString(2);

                    if (reader.IsDBNull(3))
                        track.Album = null;
                    else
                        track.Album = reader.GetString(3);

                    // Исправление: Duration теперь хранится как строка
                    if (reader.IsDBNull(4))
                    {
                        track.Duration = null;
                    }
                    else
                    {
                        string durationStr = reader.GetString(4);
                        if (TimeSpan.TryParse(durationStr, out TimeSpan duration))
                        {
                            track.Duration = duration;
                        }
                        else
                        {
                            track.Duration = null;
                        }
                    }

                    track.FilePath = reader.GetString(5);
                    track.PlayCount = reader.GetInt32(6);
                    track.DateAdded = reader.GetDateTime(7);

                    tracks.Add(track);
                }
            }
            return tracks;
        }

        // Добавить трек
        public int AddTrack(MusicPlayer.Models.MusicTrack track)
        {
            string query = @"INSERT INTO MusicTracks (Title, Artist, Album, Duration, FilePath, PlayCount, DateAdded, AudioData)
                             VALUES (@Title, @Artist, @Album, @Duration, @FilePath, @PlayCount, @DateAdded, @AudioData);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", track.Title);
                cmd.Parameters.AddWithValue("@Artist", track.Artist);

                if (track.Album == null)
                    cmd.Parameters.AddWithValue("@Album", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Album", track.Album);

                // Сохраняем Duration как строку в формате "mm:ss"
                if (track.Duration.HasValue)
                {
                    string durationString = track.Duration.Value.ToString(@"mm\:ss");
                    cmd.Parameters.AddWithValue("@Duration", durationString);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Duration", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@FilePath", track.FilePath);
                cmd.Parameters.AddWithValue("@PlayCount", track.PlayCount);
                cmd.Parameters.AddWithValue("@DateAdded", track.DateAdded);
                cmd.Parameters.AddWithValue("@AudioData", track.AudioData);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Удалить трек
        public bool DeleteTrack(int id)
        {
            string query = "DELETE FROM MusicTracks WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Поиск треков
        public List<MusicPlayer.Models.MusicTrack> SearchTracks(string keyword)
        {
            List<MusicPlayer.Models.MusicTrack> tracks = new List<MusicPlayer.Models.MusicTrack>();
            string query = @"SELECT Id, Title, Artist, Album, Duration, FilePath, PlayCount, DateAdded 
                            FROM MusicTracks 
                            WHERE Title LIKE @Keyword OR Artist LIKE @Keyword 
                            ORDER BY Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MusicPlayer.Models.MusicTrack track = new MusicPlayer.Models.MusicTrack();
                    track.Id = reader.GetInt32(0);
                    track.Title = reader.GetString(1);
                    track.Artist = reader.GetString(2);

                    if (reader.IsDBNull(3))
                        track.Album = null;
                    else
                        track.Album = reader.GetString(3);

                    // Исправление: Duration теперь хранится как строка
                    if (reader.IsDBNull(4))
                    {
                        track.Duration = null;
                    }
                    else
                    {
                        string durationStr = reader.GetString(4);
                        if (TimeSpan.TryParse(durationStr, out TimeSpan duration))
                        {
                            track.Duration = duration;
                        }
                        else
                        {
                            track.Duration = null;
                        }
                    }

                    track.FilePath = reader.GetString(5);
                    track.PlayCount = reader.GetInt32(6);
                    track.DateAdded = reader.GetDateTime(7);

                    tracks.Add(track);
                }
            }
            return tracks;
        }

        // Увеличить счетчик прослушиваний
        public void IncrementPlayCount(int trackId)
        {
            string query = "UPDATE MusicTracks SET PlayCount = PlayCount + 1 WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", trackId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Получить аудио данные трека
        public byte[] GetAudioData(int trackId)
        {
            string query = "SELECT AudioData FROM MusicTracks WHERE Id = @Id";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", trackId);
                conn.Open();
                return (byte[])cmd.ExecuteScalar();
            }
        }

        // Получить один трек по ID
        public MusicPlayer.Models.MusicTrack GetTrackById(int id)
        {
            string query = @"SELECT Id, Title, Artist, Album, Duration, FilePath, PlayCount, DateAdded 
                            FROM MusicTracks WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    MusicPlayer.Models.MusicTrack track = new MusicPlayer.Models.MusicTrack();
                    track.Id = reader.GetInt32(0);
                    track.Title = reader.GetString(1);
                    track.Artist = reader.GetString(2);

                    if (reader.IsDBNull(3))
                        track.Album = null;
                    else
                        track.Album = reader.GetString(3);

                    // Исправление: Duration теперь хранится как строка
                    if (reader.IsDBNull(4))
                    {
                        track.Duration = null;
                    }
                    else
                    {
                        string durationStr = reader.GetString(4);
                        if (TimeSpan.TryParse(durationStr, out TimeSpan duration))
                        {
                            track.Duration = duration;
                        }
                        else
                        {
                            track.Duration = null;
                        }
                    }

                    track.FilePath = reader.GetString(5);
                    track.PlayCount = reader.GetInt32(6);
                    track.DateAdded = reader.GetDateTime(7);

                    return track;
                }
                return null;
            }
        }
    }
}
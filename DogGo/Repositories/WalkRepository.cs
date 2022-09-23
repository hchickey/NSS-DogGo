﻿using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.Date, w.Duration, w.WalkerId, w.DogId, o.Name
                        FROM Walks w
                        JOIN Dog d ON d.Id = w.DogId
                        JOIN Owner o ON o.Id = d.OwnerId
                        WHERE w.WalkerId = @walkerId
                        ORDER BY o.Name DESC";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walks> walks = new List<Walks>();

                        while (reader.Read())
                        {
                            Owner owner = new Owner()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            Walks walk = new Walks()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                Owner = owner
                            };

                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }
        }
    }
}

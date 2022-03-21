﻿namespace OneGate.Backend.Core.Shared.Database
{
    public class DatabaseConnectionOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
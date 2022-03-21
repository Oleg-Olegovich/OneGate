﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OneGate.Backend.Gateway.User.Api.Contracts.Portfolio
{
    public class PortfolioModel
    {
        [Required] 
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("owner_id")]
        [Required]
        public int OwnerId { get; set; }
    }
}
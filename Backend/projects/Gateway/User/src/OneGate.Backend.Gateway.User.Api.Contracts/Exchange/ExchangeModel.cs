﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OneGate.Backend.Gateway.User.Api.Contracts.Exchange
{
    public class ExchangeModel
    {
        [Required]
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(500)]
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(150)]
        [JsonProperty("website")]
        public string Website { get; set; }
        
        [Required]
        [JsonProperty("engine_type")]
        public EngineType EngineType { get; set; }
    }
}
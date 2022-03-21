﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OneGate.Backend.Gateway.User.Api.Contracts.Order.Limit
{
    public class CreateLimitOrderRequest : CreateOrderRequest
    {
        public override OrderType? Type => OrderType.LIMIT;
        
        [JsonProperty("price")] 
        [Required] 
        public float Price { get; set; }
    }
}
﻿using Microsoft.AspNetCore.Mvc;
using OneGate.Backend.Core.Shared.Api.Contracts;

namespace OneGate.Backend.Core.Users.Api.Contracts.Account
{
    public class FilterAccountsDto : FilterDto
    {
        [FromQuery(Name = "id")]
        public int? Id { get; set; }

        [FromQuery(Name = "email")]
        public string Email { get; set; }

        [FromQuery(Name = "password")]
        public string Password { get; set; }
    }
}
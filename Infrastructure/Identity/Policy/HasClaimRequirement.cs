﻿using Microsoft.AspNetCore.Authorization;
using System;

namespace PlannerApp.Infrastructure.Identity.Policy
{
    public class HasClaimRequirement : IAuthorizationRequirement
    {
        public string UserClaims { get; set; }
        public HasClaimRequirement(string userClaims)
        {
            UserClaims = userClaims?? throw new ArgumentNullException(nameof(userClaims));
        }
    }
}

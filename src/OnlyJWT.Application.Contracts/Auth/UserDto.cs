﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace OnlyJWT.Auth
{
    public class UserDto : EntityDto
    {
        public string Username { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiClient.Model
{
    public class Author
    {
        
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

    }
}

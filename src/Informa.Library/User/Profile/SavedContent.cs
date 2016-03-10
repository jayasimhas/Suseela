﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Profile
{
    public class SavedContent : ISavedContent
    {
        public DateTime SaveDate { get; set; }
        public string DocumentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

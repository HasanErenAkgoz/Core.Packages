using Core.Packages.Domain.Comman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Domain.Entities
{
    public class Translation : BaseEntity<int>
    {
        public string Key { get; set; } // Örneğin: "Permission.Create.User"
        public string Language { get; set; } // "en", "tr", "de" vb.
        public string Value { get; set; }
    }
}

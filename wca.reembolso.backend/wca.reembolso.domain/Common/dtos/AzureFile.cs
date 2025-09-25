using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wca.reembolso.domain.Common.dtos
{
    public class AzureFile
    {
        public Stream Data { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }

    }
}
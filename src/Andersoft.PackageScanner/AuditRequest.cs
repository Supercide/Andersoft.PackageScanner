using System;
using System.Collections.Generic;

namespace Andersoft.PackageScanner
{
    public class AuditRequest
    {
        public string Version { get; set; }
        public string Project { get; set; }
        public IList<ProjectPackages> Packages { get; set; }
        public Guid ApiKey { get; set; }
    }
}
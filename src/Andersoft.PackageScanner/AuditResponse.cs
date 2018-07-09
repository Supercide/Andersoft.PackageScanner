using System.Collections.Generic;

namespace Andersoft.PackageScanner
{
    public class AuditResponse
    {
        public IList<Package> Packages { get; set; }
    }
}
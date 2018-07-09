using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Andersoft.PackageScanner
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            foreach (var argument in args)
            {
                var tokens = argument.Split("=");
                arguments.Add(tokens[0], tokens[1]);
            }

            var packageConfigs = Directory.EnumerateFiles(arguments["directory"], "package.config", SearchOption.AllDirectories);
            var packageCSPROJS = Directory.EnumerateFiles(arguments["directory"], "*.csproj", SearchOption.AllDirectories);

            var packages = ReadNugetPackages(packageConfigs);
            packages.AddRange(ReadFromCSPROJ(packageCSPROJS));
            
            AuditRequest request = new AuditRequest
            {
                Version = "someVersion",
                ApiKey = Guid.NewGuid(),
                Packages = packages.ToArray(),
                Project = "someProject"
            };

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri($"http://{arguments["domain"]}")
            };
            client.DefaultRequestHeaders.Add("X-API-KEY", arguments["apikey"]);
            var response = await client.PostAsync("/api/packages", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "applicatio/json"));
            Console.WriteLine(response.StatusCode);
            Console.ReadKey();
        }

        private static List<ProjectPackages> ReadNugetPackages(IEnumerable<string> packageConfigs)
        {
            List<ProjectPackages> packages = new List<ProjectPackages>();
            foreach (var packageConfig in packageConfigs)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Packages));
                using (TextReader reader = new StringReader(File.ReadAllText(packageConfig)))
                {
                    Packages packagesConfig = (Packages) serializer.Deserialize(reader);
                    packages.AddRange(packagesConfig.Package.Select(package => new ProjectPackages
                    {
                        Name = package.Id,
                        Version = package.Version,
                        PackageManager = "nuget"
                    }).ToList());
                }
            }

            return packages;
        }

        private static List<ProjectPackages> ReadFromCSPROJ(IEnumerable<string> csproj)
        {
            Regex regex = new Regex("PackageReference.*Include=\"(?<name>\\S+)\".*Version=\"(?<version>\\S+)\"", RegexOptions.Multiline);
            List<ProjectPackages> packages = new List<ProjectPackages>();
            foreach (var packageConfig in csproj)
            {
                var proj = File.ReadAllText(packageConfig);
                packages.AddRange(regex.Matches(proj)
                     .Select(x => new ProjectPackages { Name = x.Groups["name"].Value, Version = x.Groups["version"].Value, PackageManager = "nuget" })
                     .ToArray());
            }

            return packages;
        }
    }
}

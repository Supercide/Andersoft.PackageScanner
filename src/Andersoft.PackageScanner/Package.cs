namespace Andersoft.PackageScanner
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public class Package
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Id { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Version { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TargetFramework { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool DevelopmentDependency { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool DevelopmentDependencySpecified { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AllowedVersions { get; set; }
    }
}
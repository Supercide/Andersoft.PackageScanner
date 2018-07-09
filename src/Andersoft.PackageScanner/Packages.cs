namespace Andersoft.PackageScanner
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public class Packages
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("package")]
        public Package[] Package { get; set; }
    }
}
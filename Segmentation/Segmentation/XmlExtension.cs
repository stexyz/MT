using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Segmentation
{
    public static class XmlExtension
    {
        public static XmlElement GetChildElement(this XmlElement element, string name) {
            return element.ChildNodes.OfType<XmlElement>().SingleOrDefault(e => e.LocalName.Equals(name));
        }

        public static IEnumerable<XmlElement> GetChildElements(this XmlElement element, string name) {
            return element.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName.Equals(name));
        }

    }
}
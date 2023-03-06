using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using UiPath.MultipleXmlMerging.Activities.Design.Designers;
using UiPath.MultipleXmlMerging.Activities.Design.Properties;

namespace UiPath.MultipleXmlMerging.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(MultipleXmlMerging), categoryAttribute);
            builder.AddCustomAttributes(typeof(MultipleXmlMerging), new DesignerAttribute(typeof(MultipleXmlMergingDesigner)));
            builder.AddCustomAttributes(typeof(MultipleXmlMerging), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}

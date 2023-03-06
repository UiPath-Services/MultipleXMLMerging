using System;
using System.Activities;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using UiPath.MultipleXmlMerging.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using System.Windows.Controls;

namespace UiPath.MultipleXmlMerging.Activities
{
    [LocalizedDisplayName(nameof(Resources.MultipleXmlMerging_DisplayName))]
    [LocalizedDescription(nameof(Resources.MultipleXmlMerging_Description))]
    public class MultipleXmlMerging : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.MultipleXmlMerging_SourceXMLFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.MultipleXmlMerging_SourceXMLFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> SourceXMLFilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.MultipleXmlMerging_DestinationXMLFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.MultipleXmlMerging_DestinationXMLFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public InOutArgument<string> DestinationXMLFilePath { get; set; }

        #endregion


        #region Constructors

        public MultipleXmlMerging()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SourceXMLFilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SourceXMLFilePath)));
            if (DestinationXMLFilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(DestinationXMLFilePath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var sourcexmlfilepath = SourceXMLFilePath.Get(context);
            var destinationxmlfilepath = DestinationXMLFilePath.Get(context);

            var sourcexml = XDocument.Load(sourcexmlfilepath);
            var destinationxml = XDocument.Load(destinationxmlfilepath);

            var combinedUnique = sourcexml.Descendants("catalog").Concat(destinationxml.Descendants("catalog"));

            XDocument doc = new XDocument(new XElement("root", combinedUnique));
            doc.Save(destinationxmlfilepath);

            // Outputs
            return (ctx) => {
                DestinationXMLFilePath.Set(ctx, destinationxmlfilepath);
            };
        }

        #endregion
    }
}


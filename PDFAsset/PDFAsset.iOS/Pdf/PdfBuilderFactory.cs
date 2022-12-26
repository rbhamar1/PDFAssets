using Pdfgenerator.iOS.Pdf;
using PdfGenerator.Pdf;
using PDFLibrary;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFAsset.iOS.Pdf.PdfBuilderFactory))]
namespace PDFAsset.iOS.Pdf
{
    public sealed class PdfBuilderFactory : IPdfBuilderFactory
    {
        public PdfBuilderFactory()
        {
            _fontCache = new FontCache();

            _textAttributesCache = new TextAttributesCache();
        }

        public IPdfBuilder Create(bool enableDebugLogging = false) => new PdfBuilder(_fontCache, _textAttributesCache, DOCUMENT_TRAILER_MM, enableDebugLogging);

        // make the last page a bit longer so that the bottom of the document clears the paper tearing edge of the printer
        private const double DOCUMENT_TRAILER_MM = 10;

        private readonly FontCache _fontCache;
        private readonly TextAttributesCache _textAttributesCache;
    }
}
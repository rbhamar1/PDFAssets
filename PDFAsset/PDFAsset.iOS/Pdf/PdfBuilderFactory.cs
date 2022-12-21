using Dawn;
using Pdfgenerator.iOS.Pdf;
using PdfGenerator.Pdf;
using PDFLibrary;

namespace PDFAsset.iOS.Pdf
{
    public sealed class PdfBuilderFactory : IPdfBuilderFactory
    {
        public PdfBuilderFactory(FontCache fontCache, TextAttributesCache textAttributesCache)
        {
            _fontCache = Guard.Argument(fontCache, nameof(fontCache))
                              .NotNull();

            _textAttributesCache = Guard.Argument(textAttributesCache, nameof(textAttributesCache))
                                        .NotNull();
        }

        public IPdfBuilder Create(bool enableDebugLogging = false) => new PdfBuilder(_fontCache, _textAttributesCache, DOCUMENT_TRAILER_MM, enableDebugLogging);

        // make the last page a bit longer so that the bottom of the document clears the paper tearing edge of the printer
        private const double DOCUMENT_TRAILER_MM = 10;

        private readonly FontCache _fontCache;
        private readonly TextAttributesCache _textAttributesCache;
    }
}
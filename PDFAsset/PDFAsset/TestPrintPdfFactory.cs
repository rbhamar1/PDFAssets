using System;
using System.Collections.Generic;
using PdfGenerator.Pdf;
using PDFLibrary;
using Xamarin.Forms;

namespace PDFAsset
{
    public sealed class TestPrintPdfFactory : ITestPrintPdfFactory
    {
        public TestPrintPdfFactory()
        {
            IPdfBuilderFactory factory = DependencyService.Get<IPdfBuilderFactory>();
            _pdfBuilder = factory.Create();
        }

        public  Pdf Create()
        {
            var pageTemplate = new PdfPageTemplate(__pageMargins);
            var pdfBuilder = _pdfBuilder;
            var pages = BuildTestPrint(pageTemplate, pdfBuilder);

            var pdf = pdfBuilder.Generate(pageTemplate, pages);

            return pdf;
        }

        private IEnumerable<PdfPage> BuildTestPrint(PdfPageTemplate pageTemplate, IPdfStringMeasurer stringMeasurer)
        {
            var font = PdfFontFactory.CreateSmallRegularFont();

            var versionText = $"{"PdfAssest 1.0"}";
            var decimalLine = new string('=', PAGE_WIDTH_CHARACTERS);
            var now = DateTime.Now;
            var dateText = now.Date.ToString("MM-DD-YY");
            var timeText = now.ToString("MM-DD-YYYY");
            var fullName = $"Sample User";

            // Note: this document is mostly decimal-spaced, which is very uncommon
            var paginator = new PdfPaginator(pageTemplate, stringMeasurer)
                           .WithFont(font)
                           .AddRow(versionText, TextAlignment.End)
                           .AddBlankRow()
                           .AddRow(decimalLine)
                           .AddBlankRow()
                           .AddRow(new PdfTextRow()
                                  .AddPromptText("DATE", new PdfTextPlacement(FIRST_COLUMN_MM, 0, 7 * font.CharWidthMm))
                                  .AddText(dateText, TextAlignment.Start, new PdfTextPlacement(SECOND_COLUMN_MM)))
                           .AddRow(new PdfTextRow()
                                  .AddPromptText("TIME", new PdfTextPlacement(FIRST_COLUMN_MM, 0, 7 * font.CharWidthMm))
                                  .AddText(timeText, TextAlignment.Start, new PdfTextPlacement(SECOND_COLUMN_MM)))
                           .AddBlankRow()
                           .AddRow(fullName)
                           .AddBlankRow()
                           .AddRow("Test Print was succesful")
                           .AddBlankRow()
                           .AddRow(decimalLine)
                           .AddBlankRow()
                           .AddRow("ABCDEFGHJIKLMNOPQRSTUVWYXYZ", TextAlignment.Center)
                           .AddBlankRow()
                           .AddRow("ABCDEFGHJIKLMNOPQRSTUVWYXYZ".ToLower(), TextAlignment.Center)
                           .AddBlankRow()
                           .AddRow("123456789", TextAlignment.Center)
                           .AddBlankRow()
                           .AddBlankRow()
                           .AddRow("-----End of the Report--------", TextAlignment.Center);

            return paginator.GetPages();
        }

        private static readonly Thickness __pageMargins = new Thickness(2.0, 0);

        private const int PAGE_WIDTH_CHARACTERS = 80;

        private const double FIRST_COLUMN_MM = 78.75;
        private const double SECOND_COLUMN_MM = 86.25;
        private readonly IPdfBuilder _pdfBuilder;
    }
}
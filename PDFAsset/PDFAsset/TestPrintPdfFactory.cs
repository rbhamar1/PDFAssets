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

            var versionText = "PdfAsset 1.0";
            var decimalLine = new string('=', PAGE_WIDTH_CHARACTERS);
            var now = DateTime.Now;
            var dateText = now.Date.ToString("MM-dd-yy");
            var timeText = now.ToString("MM-dd-yyyy");

            var paginator = new PdfPaginator(pageTemplate, stringMeasurer)
                .WithFont(font);
            
            var mainHeaderRegion = AddHeaderSection(asnDocument, smallBoldFont, smallRegularFont);
            paginator.MakeSureCurrentPage();

            paginator.AddStaticImage(EmbeddedSourceror.SourceFor(SalesHubConstants.PEPSICO_LOGO), 5, 5, 60, 20)
                .AddRegions(mainHeaderRegion);

            // Note: this document is mostly decimal-spaced, which is very uncommon
            paginator.AddRow(versionText, TextAlignment.End)
                           .AddBlankRow()
                           .AddRow(decimalLine)
                           .AddBlankRow()
                           .AddRow(new PdfTextRow()
                                  .AddPromptText("DATE", new PdfTextPlacement(FIRST_COLUMN_MM, 0, 7 * font.CharWidthMm))
                                  .AddText(dateText, TextAlignment.Start, new PdfTextPlacement(SECOND_COLUMN_MM)))
                           .AddRow(new PdfTextRow()
                                  .AddPromptText("TIME", new PdfTextPlacement(FIRST_COLUMN_MM, 0, 7 * font.CharWidthMm))
                                  .AddText(timeText, TextAlignment.Start, new PdfTextPlacement(SECOND_COLUMN_MM)))
                           .AddBlankRow();
            AddProductsHeaderSection(paginator, PdfFontFactory.CreateSmallBoldFont());
            AddProducts(paginator,font);
            
                paginator.AddBlankRow()
                           .AddRow(decimalLine)
                           .AddBlankRow()
                           .AddRow("This is a sample report", TextAlignment.Center)
                           .AddBlankRow()
                           .AddBlankRow()
                           .AddRow("-----End of the Report--------", TextAlignment.Center);

            return paginator.GetPages();
        }
        
         private PdfTextRegion AddHeaderSection( PdfFont smallBoldFont)
        {
            var mainHeaderRegion = new PdfTextRegion(new Point(PAGE_WIDTH_CHARACTERS, 0))
                                  .AddRow(new PdfTextRow()
                                             .AddText($"Sample Text", smallBoldFont, 40, 30))
                                  .AddBlankRow();

            return mainHeaderRegion;
        }

        
        private void AddProductsHeaderSection(PdfPaginator paginator, PdfFont regularBoldFont)
        {
            paginator.AddRow(new PdfTextRow()
                .AddText($"ItemName", regularBoldFont, 3, 10, TextAlignment.Center)
                .AddText($"ItemDescription", regularBoldFont, 13, 20, TextAlignment.Center)
                .AddText($"ItemCount", regularBoldFont, 33 , 12, TextAlignment.Center)
                .AddText($"ItemPrice", regularBoldFont, 45, 14, TextAlignment.Center));
        }
        
        private void AddProducts(PdfPaginator paginator, PdfFont regularBoldFont)
        {
            for (int i = 1; i < 5; i++)
            {
                paginator.AddRow(new PdfTextRow()
                    .AddText($"ItemName{i}", regularBoldFont, 5, 10, TextAlignment.Center)
                    .AddText($"SampleDescription{i}", regularBoldFont, 19, 20, TextAlignment.Center)
                    .AddText($"ItemCount{i}", regularBoldFont, 42 , 12, TextAlignment.Center)
                    .AddText($"ItemPrice{i}", regularBoldFont, 58, 14, TextAlignment.Center));
            }
            
        }

        private static readonly Thickness __pageMargins = new Thickness(2.0, 0);

        private const int PAGE_WIDTH_CHARACTERS = 80;

        private const double FIRST_COLUMN_MM = 78.75;
        private const double SECOND_COLUMN_MM = 86.25;
        private readonly IPdfBuilder _pdfBuilder;
    }
}
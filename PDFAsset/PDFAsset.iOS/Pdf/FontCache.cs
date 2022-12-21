using System;
using System.Collections.Generic;
using CoreGraphics;
using Dawn;
using Foundation;
using PDFLibrary;
using UIKit;

namespace PDFAsset.iOS.Pdf
{
    public sealed class FontCache
    {
        public UIFont GetFont(PdfFont pdfFont)
        {
            Guard.Argument(pdfFont, nameof(pdfFont))
                 .NotNull();

            if (!_fonts.TryGetValue(pdfFont, out var nativeFont))
            {
                nativeFont = CreateNativeFont(pdfFont.FontName, pdfFont.CharWidthMm, pdfFont.LineHeightMm);

                _fonts.Add(pdfFont, nativeFont);
            }

            return nativeFont;
        }

        private static UIFont CreateNativeFont(string fontName, double charWidthMm, double lineHeightMm)
        {
            var sourceFont = UIFont.FromName(fontName, __arbitraryFontSize);
            double fontSizeScaling = __arbitraryFontSize / sourceFont.LineHeight;

            var fontSize = PdfUnitConversion.ConvertMmsToPoints(lineHeightMm) * fontSizeScaling;
            var sizedFont = UIFont.FromName(fontName, (nfloat) fontSize);

            var nativeString = new NSAttributedString(MEASURE_TEXT, sizedFont);
            var characterSize = nativeString.Size;

            var horizontalScaling = PdfUnitConversion.ConvertMmsToPoints(charWidthMm) / characterSize.Width;

           // __logger.Debug("{Member}: fontSizeScaling={FontSizeScaling}, character size=({CharWidth}, {CharHeight}), horizontal scaling={HorizontalScaling}",
                         //  nameof(CreateNativeFont), fontSizeScaling, characterSize.Width, characterSize.Height, horizontalScaling);

            var scaledFont = CreateNativeFont(sizedFont, fontSize, horizontalScaling, 1);

            //__logger.Information("{Member}: requested font (\"{FontName}\", w={Width} mm, lh={LineHeightMm} mm) resulted in a font with size={FontSize} points, LineHeight={LineHeightPoint} points",
                                // nameof(CreateNativeFont), fontName, charWidthMm, lineHeightMm, fontSize.ToString("F2"), scaledFont.LineHeight.ToString("F2"));

            return scaledFont;
        }

        private static UIFont CreateNativeFont(UIFont sourceFont, double fontSize, double horizontalScaling, double verticalScaling)
        {
            var sourceFontDescriptor = sourceFont.FontDescriptor;

            var scaledFontDescriptor = sourceFontDescriptor.CreateWithMatrix(CGAffineTransform.MakeScale((nfloat) horizontalScaling, (nfloat) verticalScaling));
            var scaledFont = UIFont.FromDescriptor(scaledFontDescriptor, (nfloat) fontSize);

            return scaledFont;
        }

        // Note: the font is assumed to be monospace, so this can be any single character
        private const string MEASURE_TEXT = "0";

        private static readonly nfloat __arbitraryFontSize = (nfloat) 10.0;

      //  private static readonly ILogger __logger = LoggingExtensions.ForContextEx<FontCache>();

        private readonly Dictionary<PdfFont, UIFont> _fonts = new Dictionary<PdfFont, UIFont>();
    }
}
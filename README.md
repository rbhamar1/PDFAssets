PDF Library

A Library helps to create PDF using millimeters as the unit of measurement. It builds a PDF from a set of logical pages and paginates them with optional headers, footers etc.. 

Let’s get introduce with different components implemented inside a bunch :

•	Use `PdfFontFactory` to create fonts used for printing
•	`PdfPageTemplate` describes the maximum size of the printable page, as well as the margins
•	Use `PdfPageTemplateFactory` to insulate your code from particulars of the printer
•	`PdfPaginator` allows the client to append nodes representing output, which are divided into one or more instances of `PdfPage`. The printer has a maximum height of PDF to print. This means that even print output that is not paginated from the user's point of view (e.g., pick lists) must be paginated using `PdfPaginator`
•	`PdfPaginator` supports page headers and footers
Note that most of our documents specify that the footer on the final page has a different format than those on the intermediate pages
•	`PdfPaginator` supports a fluent interface. Much of our printed output consists either entirely or mostly of text with a single font
•	`PdfPaginator` has a `WithFont` method that sets the default font. This is default font getting use, if font is not provided for the text nodes that are added to the paginator
•	Once all of the output has been added, the `PdfPaginator.GetPages` method can be called to get the pages that resulted
•	`IPdfBuilder` (which is created using an `IPdfBuilderFactory`) can then be used to create a PDF

How to consume library?

•	Right click on PDFLibrary 
•	Click on Build library option. It will create .dll file at specific folder
 

•	Add .dll file as a reference to the project
•	Use components mentioned in section 1 to create and build PDF. E.g : Go to PDFAsset.iOS.Pdf.PdfBuilder file


![image](https://user-images.githubusercontent.com/94822442/211797143-ef02feef-6040-49cd-bc52-30d195a66cc6.png)

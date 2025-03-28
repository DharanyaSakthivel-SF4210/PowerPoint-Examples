﻿using Syncfusion.Presentation;
using Syncfusion.OfficeChart;
using System.IO;

class Program
{
    public static void Main(string[] args)
    {
        // Load or open a PowerPoint presentation
        using (FileStream inputStream = new(Path.GetFullPath(@"Data/Template.pptx"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (IPresentation presentation = Presentation.Open(inputStream))
            {
                // Iterate through each slide in the presentation
                foreach (ISlide slide in presentation.Slides)
                {
                    // Iterate through each shape in the master slide shapes.
                    foreach (IShape shape in slide.LayoutSlide.MasterSlide.Shapes)
                    {
                        // Modify the shape properties (text, size, hyperlinks, etc.)
                        ModifySlideElements(shape);
                    }
                    // Iterate through each shape in the layout slide shapes.
                    foreach (IShape shape in slide.LayoutSlide.Shapes)
                    {
                        // Modify the shape properties (text, size, hyperlinks, etc.)
                        ModifySlideElements(shape);
                    }
                    // Iterate through each shape in the slide
                    foreach (IShape shape in slide.Shapes)
                    {
                        // Modify the shape properties (text, size, hyperlinks, etc.)
                        ModifySlideElements(shape);
                    }
                }

                // Save the modified presentation to an output file
                using (FileStream outputStream = new(Path.GetFullPath(@"Output/Result.pptx"), FileMode.Create, FileAccess.ReadWrite))
                {
                    presentation.Save(outputStream);
                }
            }
        }
    }

    /// <summary>
    /// Modifies slide elements based on their type.
    /// </summary>
    private static void ModifySlideElements(IShape shape )
    {
        switch (shape.SlideItemType)
        {
            case SlideItemType.AutoShape:
                {
                    // Modify text if present in the shape
                    if (!string.IsNullOrEmpty(shape.TextBody.Text))
                    {
                        ModifyTextPart(shape.TextBody);
                    }
                    // If shape is a rectangle, add a hyperlink
                    else if (shape.AutoShapeType == AutoShapeType.Rectangle)
                    {
                        shape.SetHyperlink("www.google.com");
                    }
                    break;
                }

            case SlideItemType.ConnectionShape:
                {
                    IConnector connector = shape as IConnector;
                    // Modify the arrowhead style at the beginning of the connector line
                    connector.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowDiamond;
                    break;
                }

            case SlideItemType.Placeholder:
                {
                    // Modify text if present in the placeholder
                    if (!string.IsNullOrEmpty(shape.TextBody.Text))
                    {
                        ModifyTextPart(shape.TextBody);
                    }
                    break;
                }

            case SlideItemType.Picture:
                {
                    // Resize the picture
                    IPicture picture = shape as IPicture;
                    picture.Height = 160;
                    picture.Width = 130;
                    break;
                }

            case SlideItemType.Table:
                {
                    // Get the table shape
                    ITable table = shape as ITable;

                    // Iterate through rows and modify text in each cell
                    foreach (IRow row in table.Rows)
                    {
                        foreach (ICell cell in row.Cells)
                        {
                            ModifyTextPart(cell.TextBody);
                        }
                    }
                    break;
                }

            case SlideItemType.GroupShape:
                {
                    // Get the group shape and iterate through child shapes
                    IGroupShape groupShape = shape as IGroupShape;
                    foreach (IShape childShape in groupShape.Shapes)
                    {
                        ModifySlideElements(childShape);
                    }
                    break;
                }

            case SlideItemType.Chart:
                {
                    // Modify chart properties
                    IPresentationChart chart = shape as IPresentationChart;
                    chart.ChartTitle = "Purchase Details";
                    chart.ChartTitleArea.Bold = true;
                    chart.ChartTitleArea.Color = OfficeKnownColors.Red;
                    chart.ChartTitleArea.Size = 20;
                    break;
                }

            case SlideItemType.SmartArt:
                {
                    // Modify SmartArt content
                    ISmartArt smartArt = shape as ISmartArt;
                    //Traverse through all nodes inside SmartArt
                    foreach (ISmartArtNode node in smartArt.Nodes)
                    {
                        ModifyTextPart(node.TextBody);
                    }
                    break;
                }

            case SlideItemType.OleObject:
                {
                    // Modify OLE object size
                    IOleObject oleObject = shape as IOleObject;
                    oleObject.Width = 300;
                    break;
                }
        }
    }

    /// <summary>
    /// Modifies the text content within a text body.
    /// </summary>
    private static void ModifyTextPart(ITextBody textBody)
    {
        // Iterate through paragraphs in the text body
        foreach (IParagraph paragraph in textBody.Paragraphs)
        {
            // Iterate through text parts and modify the text
            foreach (ITextPart textPart in paragraph.TextParts)
            {
                textPart.Text = "Adventure Works";
            }
        }
    }
}

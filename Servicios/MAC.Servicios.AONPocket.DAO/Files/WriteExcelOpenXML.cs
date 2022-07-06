using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using System.IO;
using MAC.Servicios.Modelos;

namespace MAC.Utilidades.Excel
{
	public class WriteExcelOpenXML<T>
	{
		private String _archivo = String.Empty;
		public String archivo { get => _archivo; set => _archivo = value; }
		private String _formatoFecha = String.Empty;
		public String formatoFecha { get => _formatoFecha; set => _formatoFecha = value; }
		PropertyInfo[] propInfos;
		private List<String> formatos = new List<String>();

		public WriteExcelOpenXML()
		{
			propInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		public WriteExcelOpenXML(String pArchivo, String pformatoFecha)
		{
			_archivo = pArchivo.ToLower().Replace(".xls", System.DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
			_formatoFecha = pformatoFecha;
			propInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		}

		public void WriteData(List<T> ListData)
		{
			try
			{
				using (var document = SpreadsheetDocument.Create(_archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
				{
					CreatePartsForExcel(document, ListData);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public String WriteDataB64(List<T> ListData)
		{
			String fileBase64 = String.Empty;
			try
			{
				WriteData(ListData);
				Byte[] bytes = File.ReadAllBytes(_archivo);
				fileBase64 = Convert.ToBase64String(bytes);
				File.Delete(_archivo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return fileBase64;
		}

		public void WriteData(List<T> ListData, String plantilla)
		{
			try
			{
				File.Copy(plantilla, _archivo);
				using (var document = SpreadsheetDocument.Open(_archivo, true))
				{
					CreatePartsForExcel(document, ListData, document.WorkbookPart);
					document.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		private void CreatePartsForExcel(SpreadsheetDocument document, List<T> data, WorkbookPart workbookPart)
		{
			IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
			string relationshipId = sheets.First().Id.Value;
			WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
			Worksheet workSheet = worksheetPart.Worksheet;
			SheetData sheetData = workSheet.GetFirstChild<SheetData>();
			GenerateSheetdataForDetails(data, sheetData);
			workSheet.Save();
			workbookPart.Workbook.Save();
		}

		private void CreatePartsForExcel(SpreadsheetDocument document, List<T> data)
		{
			SheetData partSheetData = GenerateSheetdataForDetails(data);

			WorkbookPart workbookPart1 = document.AddWorkbookPart();
			GenerateWorkbookPartContent(workbookPart1);

			WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
			GenerateWorkbookStylesPartContent(workbookStylesPart1);

			WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
			GenerateWorksheetPartContent(worksheetPart1, partSheetData);
		}

		private void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
		{
			Workbook workbook1 = new Workbook();
			Sheets sheets1 = new Sheets();
			Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
			sheets1.Append(sheet1);
			workbook1.Append(sheets1);
			workbookPart1.Workbook = workbook1;
		}

		private void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1)
		{
			Worksheet worksheet1 = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
			worksheet1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
			worksheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			worksheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
			SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1" };

			SheetViews sheetViews1 = new SheetViews();

			SheetView sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
			Selection selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

			sheetView1.Append(selection1);

			sheetViews1.Append(sheetView1);
			SheetFormatProperties sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };

			PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
			worksheet1.Append(sheetDimension1);
			worksheet1.Append(sheetViews1);
			worksheet1.Append(sheetFormatProperties1);
			worksheet1.Append(sheetData1);
			worksheet1.Append(pageMargins1);
			worksheetPart1.Worksheet = worksheet1;
		}

		private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
		{
			Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
			stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

			Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

			Font font1 = new Font();
			FontSize fontSize1 = new FontSize() { Val = 11D };
			Color color1 = new Color() { Theme = (UInt32Value)1U };
			FontName fontName1 = new FontName() { Val = "Calibri" };
			FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
			FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

			font1.Append(fontSize1);
			font1.Append(color1);
			font1.Append(fontName1);
			font1.Append(fontFamilyNumbering1);
			font1.Append(fontScheme1);

			Font font2 = new Font();
			Bold bold1 = new Bold();
			FontSize fontSize2 = new FontSize() { Val = 11D };
			Color color2 = new Color() { Theme = (UInt32Value)1U };
			FontName fontName2 = new FontName() { Val = "Calibri" };
			FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
			FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

			font2.Append(bold1);
			font2.Append(fontSize2);
			font2.Append(color2);
			font2.Append(fontName2);
			font2.Append(fontFamilyNumbering2);
			font2.Append(fontScheme2);

			fonts1.Append(font1);
			fonts1.Append(font2);

			Fills fills1 = new Fills() { Count = (UInt32Value)2U };

			Fill fill1 = new Fill();
			PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

			fill1.Append(patternFill1);

			Fill fill2 = new Fill();
			PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

			fill2.Append(patternFill2);

			fills1.Append(fill1);
			fills1.Append(fill2);

			Borders borders1 = new Borders() { Count = (UInt32Value)2U };

			Border border1 = new Border();
			LeftBorder leftBorder1 = new LeftBorder();
			RightBorder rightBorder1 = new RightBorder();
			TopBorder topBorder1 = new TopBorder();
			BottomBorder bottomBorder1 = new BottomBorder();
			DiagonalBorder diagonalBorder1 = new DiagonalBorder();

			border1.Append(leftBorder1);
			border1.Append(rightBorder1);
			border1.Append(topBorder1);
			border1.Append(bottomBorder1);
			border1.Append(diagonalBorder1);

			Border border2 = new Border();

			LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
			Color color3 = new Color() { Indexed = (UInt32Value)64U };

			leftBorder2.Append(color3);

			RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
			Color color4 = new Color() { Indexed = (UInt32Value)64U };

			rightBorder2.Append(color4);

			TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
			Color color5 = new Color() { Indexed = (UInt32Value)64U };

			topBorder2.Append(color5);

			BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
			Color color6 = new Color() { Indexed = (UInt32Value)64U };

			bottomBorder2.Append(color6);
			DiagonalBorder diagonalBorder2 = new DiagonalBorder();

			border2.Append(leftBorder2);
			border2.Append(rightBorder2);
			border2.Append(topBorder2);
			border2.Append(bottomBorder2);
			border2.Append(diagonalBorder2);

			borders1.Append(border1);
			borders1.Append(border2);

			CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
			CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

			cellStyleFormats1.Append(cellFormat1);

			CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
			CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
			CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
			CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

			cellFormats1.Append(cellFormat2);
			cellFormats1.Append(cellFormat3);
			cellFormats1.Append(cellFormat4);

			CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
			CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

			cellStyles1.Append(cellStyle1);
			DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
			TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

			StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

			StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
			stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
			X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

			stylesheetExtension1.Append(slicerStyles1);

			stylesheetExtensionList1.Append(stylesheetExtension1);

			stylesheet1.Append(fonts1);
			stylesheet1.Append(fills1);
			stylesheet1.Append(borders1);
			stylesheet1.Append(cellStyleFormats1);
			stylesheet1.Append(cellFormats1);
			stylesheet1.Append(cellStyles1);
			stylesheet1.Append(differentialFormats1);
			stylesheet1.Append(tableStyles1);
			stylesheet1.Append(stylesheetExtensionList1);

			workbookStylesPart1.Stylesheet = stylesheet1;
		}

		private void GenerateSheetdataForDetails(List<T> data, SheetData sheetData1)
		{
			sheetData1.Append(CreateHeaderRowForExcel());

			foreach (var fila in data)
			{
				Row partsRows = GenerateRowForChildPartDetail(fila);
				sheetData1.Append(partsRows);
			}
		}

		private SheetData GenerateSheetdataForDetails(List<T> data)
		{
			SheetData sheetData1 = new SheetData();
			sheetData1.Append(CreateHeaderRowForExcel());

			foreach (var fila in data)
			{
				Row partsRows = GenerateRowForChildPartDetail(fila);
				sheetData1.Append(partsRows);
			}
			return sheetData1;
		}

		private void CreateHeaderRowForExcel(SheetData sheetData1)
		{
			Row workRow = new Row();
			Type type;
			String campo = String.Empty;
			String titulo = String.Empty;
			String formato = String.Empty;
			foreach (var prop in propInfos)
			{
				type = prop.PropertyType;
				campo = prop.Name;
				titulo = campo;
				IEnumerable<DisplayAttribute> propertyAttributes = prop.GetCustomAttributes<DisplayAttribute>();
				if (propertyAttributes.Count() > 0)
				{
					//titulo = propertyAttributes.ToArray()[0].Name;
					titulo = propertyAttributes.ToArray()[0].Description;
					if (String.IsNullOrEmpty(titulo))
					{
						titulo = propertyAttributes.ToArray()[0].Name;
						titulo = String.IsNullOrEmpty(titulo) ? campo : titulo;
					}
				}
				formato = String.Empty;
				IEnumerable<DisplayFormatAttribute> propertyAttributesF = prop.GetCustomAttributes<DisplayFormatAttribute>();
				if (propertyAttributesF.Count() > 0)
				{
					formato = propertyAttributesF.ToArray()[0].DataFormatString;
				}
				formatos.Add(formato);
				workRow.Append(CreateCell(titulo, 2U));
			}
		}

		private Row CreateHeaderRowForExcel()
		{
			Row workRow = new Row();
			Type type;
			String campo = String.Empty;
			String titulo = String.Empty;
			String formato = String.Empty;
			foreach (var prop in propInfos)
			{
				type = prop.PropertyType;
				campo = prop.Name;
				titulo = campo;
				IEnumerable<DisplayAttribute> propertyAttributes = prop.GetCustomAttributes<DisplayAttribute>();
				if (propertyAttributes.Count() > 0)
				{
					//titulo = propertyAttributes.ToArray()[0].Name;
					titulo = propertyAttributes.ToArray()[0].Description;
					if (String.IsNullOrEmpty(titulo))
					{
						titulo = propertyAttributes.ToArray()[0].Name;
						titulo = String.IsNullOrEmpty(titulo) ? campo : titulo;
					}
				}
				formato = String.Empty;
				IEnumerable<DisplayFormatAttribute> propertyAttributesF = prop.GetCustomAttributes<DisplayFormatAttribute>();
				if (propertyAttributesF.Count() > 0)
				{
					formato = propertyAttributesF.ToArray()[0].DataFormatString;
				}
				formatos.Add(formato);
				workRow.Append(CreateCell(titulo, 2U));
			}
			return workRow;
		}

		private object valor(object Data, String propertyName, Type tipo, String formato)
		{
			object retval = null;
			PropertyInfo pi = Data.GetType().GetProperty(propertyName);

			if (pi == null) return String.Empty;
			retval = pi.GetValue(Data, null);
			if (tipo.FullName.ToLower().IndexOf("date") >= 0)
			{
				DateTime date = (DateTime)retval;
				String format = formato;
				if (String.IsNullOrEmpty(format))
				{
					format = _formatoFecha;
					if (date.Hour > 0 || date.Minute > 0 || date.Second > 0 || date.Millisecond > 0)
					{
						format += " hh:mm:ss";
					}
				}
				retval = date.ToString(_formatoFecha);
			}
			return retval;
		}

		private Row GenerateRowForChildPartDetail(T fila)
		{
			Row tRow = new Row();
			String formato = String.Empty;
			PropertyInfo prop;
			for (int x = 0; x < propInfos.Length; x++)
			{
				prop = propInfos[x];
				formato = formatos[x];
				tRow.Append(CreateCell(valor(fila, prop.Name, prop.PropertyType, formato).ToString()));
			}
			return tRow;
		}

		private Cell CreateCell(string text)
		{
			Cell cell = new Cell();
			cell.StyleIndex = 1U;
			cell.DataType = ResolveCellDataTypeOnValue(text);
			cell.CellValue = new CellValue(text);
			return cell;
		}

		private Cell CreateCell(string text, uint styleIndex)
		{
			Cell cell = new Cell();
			cell.StyleIndex = styleIndex;
			cell.DataType = ResolveCellDataTypeOnValue(text);
			cell.CellValue = new CellValue(text);
			return cell;
		}

		private EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
		{
			int intVal;
			double doubleVal;
			if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
			{
				return CellValues.Number;
			}
			else
			{
				return CellValues.String;
			}
		}

		public static String UpdateSheet(string fileName, List<ExcelData> Data, String identificador, Boolean TempFile)
		{
			// Open the document for editing.
			try
            {
				fileName = fileName.ToLower();
				if (TempFile)
				{
					String filePlantilla = fileName;
					String extension = Path.GetExtension(fileName).ToLower();
					String namePlantilla = Path.GetFileName(fileName).ToLower();
					identificador = String.Concat(identificador, System.DateTime.Now.ToString("yyyyMMddhhmmss"), extension);
					fileName = fileName.Replace(namePlantilla, identificador);
					File.Copy(filePlantilla, fileName);
				}
				using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(fileName, true))
				{
					// Access the main Workbook part, which contains all references.
					WorkbookPart workbookPart = spreadSheet.WorkbookPart;

					String valor = String.Empty;
					String relId = String.Empty;
					List<String> ListaHojas = Data.Select(x => x.Hoja).Distinct().ToList();
					List<ExcelData> CeldasHoja = new List<ExcelData>();
					foreach (string hoja in ListaHojas)
					{
						// get sheet by name
						CeldasHoja = Data.Where(x => x.Hoja.Equals(hoja)).ToList();
						Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name==hoja.Trim()).FirstOrDefault();

						// get worksheetpart by sheet id
						WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;
						foreach (ExcelData celdata in CeldasHoja)
						{

							// The SheetData object will contain all the data.
							SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
							Cell cell = GetCell(worksheetPart.Worksheet, celdata.Columna, celdata.Fila);
							if (celdata.Valor == null) { celdata.Valor = String.Empty; }
							Type t = celdata.Valor.GetType();
							valor = String.Empty;
							if (celdata != null) { valor = celdata.Valor.ToString(); }
							cell.CellValue = new CellValue(valor);
							if (t.Equals(typeof(byte)) || t.Equals(typeof(sbyte)) || t.Equals(typeof(int)) || t.Equals(typeof(long)) || t.Equals(typeof(short)) || t.Equals(typeof(double)) || t.Equals(typeof(decimal)))
							{
								cell.DataType = new EnumValue<CellValues>(CellValues.Number);
							}
							else if (t.Equals(typeof(DateTime)))
							{
								cell.DataType = new EnumValue<CellValues>(CellValues.Date);
							}
							else if (t.Equals(typeof(Boolean)))
							{
								cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);
							}
							else
							{
								cell.DataType = new EnumValue<CellValues>(CellValues.String);
							}
						}
						// Save the worksheet.
						worksheetPart.Worksheet.Save();
					}
					// for recacluation of formula
					spreadSheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
					spreadSheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
				}
			}
			catch(Exception ex)
            {
				throw ex;
            }
			return identificador;
		}

		private static Cell GetCell(Worksheet worksheet,string columnName, uint rowIndex)
		{
			Row row = GetRow(worksheet, rowIndex);

			if (row == null) return null;

			var FirstRow = row.Elements<Cell>().Where(c => string.Compare
			(c.CellReference.Value, columnName +
			rowIndex, true) == 0).FirstOrDefault();

			if (FirstRow == null) return null;

			return FirstRow;
		}

		private static Row GetRow(Worksheet worksheet, uint rowIndex)
		{
			Row row = worksheet.GetFirstChild<SheetData>().
			Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
			if (row == null)
			{
				throw new ArgumentException(String.Format("No row with index {0} found in spreadsheet", rowIndex));
			}
			return row;
		}

	}
}

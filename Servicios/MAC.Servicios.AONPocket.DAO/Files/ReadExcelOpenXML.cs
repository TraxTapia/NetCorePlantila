using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MAC.Utilidades
{
	public class ReadExcelOpenXML
	{
		const String defaultDates = "dd/MM/yyyy";

		private String _formatDates = defaultDates;
		public String formatDates { get => _formatDates; set => _formatDates = value; }
		private CellFormats cellFormats;
		private NumberingFormats numberingFormats;
		private String[] DateFormats = { "14", "15", "16", "17" };
		private String _archivo = String.Empty;
		public String archivo { get => _archivo; set => _archivo = value; }
		public ReadExcelOpenXML()
		{

		}

		public ReadExcelOpenXML(String pArchivo)
		{
			_archivo = pArchivo;
		}

		public Object[,] LeerHoja(Boolean conencabezado = false)
		{
			try
			{
				return getData(SpreadsheetDocument.Open(_archivo,false), conencabezado);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Object[,] LeerHoja(byte[] Data, Boolean conencabezado = false)
		{
			try
			{
				using (MemoryStream mem = new MemoryStream())
				{
					mem.Write(Data, 0, (int)Data.Length); ;
					return getData(SpreadsheetDocument.Open(mem, false), conencabezado);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public Object[,] getData(SpreadsheetDocument doc,Boolean conencabezado=false)
		{
			Object[,] result = null;
			try
			{
				using (var document = doc)
				{
					WorkbookPart workbookPart = document.WorkbookPart;
					IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
					string relationshipId = sheets.First().Id.Value;
					WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
					Worksheet workSheet = worksheetPart.Worksheet;
					SheetData sheetData = workSheet.GetFirstChild<SheetData>();
					IEnumerable<Row> rows = sheetData.Descendants<Row>();
					cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
					numberingFormats = workbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats;

					int irow = conencabezado ? 0 : -1;
					int totRows = rows.Count();
					if (!conencabezado) { totRows -= 1; }
					result = new Object[rows.ElementAt(0).Count(), totRows];
					if (rows.Count() > 0)
					{
						Row rowEncabezado = rows.First();
						int numerodeColumnas = rowEncabezado.Descendants<Cell>().Count();
						int columnasrow = 0;
						Boolean isDate = false;
						Object obStyle = null;
						foreach (Row row in rows)
						{
							if (irow >= 0) //Encabezado
							{
								columnasrow = row.Descendants<Cell>().Count();
								for (int col = 0; col < numerodeColumnas; col++)
								{
									isDate = false;
									obStyle = row.Descendants<Cell>().ElementAt(col).StyleIndex;
									if (obStyle != null)
									{
										var styleIndex = (int)row.Descendants<Cell>().ElementAt(col).StyleIndex.Value;
										var cellFormat = (CellFormat)cellFormats.ElementAt(styleIndex);
										if (cellFormat.NumberFormatId != null)
										{
											var numberFormatId = cellFormat.NumberFormatId.Value;
											NumberingFormat numberingFormat = null;
											if (numberingFormats != null)
											{
												numberingFormat = numberingFormats.Cast<NumberingFormat>()
												.SingleOrDefault(f => f.NumberFormatId.Value == numberFormatId);
												// Here's yer string! Example: $#,##0.00_);[Red]($#,##0.00)
												if (numberingFormat != null && (numberingFormat.FormatCode.Value.Contains("yyyy") || numberingFormat.FormatCode.Value.Contains("dd/")))
												{
													isDate = true;
												}
											}
											else
											{
												if (DateFormats.Contains(cellFormat.NumberFormatId)) { isDate = true; }
											}
										}
									}
									if (columnasrow > 0)
									{
										result[col, irow] = GetCellValue(document, row.Descendants<Cell>().ElementAt(col), isDate);
									}
									else
									{
										result[col, irow] = String.Empty;
									}
								}
							}
							irow++;
						}
					}
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private string GetCellValue(SpreadsheetDocument document, Cell cell, Boolean isDate)
		{
			string value = String.Empty;
			try
			{
				SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
				if (cell.CellValue != null)
				{
					value = cell.CellValue.InnerXml;
					if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
					{
						return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
					}
					else
					{
						if (isDate)
						{
							return DateTime.FromOADate(double.Parse(value)).ToString(_formatDates);
						}
						else
						{
							return value;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return value;
		}
	}
}


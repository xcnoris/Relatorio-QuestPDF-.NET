using Modelos.IntegradorCRM.Models.EF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

public class QuestPDFService
{

   
    private List<Controle_Liberacao_ComissaoModel> Controle_Liberacao_ComissaoList;
    private string vendedor { get; set; }
    private string StartDate { get; set; }
    private string EndDate { get; set; }
  

    public QuestPDFService(List<Controle_Liberacao_ComissaoModel> comissaoList, string vendedor, string startDate, string endDate)
    {
        Controle_Liberacao_ComissaoList = comissaoList;
        this.vendedor = vendedor;
        StartDate = startDate;
        EndDate = endDate;
    }
    public void GerarPDF(string filePath)
    {
       
        Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);



                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem(3)
                            .Height(100)
                            .Column(column =>
                            {
                                column.Item().Text("Relatorio de Comissão").FontSize(25).FontColor(Colors.Blue.Medium).SemiBold();
                                column.Item().Text($"Vendedor : {vendedor}").FontSize(10);
                                column.Item().Text($"Periodo: {StartDate} - {EndDate}").FontSize(10);
                            });
                            
                        row.RelativeItem(1)
                          .Height(100)
                          .Image("4.png");
                    });
                 


                page.Content()
                    .Column(col =>
                    {
                        col.Item().Text("Comissão Detalhada:").FontSize(20).Bold();
                        col.Item().Text("");
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.RelativeColumn(70);
                                columns.RelativeColumn(90);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(90);
                                columns.RelativeColumn(50);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(1).Text("Ped. Venda").Bold().FontColor(Colors.Orange.Accent3);
                                header.Cell().BorderBottom(1).Text("NR Nota").Bold().FontColor(Colors.Blue.Accent2);
                                header.Cell().BorderBottom(1).Text("Emis. NF").Bold().FontColor(Colors.Blue.Accent2);
                                header.Cell().BorderBottom(1).Text("Id DR").Bold().FontColor(Colors.Green.Accent2);
                                header.Cell().BorderBottom(1).Text("Quitação").Bold().FontColor(Colors.Green.Accent2);
                                header.Cell().BorderBottom(1).Text("NR. Gerados").Bold().FontColor(Colors.Green.Accent2);
                                header.Cell().BorderBottom(1).Text("Comissão").Bold().FontColor(Colors.Red.Accent2);
                            });


                            int i = 0;
                            foreach (var comissaoItem in Controle_Liberacao_ComissaoList)
                            {
                                table.Cell().Element(CellStyle).Text(comissaoItem.Id_Pedido_Venda.ToString());
                                table.Cell().Element(CellStyle).Text(comissaoItem.numero_documento_fiscal.ToString());
                                table.Cell().Element(CellStyle).Text(comissaoItem.data_hora_emissao_nota.Value.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(comissaoItem.Id_Documento_Receber.ToString());
                                table.Cell().Element(CellStyle).Text(comissaoItem.Data_Quitacao.HasValue ? comissaoItem.Data_Quitacao.Value.ToString("dd/MM/yyyy") : "ABERTO");
                                table.Cell().Element(CellStyle).Text(comissaoItem.DR_Total_Gerados.ToString());
                                table.Cell().Element(CellStyle).Text("R$ " + comissaoItem.Valor_Comissao_Por_Parcela.ToString("F2")).AlignRight().AlignLeft();

                                IContainer CellStyle(IContainer container)
                                {
                                    var backgroundColor = i % 2 == 0
                                        ? Colors.Grey.Lighten2
                                        : Colors.Grey.Lighten5;

                                    return container
                                        .Background(backgroundColor)
                                        .PaddingVertical(5);
                                }
                                i++;
                            }
                        });

                        decimal grandTotal = 0;
                        foreach (var product in Controle_Liberacao_ComissaoList)
                        {
                            grandTotal += product.Valor_Comissao_Por_Parcela;
                        }

                        col.Item().PaddingVertical(30).AlignRight().Text($"Valor total: R${grandTotal:F2}").Bold().FontSize(14);
                        col.Item().AlignBottom().Text($"Assinatura do vendedor:").FontSize(14);
                        col.Item().PaddingVertical(10).AlignBottom().Text($"_______________________").FontSize(14);

                        page.Footer()
                       .AlignCenter()
                       .Text(text =>
                       {
                           text.CurrentPageNumber();
                           text.Span(" / ");
                           text.TotalPages();
                       });
                    });
            });
        }).GeneratePdf(filePath);
    }
}




// See https://aka.ms/new-console-template for more information
using Modelos.IntegradorCRM.Models.EF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var products = new List<(int, int, string, int, string, int, Decimal)>
        {
            (1, 1001, "01/02/2025", 5, "02/03/2025",3, 53.35m),
        };

List<Controle_Liberacao_ComissaoModel> comissao = new List<Controle_Liberacao_ComissaoModel>() { };

comissao.Add(new Controle_Liberacao_ComissaoModel()
{
    Id_Pedido_Venda = 1,
    numero_documento_fiscal = 2,
    data_hora_emissao_nota = Convert.ToDateTime("01/02/2025"),
    Id_Documento_Receber = 2,
    //Data_Quitacao = Convert.ToDateTime("01/02/2025"),
    DR_Total_Gerados = 3,
    Valor_Comissao_Por_Parcela = 4,
    Nome_Vendedor = "Augusto Nascimento",
});


var reportGenerator = new QuestPDFService(comissao,"Augusto", "01/03/2025", "10/03/2025");
reportGenerator.GerarPDF("ProductReport.pdf");

Console.WriteLine("PDF generated successfully!");
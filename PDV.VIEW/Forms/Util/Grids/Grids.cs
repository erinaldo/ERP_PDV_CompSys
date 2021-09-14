using DevExpress.DataProcessing;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDV.VIEW.Forms.Util
{
    public class Grids
    {

        public static string GetValorStr(GridView gridView, string fieldName, int rowHandle = -1)
        {
            //
            //Buscará um valor específico na grid recebida e no campo informado na assinatura
            //gridView: a grid em que se quer buscar um dado
            //fieldName: o nome do campo na grid que se quer buscar o vvallor
            //rowHandle: é a linha de onde se quer buscar o valor; se não for informado será a linha selecionada pelo usuário.
            //
            try
            {
                if (gridView.FocusedRowHandle < 0)
                    throw new Exception("Nenhuma linha foi selecionada");

                if (rowHandle == -1)
                    rowHandle = gridView.FocusedRowHandle;

                return gridView.GetRowCellValue(rowHandle, fieldName).ToString();
            }
            catch(Exception exception)
            {
               var msg = $"Desculpe! Não foi possível buscar o valor selecionado no campo '{fieldName}'. {exception.Message}.";
               throw new Exception(msg);
            }
            
        }

        public static int GetValorInt(GridView gridView, string fieldName, int rowHandle = -1)
        {
            try
            {
                return Convert.ToInt32(GetValorStr(gridView, fieldName, rowHandle));

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static decimal GetValorDec(GridView gridView, string fieldName, int rowHandle = -1)
        {
            try
            {
                return Convert.ToDecimal(GetValorStr(gridView, fieldName, rowHandle));
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public static void FormatGrid(ref GridView gridView, string firstColumnCaption = "ID")
        {
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                var key = gridView.Columns[i].FieldName;

                try
                {
                    var value = GetCaptionsMap().Where(m => m.Key.ToLower() == key.ToLower()).Select(m => m.Value).Single();

                    gridView.Columns[i].Caption = value.ToUpper();
                }
                catch (InvalidOperationException)
                {
                    gridView.Columns[i].Caption = key.ToUpper();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                
            }

            if (gridView.Columns.Count > 0)
            {
                gridView.Columns[0].Caption = firstColumnCaption;
                FormatColumnType(ref gridView, gridView.Columns[0].FieldName, GridFormats.Count);
            }
            gridView.OptionsBehavior.Editable = false;
            gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridView.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridView.BestFitColumns();

        }

        private static Dictionary<string, string> GetCaptionsMap()
        {
            return new Dictionary<string, string>()
            {
                { "idvenda", "id venda" },
                { "idnfe", "id nfe" },
                { "numero", "número" },
                { "dataemissao", "data de emissão" },
                { "totalnfe", "total da nfe" },
                { "idmovimentofiscal", "id mf" },
                { "xmotivo", "retorno da sefaz" },
                { "iditemvenda", "id item de venda" },
                { "descricao", "descrição" },
                { "descricaoitem", "descrição do item" },
                { "descontovalor", "valor de desconto" },
                { "valorunitarioitem", "valor unitário do item" },
                { "valortotalitem", "valor total do item" },
                { "codigoitem", "código do item" },
                { "idpedidocompra", "id compra" },
                { "valorunitario", "valor unitário" },
                { "usuario", "usuário" },
                { "datacadastro", "data" },
                { "valortotal", "total" },
                { "hora", "hora" },
                { "codigo", "código" },
                { "nacionalfederal", "nacional federal" },
                { "importadosfederal", "importados federal" },
                { "vigenciafim", "fim vigência" },
                { "vigenciainicio", "início vigência" },
                { "dataentrega", "data de entrega" },
                { "datacancelamento", "data de cancelamento" },
                { "tipodeoperacao", "operação" },
                { "tipodefrete", "tipo de frete" },
                { "controlarestoque", "controlar estoque" },
                { "gerarfinanceiro", "gerar financeiro" },
                { "limitecredito", "limite de crédito" },
                { "permiteestoquenegativo", "permite estoque negativo" },
                { "tipodemovimento", "tipo de movimento" },
                { "serie", "série" },
                { "documento", "cpf/cnpj" },
                { "numerodocumento", "Doc" },
                { "razaosocial", "razão social" },
                { "inscricaoestadual", "IE" },
                { "cod", "cód" },
                { "vendedor", "vendedor" },
                { "ean", "c. de barras" },
                { "saldoestoque", "estoque" },
                { "valorcusto", "preço de compra" },
                { "valorvenda", "preço de venda" },
                { "valorvendaprazo", "valor de venda a prazo" },
                { "bandeiracartao", "bandeira de cartão" },
                { "identificacao", "identificação" },
                { "agencia", "agência" },
                { "titulo", "título" },
                { "marcadeveiculo", "marca de veículo" },
                { "marcadeproduto", "marca de produto" },
                { "operacao", "operação" },
                { "email", "e-mail" },
                { "observacao", "observação" },
                { "datainclusao", "data de inclusão" },
                { "idusuario", "id do usuário" },
                { "transportadoraid", "id da transportadora" },
                { "transportadoranome", "nome da transportadora" },
                { "veiculoid", "id do veículo" },
                { "veiculodescricao", "descrição do veículo" },
                { "motoristaid", "id do motorista" },
                { "motoristanome", "nome do motorista" },
                { "totalitens", "total de itens" },
                { "datafaturamento",  "data de faturamento" },
                { "idromaneio", "id do romaneio" },
                { "idproduto", "id do produto" },
                { "descontoporcentagem", "porcentagem de desconto" },
                { "precovenda", "preço de venda" },
                { "codigodebarras", "código de barras" },
                { "tipodevenda", "tipo" },
                { "paravender", "para vender" },
                { "modificacao", "modificação" },
                { "situacao", "situação" },
                { "formapagamento", "forma de pagamento" },
                { "datamodificacao", "data de modificação" },
                { "horamodificacao", "hora de modificação" },
                { "emissao", "emissão" },
                { "unidadedemedidasigla", "unidade de medida(sigla)" },
                { "motivodecancelamento", "motivo de cancelamento" },
                { "idcliente", "id do cliente" },
                { "idvendedor", "id do vendedor" },
                { "caixaid", "id do caixa" },
                { "valorfechamentocaixa", "valor de fechamento"},
                { "valorcaixa", "valor de abertura" },
                { "numerocaixa", "número do caixa" },
                { "dataaberturacaixa", "data de abertura" },
                { "totalcusto", "total custo" },
                { "idcontareccobranca", "id conta receber cobrança" },
                { "valorduplicata", "valor da duplicata" },
                { "emissaotitulo", "emissão título" },
                { "vencimentotitulo", "vencimento título" },
                { "vencimentoduplicata", "vencimento duplicata" },
                { "emissaoduplicata", "emissão duplicata" },
                { "situacaotitulo", "situação título" },
                { "statusduplicata", "status duplicata" },
                { "saida", "saída" },
                { "cdebarras", "c. de barras" }
            };
        }
        public static void FormatColumnType(ref GridView gridView, string columnIndex, GridFormats format)
        {
            try
            {
                var column = gridView.Columns.Where(c => c.FieldName.ToLower() == columnIndex.ToLower()).Single();
                switch (format)
                {
                    case GridFormats.Finance:                        
                        gridView.Columns[column.FieldName].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        gridView.Columns[column.FieldName].DisplayFormat.FormatString = "c2";
                    break;
                    case GridFormats.VisibleFalse:
                        gridView.Columns[column.FieldName].Visible = false;
                    break;
                    case GridFormats.Count:
                        gridView.Columns[column.FieldName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                        gridView.Columns[column.FieldName].SummaryItem.DisplayFormat = "Registros: {0}";
                        break;
                    case GridFormats.SumFinance:
                        gridView.Columns[column.FieldName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView.Columns[column.FieldName].SummaryItem.DisplayFormat = "Total: R${0:n2}";                 
                    break;
                    case GridFormats.Sum:
                        gridView.Columns[column.FieldName].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        gridView.Columns[column.FieldName].SummaryItem.DisplayFormat = "Soma: {0}";
                    break;
                }
            }

            catch(NullReferenceException exception)
            {
                var msg = $"Pode ser que o nome do campo ('{columnIndex}'), enviado para formatação de uma coluna na grid, seja incorreto. {exception.Message}";
                throw new Exception(msg);
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static void FormatColumnType(ref GridView gridView, List<string> columnsIndexList, GridFormats format)
        {
            foreach (var index in columnsIndexList)
                FormatColumnType(ref gridView, index, format);
        }

    }
}

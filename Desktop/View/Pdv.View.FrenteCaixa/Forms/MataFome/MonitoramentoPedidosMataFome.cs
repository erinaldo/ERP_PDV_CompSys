using DevExpress.CodeParser;
using MataFome_NET.Controller;
using MataFome_NET.Model;
using ModelAndroidApp.ModelAndroid;
using PDV.CONTROLER.Funcoes;
using PDV.DAO.Custom;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.PDV;
using PDV.VIEW.App_Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sequence = PDV.DAO.DB.Utils.Sequence;

namespace PDV.VIEW.FRENTECAIXA.Forms.MataFome
{
    public static class MonitoramentoPedidosMataFome
    {
        public class Log
        {
            public DateTime DataHora { get; set; }
            public string Mensagem { get; set; }
        }
        public static class WSMonitoramento
        {
            private static BindingList<Log> _Log { get; set; }
            public static BindingList<Log> Log
            {
                get
                {
                    if (_Log == null)
                    {
                        _Log = new BindingList<Log>();
                        _Log.AllowEdit = false;
                        _Log.AllowNew = true;
                        _Log.AllowRemove = true;
                    }

                    return _Log;
                }
                set
                {
                    _Log = value;
                }
            }

            public static void AddLog(string msg)
            {
                AddLog(new Log() { DataHora = DateTime.Now, Mensagem = msg });

            }
            public static void AddLog(Log log)
            {
                try
                {
                    Log.Add(log);

                    if (Log.Count > 220)
                    {
                        Log = new BindingList<Log>(Log.ToList());
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static void RunMonitoramento()
        {

            try
            {
                WSMonitoramento.AddLog("Iniciando Atualização...");
                AtualizarAPP();
                Thread.Sleep(10000);
                WSMonitoramento.AddLog("Atualização da base do aplicativo foi atualizada com sucesso!");
                System.Threading.Tasks.Task.Run((Action)(RunMonitoramento));
            }
            catch (Exception ex)
            {
                WSMonitoramento.AddLog($"Ocorreu um erro :{ex.Message}");
            }
        }
        public static DAO.Entidades.PDV.Venda Venda = null;
        public static List<ItemVenda> lstItemDeVenda = null;
        public static List<DuplicataNFCe> lstPagamentos = null;
        public static DAO.Entidades.Cliente Cliente = null;
        public static void AtualizarAPP()
        {
            try
            {
             WSMonitoramento.AddLog("Iniciando captura de pedidos...");
             List<Orders> Pedido = PedidosController.GetOrders().ToList();
             List<OrderProducts> ItensPedidos = PedidosController.GetOrdersProduct().ToList();
              Configuracao config2 = FuncoesConfiguracao.GetConfiguracao(ChavesConfiguracao.TIPO_OPERACAO_PADRAO_APP);
                Venda = new DAO.Entidades.PDV.Venda()
                {
                    IDVenda = Sequence.GetNextID("VENDA", "IDVENDA"),
                    IDUsuario = Contexto.USUARIOLOGADO.IDUsuario,
                    DataCadastro = DateTime.Now,
                    Status = 0,
                    TipoDeVenda = 1,
                    IDFluxoCaixa = 0, //Arrumar isso
                    IDTipoDeOperacao = decimal.Parse(Encoding.UTF8.GetString(config2.Valor))
                };

                if(Pedido.Count>0)
                {
                    if(ItensPedidos.Count >0 )
                    {
                        foreach (var item in Pedido)
                        {
                            PDVControlador.BeginTransaction();
                            Venda.ValorTotal = item.OrderTotal;
                            Venda.IDCliente = null;
                            Venda.IDVendedor = Contexto.USUARIOLOGADO.IDUsuario;
                            try
                            {
                                Venda.IDFormaPagamento = 0;
                            }
                            catch (Exception)
                            {

                            }
                            if (Cliente != null)
                            {
                                Venda.IDCliente = Cliente.IDCliente;
                            }

                            //Objter fluxo de caixa PDV
                            decimal IDFluxo = FuncoesVenda.GetFluxoCaixa();
                            if (IDFluxo != null)
                            {
                                Venda.IDFluxoCaixa = IDFluxo;
                            }
                            FormaDePagamento Formapagamento = FuncoesFormaDePagamento.GetFormaDePagamento(1); //arrumar isso
                            if (string.IsNullOrEmpty(item.Observation))
                            {
                                Venda.Observacao = $"PAGAMENTO : {Formapagamento.IdentificacaoDescricao}";
                            }
                            else
                            {
                                Venda.Observacao = $"PAGAMENTO : {Formapagamento.Descricao}  | OBSERVAÇÕES : {item.Observation}";
                            }

                            if (!FuncoesVenda.SalvarVenda(Venda))
                            {
                                throw new Exception("Não foi possível salvar a Venda.");
                            }

                            if (!FuncoesItemVenda.RemoverItensDaVenda(lstItemDeVenda, Venda.IDVenda))
                            {
                                throw new Exception("Não foi possível salvar a Venda.");
                            }

                            #region Item

                            var ItensProcessado = item.OrderProducts;

                            //if(NotaItemAPP[0].ID == 302)
                            //{m

                            //}

                            //for (int j = 0; j < NotaItemAPP.Count; j++)
                            //{

                            foreach (var itemProduto in ItensProcessado)
                            {
                                decimal itemx = 1;
                                ItemVenda itemVenda = new ItemVenda()
                                {
                                    Item = itemx++,
                                    CodigoItem = itemProduto.ID.ToString(),
                                    DescricaoItem = itemProduto.Name,
                                    ValorUnitarioItem = itemProduto.Price,
                                    Subtotal = itemProduto.Price * itemProduto.Amount,
                                    IDProduto = Convert.ToDecimal(itemProduto.ExternalID.ToString()),
                                    IDItemVenda = Sequence.GetNextID("ITEMVENDA", "IDITEMVENDA"),
                                    IDVenda = Venda.IDVenda,
                                    Quantidade = Convert.ToDecimal(itemProduto.Amount),
                                    DescontoValor = itemProduto.Discount.Value,
                                    IDUsuario = Contexto.USUARIOLOGADO.IDUsuario
                                };
                                lstItemDeVenda.Add(itemVenda);
                            }
                            #endregion
                            foreach (ItemVenda Item in lstItemDeVenda)
                            {
                                if (!FuncoesItemVenda.SalvarItemVenda(Item))
                                {
                                    throw new Exception("Não foi possível salvar os Itens da Venda.");
                                }
                            }

                            //Atualizar aqui indicando que o item já foi importado

                            int dias =1;

                            DuplicataNFCe duplicataNFCe = new DuplicataNFCe()
                            {
                                IDDuplicataNFCe = Sequence.GetNextID("DUPLICATANFCE", "IDDUPLICATANFCE"),
                                IDVenda = Venda.IDVenda,
                                Valor = Convert.ToDecimal(Venda.ValorTotal),
                                DataVencimento = DateTime.Now.AddDays(dias),
                                IDFormaDePagamento = Venda.IDFormaPagamento,
                                Pagamento = 0
                            };
                            if (!FuncoesItemDuplicataNFCe.SalvarDuplicataNFCe(duplicataNFCe))
                                throw new Exception("Não foi possível salvar as Duplicatas NFCE.");


                            PDVControlador.Commit();
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

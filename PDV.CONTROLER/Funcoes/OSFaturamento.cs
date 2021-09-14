using PDV.CONTROLER.Funcoes;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Entidades.Financeiro;
using PDV.DAO.Entidades.PDV;
using PDV.DAO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDV.CONTROLER.FuncoesFaturamento
{
    public class OSFaturamento
    {
        public OrdemDeServico OrdemDeServico { get; set; }

        public List<DuplicataServico> DuplicatasServico { get; set; }

        public TipoDeOperacao TipoDeOperacao { get; set; }

        public Usuario Usuario { get; set; }
        public OSFaturamento(OrdemDeServico ordemDeServico, Usuario usuario)
        {
            PopularAtributos(ordemDeServico, usuario);
        }

        public OSFaturamento(decimal idOrdemDeServico, Usuario usuario)
        {
            PopularAtributos(FuncoesOrdemServico.GetOrdemDeServico(idOrdemDeServico), usuario);
        }

        private void PopularAtributos(OrdemDeServico ordemDeServico, Usuario usuario)
        {
            OrdemDeServico = ordemDeServico;
            Usuario = usuario;
            DuplicatasServico = FuncoesDuplicataServico.GetDuplicatasServicos(OrdemDeServico.IDOrdemDeServico);
            TipoDeOperacao = FuncoesTipoDeOperacao.GetTipoDeOperacao(OrdemDeServico.IDTipoDeOperacao);
        }

        public void FaturarOS()
        {
            if (OrdemDeServico.Status == DAO.Enum.StatusPedido.Faturado)
                return;

            if (TipoDeOperacao.GerarFinanceiro)
                GerarFinanceiro();
            OrdemDeServico.Status = DAO.Enum.StatusPedido.Faturado;
            OrdemDeServico.DataFaturamento = DateTime.Now;
            OrdemDeServico.MotivoDeCancelamento = string.Empty;
            SalvarOS("Não foi possível faturar a ordem de serviço");
        }

        private void SalvarOS(string exceptionMsg)
        {
            if (!FuncoesOrdemServico.Salvar(OrdemDeServico, TipoOperacao.UPDATE))
                throw new Exception(exceptionMsg);
        }

        private void GerarFinanceiro()
        {
            var duplicatas = FuncoesDuplicataServico.GetDuplicatasServicos(OrdemDeServico.IDOrdemDeServico);
            if (!duplicatas.Any())
                throw new Exception(
                    $"As formas de pagamento da ordem de serviço {OrdemDeServico.IDOrdemDeServico} não foram informadas" +
                    $" portanto ela não poderá ser faturada"
                    );

            InserirContasReceber(duplicatas);
        }

        private void InserirContasReceber(List<DuplicataServico> duplicatas)
        {
            foreach (var duplicata in duplicatas)
            {
                var formaDePagamento = FuncoesFormaDePagamento.GetFormaDePagamento(duplicata.IDFormaDePagamento);
                var conta = new ContaReceber
                {
                    Origem = TipoDeOperacao.Nome.Length > 30 ? TipoDeOperacao.Nome.Substring(0, 30) : TipoDeOperacao.Nome,
                    IDOrdemDeServico = OrdemDeServico.IDOrdemDeServico,
                    IDFormaDePagamento = formaDePagamento.IDFormaDePagamento,
                    Situacao = formaDePagamento.Transacao == FormaDePagamento.AVista ? 3 : 1,
                    Saldo = formaDePagamento.Transacao == FormaDePagamento.AVista ? 0 : duplicata.Valor,
                    IDContaReceber = Sequence.GetNextID("CONTARECEBER", "IDCONTARECEBER"),
                    IDContaBancaria = TipoDeOperacao.IDContaBancaria,
                    IDCentroCusto = TipoDeOperacao.IdCentroCusto,
                    IDHistoricoFinanceiro = TipoDeOperacao.IDHistoricoFinanceiro,
                    IDCliente = OrdemDeServico.IDCliente,
                    Titulo = "",
                    Emissao = DateTime.Now,
                    Vencimento = duplicata.DataVencimento,
                    Fluxo = DateTime.Now,
                    ComplmHisFin = $"Ordem de Serviço Codigo: {OrdemDeServico.IDOrdemDeServico}.",
                    Valor = duplicata.Valor,
                    ValorTotal = duplicata.Valor,
                    IDUsuario = Usuario.IDUsuario
                };


                if (!FuncoesContaReceber.Salvar(conta, TipoOperacao.INSERT))
                    throw new Exception($"Não foi possível salvar o lançamento na ordem {OrdemDeServico.IDOrdemDeServico}");

                if (formaDePagamento.Transacao == FormaDePagamento.AVista)
                    GerarBaixa(conta);
            }

        }

        private void GerarBaixa(ContaReceber conta)
        {
            var baixaRecebimento = new BaixaRecebimento
            {
                IDContaReceber = conta.IDContaReceber,
                IDBaixaRecebimento = Sequence.GetNextID("BAIXARECEBIMENTO", "IDBAIXARECEBIMENTO"),
                Valor = conta.ValorTotal,
                IDFormaDePagamento = Convert.ToDecimal(conta.IDFormaDePagamento),
                IDHistoricoFinanceiro = TipoDeOperacao.IDHistoricoFinanceiro,
                IDContaBancaria = Convert.ToDecimal(conta.IDContaBancaria),
                Baixa = DateTime.Now
            };
            if (!FuncoesBaixaRecebimento.Salvar(baixaRecebimento, TipoOperacao.INSERT))
                throw new Exception($"Não foi possível salvar a Baixa na ordem {OrdemDeServico.IDOrdemDeServico}.");
        }

        public void CancelarOs(string motivoCancelamento)
        {
            if (OrdemDeServico.Status != DAO.Enum.StatusPedido.Faturado)
                return;
            if (TipoDeOperacao.GerarFinanceiro)
                CancelarFinanceiro();
            OrdemDeServico.Status = DAO.Enum.StatusPedido.Cancelado;
            OrdemDeServico.DataFaturamento = null;
            OrdemDeServico.MotivoDeCancelamento = motivoCancelamento;
            SalvarOS("Não foi possível salvar a ordem de serviço");
        }

        private void CancelarFinanceiro()
        {
            FuncoesContaReceber
                .CancelarContaReceberPorIDOrdemDeServico(OrdemDeServico.IDOrdemDeServico);
        }
    }
}

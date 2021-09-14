using PDV.DAO.Custom;
using PDV.DAO.DB.Controller;
using PDV.DAO.DB.Utils;
using PDV.DAO.Entidades;
using PDV.DAO.Enum;
using PDV.DAO.GridViewModels;
using System;
using System.Collections.Generic;
using System.Data;

namespace PDV.CONTROLER.Funcoes
{
    public static class FuncoesOrdemServico
    {
        public static List<OrdemDeServicoGridViewModel> GetOrdensDeServico(DateTime dataDe, DateTime dataAte)
        {
            using(SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT 
                                IDORDEMDESERVICO,
                                DATACADASTRO,
                                DATAFATURAMENTO,
                                VALORTOTAL,
                                CASE 
                                    WHEN STATUS = @FATURADO THEN 'FATURADO'
                                    WHEN STATUS = @ABERTO THEN 'ABERTO'
                                    WHEN STATUS = @CANCELADO THEN 'CANCELADO'
                                END AS STATUS,
                                COALESCE(CLIENTE.NOME, CLIENTE.RAZAOSOCIAL, CLIENTE.NOMEFANTASIA) AS CLIENTE
                            FROM ORDEMDESERVICO
                            JOIN CLIENTE ON CLIENTE.IDCLIENTE = ORDEMDESERVICO.IDCLIENTE
                            WHERE DATACADASTRO BETWEEN @DATADE AND @DATAATE
                            ORDER BY IDORDEMDESERVICO DESC";
                oSQL.ParamByName["DATADE"] = dataDe;
                oSQL.ParamByName["DATAATE"] = dataAte;
                oSQL.ParamByName["FATURADO"] = Status.Faturado;
                oSQL.ParamByName["ABERTO"] = Status.Aberto;
                oSQL.ParamByName["CANCELADO"] = Status.Cancelado;
                oSQL.Open();
                return new DataTableParser<OrdemDeServicoGridViewModel>().ParseDataTable(oSQL.dtDados);
            }
        }

        public static DataTable GetOrdemDeServicoModelo2(decimal idOs)
        {
            using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = @"SELECT 
                            OS.IDORDEMDESERVICO AS CODIGO,
                            OS.DATACADASTRO AS DATA ,
                            U.NOME AS VENDEDOR,
                            CASE
								WHEN CLI.NOME NOTNULL THEN CLI.NOME
								WHEN CLI.NOME ISNULL THEN CLI.NOMEFANTASIA
								WHEN CLI.NOMEFANTASIA ISNULL AND CLI.NOME ISNULL THEN CLI.RAZAOSOCIAL
								
							END AS CLIENTE, 
                            CASE WHEN CLI.TIPODOCUMENTO = 0 THEN CLI.CNPJ ELSE CLI.CPF END AS DOCUMENTO,
                            CASE WHEN CON.TELEFONE ISNULL THEN CON.CELULAR ELSE CON.TELEFONE END AS TELEFONE,
                            E.LOGRADOURO|| ', ' || E.NUMERO || ', ' ||  E.CEP  || ', ' ||  E.BAIRRO || ', ' ||  MC.DESCRICAO || '- ' ||  UF.SIGLA AS ENDERECO,
                            S.IDSERVICO AS CODIGOSERVICO, 
                            IOS.DESCRICAO AS SERVICO,
                            IOS.QUANTIDADE AS QUANTIDADE,
                            UN.SIGLA AS UN,
                            IOS.VALORUNITARIOITEM AS VALOR,
                            IOS.DESCONTOVALOR AS DESCONTO,
                            OS.VALORTOTAL AS VALORTOTAL,
                            OS.OBSERVACAO AS OBSERVACAO
                            FROM ORDEMDESERVICO OS
                            LEFT JOIN ITEMORDEMDESERVICO IOS ON IOS.IDORDEMDESERVICO = OS.IDORDEMDESERVICO
                            LEFT JOIN SERVICO S ON S.IDSERVICO = IOS.IDSERVICO
                            LEFT JOIN UNIDADEDEMEDIDA UN ON UN.IDUNIDADEDEMEDIDA = S.IDUNIDADEDEMEDIDA
                            LEFT JOIN CLIENTE CLI ON CLI.IDCLIENTE = OS.IDCLIENTE
                            LEFT JOIN USUARIO U ON U.IDUSUARIO = OS.IDVENDEDOR
                            LEFT JOIN ENDERECO E ON E.IDENDERECO = CLI.IDENDERECO
                            LEFT JOIN MUNICIPIO MC ON MC.IDMUNICIPIO = E.IDMUNICIPIO
                            LEFT JOIN UNIDADEFEDERATIVA UF ON UF.IDUNIDADEFEDERATIVA = MC.IDUNIDADEFEDERATIVA
							LEFT JOIN CONTATO CON ON CON.IDCONTATO = CLI.IDCONTATO
                           WHERE OS.IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOs;
                oSQL.Open();
                return oSQL.dtDados;
            }
        }

        public static OrdemDeServico GetOrdemDeServico(decimal idOrdemDeServico)
        {
            using(SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "SELECT * FROM ORDEMDESERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                oSQL.Open();
                return new DataTableParser<OrdemDeServico>().ParseDataRow(oSQL.dtDados.Rows[0]);
            }
        }



        public static bool Salvar(OrdemDeServico ordemDeServico, TipoOperacao tipoOperacao)
        {
            using(SQLQuery oSQL = new SQLQuery())
            {
                if (tipoOperacao == TipoOperacao.INSERT)
                {
                    oSQL.SQL = @"INSERT INTO ORDEMDESERVICO
                                    (IDORDEMDESERVICO, IDCLIENTE, VALORTOTAL, DATACADASTRO, DINHEIRO,
                                    TROCO, STATUS, OBSERVACAO, IDVENDEDOR, IDTIPODEOPERACAO,
                                    MOTIVODECANCELAMENTO, DATAFATURAMENTO, IDROMANEIO, ROMANEIO)
                                VALUES
                                    (@IDORDEMDESERVICO, @IDCLIENTE, @VALORTOTAL, @DATACADASTRO, @DINHEIRO,
                                    @TROCO, @STATUS, @OBSERVACAO, @IDVENDEDOR, @IDTIPODEOPERACAO,
                                    @MOTIVODECANCELAMENTO, @DATAFATURAMENTO, @IDROMANEIO, @ROMANEIO)
                                ";
                }
                else if (tipoOperacao == TipoOperacao.UPDATE)
                {
                    oSQL.SQL = @"UPDATE ORDEMDESERVICO SET
                                    IDCLIENTE = @IDCLIENTE, 
                                    VALORTOTAL = @VALORTOTAL, 
                                    DATACADASTRO = @DATACADASTRO, 
                                    DINHEIRO = @DINHEIRO,
                                    TROCO = @TROCO, 
                                    STATUS = @STATUS, 
                                    OBSERVACAO = @OBSERVACAO, 
                                    IDVENDEDOR = @IDVENDEDOR,
                                    IDTIPODEOPERACAO = @IDTIPODEOPERACAO,
                                    MOTIVODECANCELAMENTO = @MOTIVODECANCELAMENTO, 
                                    DATAFATURAMENTO = @DATAFATURAMENTO, 
                                    IDROMANEIO = @IDROMANEIO, 
                                    ROMANEIO = @ROMANEIO
                                WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                   
                }

                oSQL.ParamByName["IDORDEMDESERVICO"] = ordemDeServico.IDOrdemDeServico;
                oSQL.ParamByName["IDCLIENTE"] = ordemDeServico.IDCliente;
                oSQL.ParamByName["VALORTOTAL"] = ordemDeServico.ValorTotal;
                oSQL.ParamByName["DATACADASTRO"] = ordemDeServico.DataCadastro;
                oSQL.ParamByName["DINHEIRO"] = ordemDeServico.Dinheiro;
                oSQL.ParamByName["TROCO"] = ordemDeServico.Troco;
                oSQL.ParamByName["STATUS"] = ordemDeServico.Status;
                oSQL.ParamByName["OBSERVACAO"] = ordemDeServico.Observacao;
                oSQL.ParamByName["IDVENDEDOR"] = ordemDeServico.IDVendedor;
                oSQL.ParamByName["IDTIPODEOPERACAO"] = ordemDeServico.IDTipoDeOperacao;
                oSQL.ParamByName["MOTIVODECANCELAMENTO"] = ordemDeServico.MotivoDeCancelamento;
                oSQL.ParamByName["DATAFATURAMENTO"] = ordemDeServico.DataFaturamento;
                oSQL.ParamByName["IDROMANEIO"] = ordemDeServico.IDRomaneio;
                oSQL.ParamByName["ROMANEIO"] = ordemDeServico.Romaneio;
                return oSQL.ExecSQL() == 1;
            }
        }

        public static bool Remover(decimal idOrdemDeServico)
        {
           using (SQLQuery oSQL = new SQLQuery())
            {
                oSQL.SQL = "DELETE FROM ORDEMDESERVICO WHERE IDORDEMDESERVICO = @IDORDEMDESERVICO";
                oSQL.ParamByName["IDORDEMDESERVICO"] = idOrdemDeServico;
                return oSQL.ExecSQL() == 1;
            }
        }
    }
}

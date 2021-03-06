/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/
using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.Classes.Servicos.ConsultaCadastro
{
    /// <summary>
    ///     Situação do contribuinte:
    ///     <para>0 - Não habilitado;</para>
    ///     <para>1 - Habilitado.</para>
    /// </summary>
    public enum SituacaoContribuinte
    {
        /// <summary>
        /// 0 - Não habilitado
        /// </summary>
        [Description("Não habilitado")]
        [XmlEnum("0")]
        NaoHabilitado = 0,

        /// <summary>
        /// 1 - Habilitado
        /// </summary>
        [Description("Habilitado")]
        [XmlEnum("1")]
        Habilitado = 1
    }

    /// <summary>
    ///     Indicador de contribuinte credenciado a emitir NF-e.
    ///     <para>0 - Não credenciado para emissão da NF-e;</para>
    ///     <para>1 - Credenciado;</para>
    ///     <para>2 - Credenciado com obrigatoriedade para todas operações;</para>
    ///     <para>3 - Credenciado com obrigatoriedade parcial;</para>
    ///     <para>4 – A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir NF-e na SEFAZ consultada.</para>
    /// </summary>
    public enum IndicadorCredenciamentoNfe
    {
        /// <summary>
        /// 0 - Não credenciado para emissão da NF-e
        /// </summary>
        [Description("Não credenciado para emissão da NF-e")]
        [XmlEnum("0")]
        NaoCredenciado = 0,

        /// <summary>
        /// 1 - Credenciado
        /// </summary>
        [Description("Credenciado")]
        [XmlEnum("1")]
        Credenciado = 1,

        /// <summary>
        /// 2 - Credenciado com obrigatoriedade para todas operações
        /// </summary>
        [Description("Credenciado com obrigatoriedade para todas operações")]
        [XmlEnum("2")]
        CredenciadoTodasOperacoes = 2,

        /// <summary>
        /// 3 - Credenciado com obrigatoriedade parcial
        /// </summary>
        [Description("Credenciado com obrigatoriedade parcial")]
        [XmlEnum("3")]
        CredenciadoParcial = 3,

        /// <summary>
        /// 4 – A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir NF-e na SEFAZ consultada
        /// </summary>
        [Description("A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir NF-e na SEFAZ consultada")]
        [XmlEnum("4")]
        SemInformacaoSefaz = 4
    }

    /// <summary>
    ///     Indicador de contribuinte credenciado a emitir CT-e.
    ///     <para>0 - Não credenciado para emissão da CT-e;</para>
    ///     <para>1 - Credenciado;</para>
    ///     <para>2 - Credenciado com obrigatoriedade para todas operações;</para>
    ///     <para>3 - Credenciado com obrigatoriedade parcial;</para>
    ///     <para>4 – A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir CT-e na SEFAZ consultada.</para>
    /// </summary>
    public enum IndicadorCredenciamentoCte
    {
        /// <summary>
        /// 0 - Não credenciado para emissão da CT-e
        /// </summary>
        [Description("Não credenciado para emissão da CT-e")]
        [XmlEnum("0")] NaoCredenciado = 0,

        /// <summary>
        /// 1 - Credenciado
        /// </summary>
        [Description("Credenciado")]
        [XmlEnum("1")] Credenciado = 1,

        /// <summary>
        /// 2 - Credenciado com obrigatoriedade para todas operações
        /// </summary>
        [Description("Credenciado com obrigatoriedade para todas operações")]
        [XmlEnum("2")] CredenciadoTodasOperacoes = 2,

        /// <summary>
        /// 3 - Credenciado com obrigatoriedade parcial
        /// </summary>
        [Description("Credenciado com obrigatoriedade parcial")]
        [XmlEnum("3")] CredenciadoParcial = 3,

        /// <summary>
        /// 4 – A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir CT-e na SEFAZ consultada
        /// </summary>
        [Description("A SEFAZ não fornece a informação. Este indicador significa apenas que o contribuinte é credenciado para emitir CT-e na SEFAZ consultada")]
        [XmlEnum("4")] SemInformacaoSefaz = 4
    }

    /// <summary>
    ///     Tipo de documento a ser utilizado na consulta de cadastro.
    ///     <para>Ie - Inscrição Estadual</para>
    ///     <para>Cnpj - CNPJ</para>
    ///     <para>Cpf - CPF</para>
    /// </summary>
    public enum ConsultaCadastroTipoDocumento
    {
        Ie = 0,
        Cnpj = 1,
        Cpf = 2
    }
}
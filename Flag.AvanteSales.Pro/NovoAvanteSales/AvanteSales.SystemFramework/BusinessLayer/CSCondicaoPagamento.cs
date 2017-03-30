using System;
using System.Collections;
using System.Data;
#if ANDROID
using Mono.Data.Sqlite;
#else
using System.Data.SQLite;
using System.Windows.Forms;
using System.Drawing;
#endif

#if ANDROID
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using SQLiteDataAdapter = Mono.Data.Sqlite.SqliteDataAdapter;
using SQLiteException = Mono.Data.Sqlite.SqliteException;
using SQLiteParameter = Mono.Data.Sqlite.SqliteParameter;
using SQLiteTransaction = Mono.Data.Sqlite.SqliteTransaction;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using System.Text;
#endif

namespace AvanteSales
{
    public class CSCondicoesPagamento : CollectionBase
    {
        #region [ Variaveis ]

        private static CSCondicoesPagamento m_Items;

        public struct TipoMensagemAbaixoMinimo
        {
            public const string ADVERTENCIA = "A";
            public const string BLOQUEIO = "B";
            public const string ERRO = "E";
        }

        #endregion

        #region [ Propriedades ]

        /// <summary>
        /// Retorna coleção das operações
        /// </summary>
        public static CSCondicoesPagamento Items
        {
            get
            {
                m_Items = new CSCondicoesPagamento();
                return m_Items;
            }
        }

        public CSCondicoesPagamento.CSCondicaoPagamento this[int Index]
        {
            get
            {
                return (CSCondicoesPagamento.CSCondicaoPagamento)this.InnerList[Index];
            }
        }

        #endregion

        #region [ Metodos ]

        /// <summary>
        /// Contrutor da classe. Busca as condiçoes de pagamento
        /// </summary>
        public CSCondicoesPagamento()
        {
            try
            {
                StringBuilder sqlQuery = new StringBuilder();
                SQLiteDataReader sqlReader;

                // Se for Broker...
                if (CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA == 2)
                {
                    sqlQuery.AppendLine(" SELECT T1.COD_TIPO_CONDICAO_PAGAMENTO, T1.COD_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("       ,T1.PRC_ADICIONAL_FINANCEIRO, T1.DSC_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("       ,T1.PRIORIDADE_CONDICAO_PAGAMENTO, T1.IND_ATIVO ");
                    sqlQuery.AppendLine("       ,T1.VLR_MINIMO_PEDIDO, T1.TPO_MENSAGEM_ABAIXO_MINIMO, T1.IND_PAGAMENTO_ANTECIPADO ");
                    sqlQuery.AppendLine("       ,0 AS CODPRZCLIENTE, T1.CODIGO_PRAZO AS CODPRZPGT ");
                    sqlQuery.AppendLine("       ,0 AS DIACSTFIN, 0 AS DIAVCM ");
                    sqlQuery.AppendLine("   FROM CONDICAO_PAGAMENTO T1 ");
                    sqlQuery.AppendLine("  WHERE T1.COD_TIPO_CONDICAO_PAGAMENTO = 1 ");
                    sqlQuery.AppendLine("  UNION ");
                    sqlQuery.AppendLine(" SELECT DISTINCT T1.COD_TIPO_CONDICAO_PAGAMENTO, T1.COD_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("       ,T1.PRC_ADICIONAL_FINANCEIRO, T1.DSC_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("       ,T1.PRIORIDADE_CONDICAO_PAGAMENTO, T1.IND_ATIVO ");
                    sqlQuery.AppendLine("       ,T1.VLR_MINIMO_PEDIDO, T1.TPO_MENSAGEM_ABAIXO_MINIMO, T1.IND_PAGAMENTO_ANTECIPADO ");
                    // "      ,T2.COD_CONDICAO_PAGAMENTO AS CODPRZCLIENTE " +
                    sqlQuery.AppendLine("       ,1 AS CODPRZCLIENTE ");
                    sqlQuery.AppendLine("       ,T1.CODIGO_PRAZO AS CODPRZPGT, 0 AS DIACSTFIN, 0 AS DIAVCM ");
                    sqlQuery.AppendLine("   FROM CONDICAO_PAGAMENTO T1 ");
                    sqlQuery.AppendLine("   JOIN PDV_GRUPO_COMERCIALIZACAO T2 ");
                    sqlQuery.AppendLine("        ON T2.COD_PDV = ? ");
                    sqlQuery.AppendLine("   JOIN CONDICAO_PAGAMENTO T3 ");
                    sqlQuery.AppendLine("        ON T3.COD_CONDICAO_PAGAMENTO = T2.COD_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("  WHERE T1.COD_TIPO_CONDICAO_PAGAMENTO <> 1 ");
                    sqlQuery.AppendLine("    AND T1.PRIORIDADE_CONDICAO_PAGAMENTO <= T3.PRIORIDADE_CONDICAO_PAGAMENTO ");

                    if (CSEmpresa.Current.IND_UTILIZA_PRIORIDADE_TIPO_DOCUMENTO &&
                        CSPDVs.Current != null &&
                        CSEmpresa.ColunaExiste("CONDICAO_PAGAMENTO", "COD_TIPO_DOCUMENTO_COBRANCA"))
                    {
                        sqlQuery.AppendFormat(" AND (T1.COD_TIPO_DOCUMENTO_COBRANCA IS NULL OR T1.COD_TIPO_DOCUMENTO_COBRANCA = {0})", CSPDVs.Current.CONDICAO_PAGAMENTO.COD_TIPO_DOCUMENTO_COBRANCA);
                    }

                    sqlQuery.AppendLine("  ORDER BY T1.PRIORIDADE_CONDICAO_PAGAMENTO, T1.COD_CONDICAO_PAGAMENTO ");

                    // Cria os parametros
                    SQLiteParameter pCOD_PDV = new SQLiteParameter("@COD_PDV", CSPDVs.Current.COD_PDV);
                    sqlReader = CSDataAccess.Instance.ExecuteReader(sqlQuery.ToString(), pCOD_PDV);
                }
                else
                {
                    sqlQuery.AppendLine("SELECT COD_TIPO_CONDICAO_PAGAMENTO, COD_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("      ,PRC_ADICIONAL_FINANCEIRO, DSC_CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine("      ,PRIORIDADE_CONDICAO_PAGAMENTO, IND_ATIVO ");
                    sqlQuery.AppendLine("      ,VLR_MINIMO_PEDIDO, TPO_MENSAGEM_ABAIXO_MINIMO, IND_PAGAMENTO_ANTECIPADO ");

                    if (CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA == 3)
                        sqlQuery.AppendLine(",CODIGO_PRAZO_BUNGE");

                    sqlQuery.AppendLine("  FROM CONDICAO_PAGAMENTO ");
                    sqlQuery.AppendLine(" WHERE CODIGO_PRAZO IS NULL ");

                    if ((CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA == 1) &&
                        CSEmpresa.ColunaExiste("CONDICAO_PAGAMENTO", "CODIGO_PRAZO_BUNGE"))
                        sqlQuery.AppendLine("AND CODIGO_PRAZO_BUNGE IS NULL ");

                    if (CSEmpresa.Current.IND_UTILIZA_PRIORIDADE_TIPO_DOCUMENTO &&
                       CSPDVs.Current != null &&
                       CSEmpresa.ColunaExiste("CONDICAO_PAGAMENTO", "COD_TIPO_DOCUMENTO_COBRANCA"))
                    {
                        sqlQuery.AppendFormat(" AND (CONDICAO_PAGAMENTO.COD_TIPO_DOCUMENTO_COBRANCA IS NULL OR CONDICAO_PAGAMENTO.COD_TIPO_DOCUMENTO_COBRANCA = {0})", CSPDVs.Current.CONDICAO_PAGAMENTO.COD_TIPO_DOCUMENTO_COBRANCA);
                    }

                    sqlQuery.AppendLine(" ORDER BY PRIORIDADE_CONDICAO_PAGAMENTO, COD_CONDICAO_PAGAMENTO");

                    // Busca todas as condicoes de pagamento
                    sqlReader = CSDataAccess.Instance.ExecuteReader(sqlQuery.ToString());
                }

                while (sqlReader.Read())
                {
                    // Preenche a instancia da classe de condicoes
                    CSCondicaoPagamento condicao = new CSCondicaoPagamento();
                    // TODO: Mudar para buscar o objeto da condicao de pagamento
                    condicao.COD_TIPO_CONDICAO_PAGAMENTO = sqlReader.GetValue(0) == System.DBNull.Value ? -1 : sqlReader.GetInt32(0);
                    condicao.COD_CONDICAO_PAGAMENTO = sqlReader.GetValue(1) == System.DBNull.Value ? -1 : sqlReader.GetInt32(1);
                    condicao.PRC_ADICIONAL_FINANCEIRO = sqlReader.GetValue(2) == System.DBNull.Value ? -1 : Convert.ToDecimal(sqlReader.GetValue(2));
                    condicao.DSC_CONDICAO_PAGAMENTO = sqlReader.GetValue(3) == System.DBNull.Value ? "" : sqlReader.GetString(3);
                    condicao.PRIORIDADE_CONDICAO_PAGAMENTO = sqlReader.GetValue(4) == System.DBNull.Value ? -1 : sqlReader.GetInt32(4);
                    condicao.IND_ATIVO = sqlReader.GetValue(5) == System.DBNull.Value ? false : sqlReader.GetBoolean(5);
                    condicao.VLR_MINIMO_PEDIDO = sqlReader.GetValue(6) == System.DBNull.Value ? 0 : Convert.ToDecimal(sqlReader.GetValue(6));
                    condicao.TPO_MENSAGEM_ABAIXO_MINIMO = sqlReader.GetValue(7) == System.DBNull.Value ? TipoMensagemAbaixoMinimo.ADVERTENCIA : sqlReader.GetString(7).ToUpper();
                    condicao.IND_PAGAMENTO_ANTECIPADO = sqlReader.GetValue(8) == System.DBNull.Value ? false : sqlReader.GetBoolean(8);

                    // Somente preenche se for broker...
                    if (CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA == 2)
                    {
                        condicao.CODPRZCLIENTE = sqlReader.GetValue(9) == System.DBNull.Value ? 0 : sqlReader.GetInt32(9);
                        condicao.CODPRZPGT = sqlReader.GetValue(10) == System.DBNull.Value ? "" : sqlReader.GetString(10);
                        condicao.DIACSTFIN = sqlReader.GetValue(11) == System.DBNull.Value ? 0 : sqlReader.GetInt32(11);
                        condicao.DIAVCM = sqlReader.GetValue(12) == System.DBNull.Value ? 0 : sqlReader.GetInt32(12);

                        // Devido ao problema no Broker de aracaju,a condicao de pagamento a prazo estava trazendo
                        // % de ADF, sendo que isso nao pode acontecer, por isso estamos setando o valor 0 na variavel.
                        condicao.PRC_ADICIONAL_FINANCEIRO = 0;
                    }
                    else if (CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA == 3)
                        condicao.CODIGO_PRAZO_BUNGE = sqlReader.GetValue(9) == System.DBNull.Value ? "" : sqlReader.GetString(9);

                    // Adciona a operação na coleção de operações
                    base.InnerList.Add(condicao);
                }

                // Fecha o reader
                sqlReader.Close();
                sqlReader.Dispose();

            }
            catch (Exception ex)
            {
                CSGlobal.ShowMessage(ex.ToString());
                throw new Exception("Erro na busca das operações", ex);
            }
        }

        /// <summary>
        /// Busca a condição de pagamento pelo codigo
        /// </summary>
        /// <param name="COD_OPERACAO">Codigo da operacao a ser procurada</param>
        /// <returns>Retorna o objeto da operação</returns>
        public static CSCondicaoPagamento GetCondicaPagamento(int COD_CONDICAO_PAGAMENTO)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("SELECT COD_TIPO_CONDICAO_PAGAMENTO, COD_CONDICAO_PAGAMENTO, PRC_ADICIONAL_FINANCEIRO, DSC_CONDICAO_PAGAMENTO, PRIORIDADE_CONDICAO_PAGAMENTO, IND_ATIVO, VLR_MINIMO_PEDIDO, TPO_MENSAGEM_ABAIXO_MINIMO, IND_PAGAMENTO_ANTECIPADO, CODIGO_PRAZO ");

            if (CSEmpresa.ColunaExiste("CONDICAO_PAGAMENTO", "COD_TIPO_DOCUMENTO_COBRANCA"))
                sqlQuery.AppendLine(", COD_TIPO_DOCUMENTO_COBRANCA ");
            else
                sqlQuery.AppendLine(", NULL AS CODTIPO_DOCUMENTOCOBRANCA ");

            sqlQuery.AppendLine(" FROM CONDICAO_PAGAMENTO WHERE COD_CONDICAO_PAGAMENTO=?");

            CSCondicaoPagamento condicao = new CSCondicaoPagamento();

            SQLiteParameter pCOD_CONDICAO_PAGAMENTO = new SQLiteParameter("@COD_CONDICAO_PAGAMENTO", COD_CONDICAO_PAGAMENTO);

            // Busca a condicao de pagamento do pdv
            using (SQLiteDataReader sqlReader = CSDataAccess.Instance.ExecuteReader(sqlQuery.ToString(), pCOD_CONDICAO_PAGAMENTO))
            {
                while (sqlReader.Read())
                {
                    condicao.COD_TIPO_CONDICAO_PAGAMENTO = sqlReader.GetValue(0) == System.DBNull.Value ? -1 : sqlReader.GetInt32(0);
                    condicao.COD_CONDICAO_PAGAMENTO = sqlReader.GetValue(1) == System.DBNull.Value ? -1 : sqlReader.GetInt32(1);
                    condicao.PRC_ADICIONAL_FINANCEIRO = sqlReader.GetValue(2) == System.DBNull.Value ? -1 : Convert.ToDecimal(sqlReader.GetValue(2));
                    condicao.DSC_CONDICAO_PAGAMENTO = sqlReader.GetValue(3) == System.DBNull.Value ? "" : sqlReader.GetString(3);
                    condicao.PRIORIDADE_CONDICAO_PAGAMENTO = sqlReader.GetValue(4) == System.DBNull.Value ? -1 : sqlReader.GetInt32(4);
                    condicao.IND_ATIVO = sqlReader.GetValue(5) == System.DBNull.Value ? false : sqlReader.GetBoolean(5);
                    condicao.VLR_MINIMO_PEDIDO = sqlReader.GetValue(6) == System.DBNull.Value ? 0 : Convert.ToDecimal(sqlReader.GetValue(6));
                    condicao.TPO_MENSAGEM_ABAIXO_MINIMO = sqlReader.GetValue(7) == System.DBNull.Value ? TipoMensagemAbaixoMinimo.ADVERTENCIA : sqlReader.GetString(7);
                    condicao.IND_PAGAMENTO_ANTECIPADO = sqlReader.GetValue(8) == System.DBNull.Value ? false : sqlReader.GetBoolean(8);
                    condicao.CODPRZPGT = sqlReader.GetValue(9) == System.DBNull.Value ? string.Empty : sqlReader.GetString(9);
                    condicao.COD_TIPO_DOCUMENTO_COBRANCA = sqlReader.GetValue(10) == System.DBNull.Value ? 0 : sqlReader.GetInt32(10);
                }

                sqlReader.Close();
                sqlReader.Dispose();
            }

            // retorna o objeto da condição de pagamento
            return condicao;
        }

        #endregion

        #region [ SubClasses ]

        /// <summary>
        /// Guarda a informaçào sobre condição de pagamento
        /// </summary>
        public class CSCondicaoPagamento
        {
            #region [ Variaveis ]

            private int m_COD_TIPO_CONDICAO_PAGAMENTO;
            private int m_COD_CONDICAO_PAGAMENTO;
            private decimal m_PRC_ADICIONAL_FINANCEIRO;
            private string m_DSC_CONDICAO_PAGAMENTO;
            private int m_PRIORIDADE_CONDICAO_PAGAMENTO;
            private bool m_IND_ATIVO;
            private decimal m_VLR_MINIMO_PEDIDO;
            private string m_TPO_MENSAGEM_ABAIXO_MINIMO;
            private int m_CODPRZCLIENTE;
            private string m_CODPRZPGT;
            private int m_DIACSTFIN;
            private int m_DIAVCM;
            private bool m_IND_PAGAMENTO_ANTECIPADO;
            private string m_CODIGO_PRAZO_BUNGE;
            private int m_COD_TIPO_DOCUMENTO_COBRANCA;

            #endregion

            #region [ Propriedades ]

            /// <summary>
            /// Guarda o objeto da condicao da condicao de pagamento
            /// </summary>
            public int COD_TIPO_CONDICAO_PAGAMENTO
            {
                get
                {
                    return m_COD_TIPO_CONDICAO_PAGAMENTO;
                }
                set
                {
                    m_COD_TIPO_CONDICAO_PAGAMENTO = value;
                }
            }

            /// <summary>
            /// Guarda o codigo da condicao de pagamento
            /// </summary>
            public int COD_CONDICAO_PAGAMENTO
            {
                get
                {
                    return m_COD_CONDICAO_PAGAMENTO;
                }
                set
                {
                    m_COD_CONDICAO_PAGAMENTO = value;
                }
            }

            /// <summary>
            /// Guarda o adicional financeiro
            /// </summary>
            public decimal PRC_ADICIONAL_FINANCEIRO
            {
                get
                {
                    return m_PRC_ADICIONAL_FINANCEIRO;
                }
                set
                {
                    m_PRC_ADICIONAL_FINANCEIRO = value;
                }
            }

            /// <summary>
            /// Guarda a descrição da condição de pagamento
            /// </summary>
            public string DSC_CONDICAO_PAGAMENTO
            {
                get
                {
                    return m_DSC_CONDICAO_PAGAMENTO;
                }
                set
                {
                    m_DSC_CONDICAO_PAGAMENTO = value;
                }
            }

            public int PRIORIDADE_CONDICAO_PAGAMENTO
            {
                get
                {
                    return m_PRIORIDADE_CONDICAO_PAGAMENTO;
                }
                set
                {
                    m_PRIORIDADE_CONDICAO_PAGAMENTO = value;
                }
            }

            public bool IND_ATIVO
            {
                get
                {
                    return m_IND_ATIVO;
                }
                set
                {
                    m_IND_ATIVO = value;
                }
            }

            public decimal VLR_MINIMO_PEDIDO
            {
                get
                {
                    return m_VLR_MINIMO_PEDIDO;
                }
                set
                {
                    m_VLR_MINIMO_PEDIDO = value;
                }
            }

            public string TPO_MENSAGEM_ABAIXO_MINIMO
            {
                get
                {
                    return m_TPO_MENSAGEM_ABAIXO_MINIMO;
                }
                set
                {
                    m_TPO_MENSAGEM_ABAIXO_MINIMO = value;
                }
            }

            public int CODPRZCLIENTE
            {
                get
                {
                    return m_CODPRZCLIENTE;
                }
                set
                {
                    m_CODPRZCLIENTE = value;
                }
            }

            public string CODPRZPGT
            {
                get
                {
                    return m_CODPRZPGT;
                }
                set
                {
                    m_CODPRZPGT = value;
                }
            }

            public int DIACSTFIN
            {
                get
                {
                    return m_DIACSTFIN;
                }
                set
                {
                    m_DIACSTFIN = value;
                }
            }

            public int DIAVCM
            {
                get
                {
                    return m_DIAVCM;
                }
                set
                {
                    m_DIAVCM = value;
                }
            }
            public bool IND_PAGAMENTO_ANTECIPADO
            {
                get
                {
                    return m_IND_PAGAMENTO_ANTECIPADO;
                }
                set
                {
                    m_IND_PAGAMENTO_ANTECIPADO = value;
                }
            }
            public string CODIGO_PRAZO_BUNGE
            {
                get
                {
                    return m_CODIGO_PRAZO_BUNGE;
                }
                set
                {
                    m_CODIGO_PRAZO_BUNGE = value;
                }
            }

            public int COD_TIPO_DOCUMENTO_COBRANCA
            {
                get
                {
                    return m_COD_TIPO_DOCUMENTO_COBRANCA;
                }
                set
                {
                    m_COD_TIPO_DOCUMENTO_COBRANCA = value;
                }
            }
            #endregion

            #region [ Metodos ]

            public CSCondicaoPagamento()
            {
            }

            #endregion
        }

        #endregion
    }
}
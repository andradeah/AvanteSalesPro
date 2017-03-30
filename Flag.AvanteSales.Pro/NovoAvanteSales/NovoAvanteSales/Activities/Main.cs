using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AvanteSales.Pro.Dialogs;
using AvanteSales.Pro.Formatters;
using Java.Lang;
using Master.CompactFramework.Sync;
using Master.CompactFramework.Sync.SQLLiteProvider;
using Android.Util;

namespace AvanteSales.Pro.Activities
{
    [Activity(Label = "Main", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AvanteSalesTheme2")]
    public class Main : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar tbToolbar;
        TextView lblVendedor;
        TextView lblData;
        //TextView lblInformativo;
        //Button btnListaCliente;
        //Button btnSair;
        //Button btnListaRelatorio;
        //Button btnSobre;
        //Button btnExpediente;
        //Button btnDescarga;
        static ProgressDialog progress;
        static AppCompatActivity CurrentActivity;
        private static ISyncProvider syncProvider;
        static string VersaoAvante;
        GridView grdMenu;
        bool UtilizaExpediente;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main);

            FindViewsById();

            Eventos();

            SetSupportActionBar(tbToolbar);

            Inicializacao();
        }

        public override void OnBackPressed()
        {
            Sair();
        }

        private void Sair()
        {
            MessageBox.Alert(this, "Deseja Sair?", "Sair", (_sender, _e) =>
            {
                Intent i = new Intent();
                i.SetClass(this, typeof(ServiceExpediente));
                StopService(i);

                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }, "Cancelar", (_sender, _e) => { }, true);
        }

        private static string DatabaseFilePath
        {
            get
            {
                return Path.Combine(CSGlobal.GetCurrentDirectoryDB(), CSConfiguracao.GetConfig("dbFile") + CSEmpresa.Current.CODIGO_REVENDA + ".sdf");
            }
        }

        private void Eventos()
        {
            grdMenu.ItemClick += GrdMenu_ItemClick;
        }

        private void GrdMenu_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (UtilizaExpediente)
            {
                switch (e.Position)
                {
                    case 0:
                        AbrirListaCliente();
                        break;
                    case 1:
                        AbrirDescarga();
                        break;
                    case 2:
                        AbrirListaRelatorio();
                        break;
                    case 3:
                        AbrirExpediente();
                        break;
                    case 4:
                        AbrirInformativo();
                        break;
                    case 5:
                        AbrirSobre();
                        break;
                    case 6:
                        Sair();
                        break;
                }
            }
            else
            {
                switch (e.Position)
                {
                    case 0:
                        AbrirListaCliente();
                        break;
                    case 1:
                        AbrirDescarga();
                        break;
                    case 2:
                        AbrirListaRelatorio();
                        break;
                    case 3:
                        AbrirInformativo();
                        break;
                    case 4:
                        AbrirSobre();
                        break;
                    case 5:
                        Sair();
                        break;
                }
            }
        }

        private void AbrirDescarga()
        {
            if (CSEmpresa.ColunaExiste("PEDIDO", "BOL_PEDIDO_VALIDADO"))
            {
                int[] Resultados = new int[2];

                Resultados = CSPedidosPDV.EXISTE_PEDIDO_SALVAMENTO_PENDENTE();

                if (Resultados[0] != 0)
                {
                    MessageBox.Alert(this, "O pedido de código " + Resultados[0].ToString() + ", referente ao PDV " + Resultados[1].ToString() + " não foi validado corretamente. Deseja ir ao pedido?", "Ir pedido",
                        (_sender, _e) =>
                        {
                            CSGlobal.ROTINA_VALIDACAO_PEDIDO = true;
                            CSGlobal.CODIGO_VALIDACAO_PEDIDO = Convert.ToInt32(Resultados[0]);
                            CSGlobal.PDV_VALIDACAO_PEDIDO = Convert.ToInt32(Resultados[1]);

                            Intent i = new Intent();
                            i.SetClass(this, typeof(ListaCliente));
                            this.StartActivity(i);

                        }, "Cancelar", (_sender, _e) => { }, false);
                }
                else
                    MessageBox.Alert(this, "Deseja efetuar a descarga?", "Descarga", (_sender, _e) => { Descarga(); }, "Cancelar", null, true);
            }
            else
                MessageBox.Alert(this, "Deseja efetuar a descarga?", "Descarga", (_sender, _e) => { Descarga(); }, "Cancelar", null, true);
        }

        private void AbrirInformativo()
        {
            MessageBox.Alert(this, CSEmpresa.Current.DES_INFORMACAO.Trim(), "Ok", (_sender, _e) => { }, true);
        }

        private void BtnDescarga_Click(object sender, EventArgs e)
        {
            MessageBox.Alert(this, "Deseja efetuar a descarga?", "Descarga", (_sender, _e) => { Descarga(); }, "Cancelar", null, true);
        }

        private void Descarga()
        {
            ValidacoesDescarga();
        }

        private void ValidacoesDescarga()
        {
            if (CSEmpregados.Current.NUM_MINUTOS_TOTAL_EXPEDIENTE > 0 &&
                                    CSEmpregados.Current.ExpedienteAnteriorExistente())
            {
                MessageBox.Alert(this, "Você possui um expediente retroativio que ainda não foi finalizado. Para prosseguir, você deve finalizá-lo. Deseja finalizar?", "Finalizar",
                    (_sender, _e) =>
                    {
                        CSEmpregados.Current.FinalizarExpedienteAnterior();
                        IniciarDescarga();
                    }, "Cancelar", (_sender, _e) => { }, true);
            }
            else
            {
                ValidacoesAlmoco();
            }
        }

        private void ValidacoesAlmoco()
        {
            if (CSEmpregados.Current.AlmocoIniciadoNaoFinalizado())
            {
                if (CSEmpregados.Current.NUM_MINUTOS_INTERVALO_ALMOCO > 0)
                {
                    if (CSEmpregados.Current.IntervaloAlmocoEncerrado())
                    {
                        CSEmpregados.Current.FinalizarAlmoco();
                        IniciarDescarga();
                    }
                    else
                        MessageBox.Alert(this, string.Format("Não é possível realizar descarga dentro do horário de almoco. Falta(m) aproximadamente {0} minuto(s) para o encerramento do seu horário de almoço.", CSEmpregados.Current.TempoAlmocoRestante()));
                }
                else
                {
                    MessageBox.Alert(this, "Você não pode realizar descarga dentro do horário de almoço. Deseja informar fim do horário de almoço?", "Fim almoço",
                        (_sender, _e) =>
                        {
                            CSEmpregados.Current.FinalizarAlmoco();
                            IniciarDescarga();
                        }
                    , "Cancelar", (_sender, _e) =>
                     {

                     }, false);
                }
            }
            else
                IniciarDescarga();
        }

        private void IniciarDescarga()
        {
            progress = new ProgressDialogCustomizado(this, LayoutInflater).Customizar();
            progress.Show();

            new ThreadDescarga().Execute();
        }

        private void BtnListaExpediente_Click(object sender, EventArgs e)
        {
            AbrirExpediente();
        }

        private void BtnSobre_Click(object sender, EventArgs e)
        {
            AbrirSobre();
        }

        private void AbrirSobre()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Sobre));
            this.StartActivity(i);
        }

        private void BtnListaRelatorio_Click(object sender, EventArgs e)
        {
            AbrirListaRelatorio();
        }

        private void AbrirListaRelatorio()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Relatorio));
            this.StartActivity(i);
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Sair();
        }

        void btnListaCliente_Click(object sender, EventArgs e)
        {
            AbrirListaCliente();
        }

        private void AbrirListaCliente()
        {
            if (CSEmpresa.ColunaExiste("PEDIDO", "BOL_PEDIDO_VALIDADO"))
            {
                int[] Resultados = new int[2];

                Resultados = CSPedidosPDV.EXISTE_PEDIDO_SALVAMENTO_PENDENTE();

                if (Resultados[0] != 0)
                {
                    MessageBox.Alert(this, "O pedido de código " + Resultados[0].ToString() + ", referente ao PDV " + Resultados[1].ToString() + " não foi validado corretamente. Deseja ir ao pedido?", "Ir pedido",
                        (_sender, _e) =>
                        {
                            CSGlobal.ROTINA_VALIDACAO_PEDIDO = true;
                            CSGlobal.CODIGO_VALIDACAO_PEDIDO = Convert.ToInt32(Resultados[0]);
                            CSGlobal.PDV_VALIDACAO_PEDIDO = Convert.ToInt32(Resultados[1]);

                            Intent i = new Intent();
                            i.SetClass(this, typeof(ListaCliente));
                            this.StartActivity(i);

                        }, "Cancelar", (_sender, _e) => { }, false);
                }
                else
                {
                    Intent i = new Intent();
                    i.SetClass(this, typeof(ListaCliente));
                    this.StartActivity(i);
                }
            }
            else
            {
                Intent i = new Intent();
                i.SetClass(this, typeof(ListaCliente));
                this.StartActivity(i);
            }
        }

        private void Inicializacao()
        {
            VersaoAvante = PackageManager.GetPackageInfo(PackageName, 0).VersionName;
            CSGlobal.ValidarTopCategoria = CSEmpresa.ColunaExiste("PRODUTO_CATEGORIA", "IND_PROD_TOP_CATEGORIA");
            CSEmpresa.Current.DATA_ULTIMA_DESCARGA = CSDescarga.DataUltimaDescarga;
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            CurrentActivity = this;

            lblVendedor.Text = string.Format("{0} - {1}", CSEmpregados.Current.COD_EMPREGADO, CSEmpregados.Current.NOM_EMPREGADO);
            lblData.Text = DateTime.Now.ToString("dd/MM/yy");

            AbrirInformativo();

            if (!CSEmpregados.Current.IND_VALIDAR_EXPEDIENTE ||
               (CSEmpregados.Current.IND_VALIDAR_EXPEDIENTE &&
               !CSEmpregados.Current.DAT_HORA_INICIO_EXPEDIENTE.HasValue &&
               CSEmpregados.Current.NUM_MINUTOS_TOTAL_EXPEDIENTE == 0))
                UtilizaExpediente = false;
            else
                UtilizaExpediente = true;

            if (CSEmpregados.Current.COD_EMPREGADO != CSGlobal.COD_VENDEDOR_DADOS)
                MessageBox.Alert(this, string.Format("Login efetuado com o vendedor {0}-{1}, e base dados com carga do vendedor {2}-{3}", CSEmpregados.Current.COD_EMPREGADO, CSEmpregados.Current.NOM_EMPREGADO, CSGlobal.COD_VENDEDOR_DADOS, CSEmpregados.Items.Cast<CSEmpregados.CSEmpregado>().Where(e => e.COD_EMPREGADO == CSGlobal.COD_VENDEDOR_DADOS).FirstOrDefault().NOM_EMPREGADO));

            if (CSEmpregados.Current.NUM_MINUTOS_TOTAL_EXPEDIENTE > 0 &&
                CSEmpregados.Current.ExpedienteIniciadoNaoFinalizado() &&
                CSEmpregados.Current.IND_FINALIZA_JORNADA_AUTOMATICA)
            {
                Intent i = new Intent();
                i.SetClass(this, typeof(ServiceExpediente));
                StartService(i);
            }

            List<int> icones = new List<int>();
            icones.Add(Resource.Drawable.clientes_main);
            icones.Add(Resource.Drawable.descarga_main);
            icones.Add(Resource.Drawable.relatorio_main);

            if (UtilizaExpediente)
                icones.Add(Resource.Drawable.sair_main);

            icones.Add(Resource.Drawable.informativo_main);
            icones.Add(Resource.Drawable.sobre_main);
            icones.Add(Resource.Drawable.sair_main);

            List<string> legendas = new List<string>();

            legendas.Add("Clientes");
            legendas.Add("Descarga");
            legendas.Add("Relatório");

            if (UtilizaExpediente)
                legendas.Add("Expediente");

            legendas.Add("Informativo");
            legendas.Add("Sobre");
            legendas.Add("Sair");

            grdMenu.Adapter = new GridViewMenu(this, icones, legendas);
        }

        private class GridViewMenu : BaseAdapter
        {
            List<int> Icones;
            List<string> Legendas;
            AppCompatActivity Context;

            public GridViewMenu(AppCompatActivity context, List<int> ListaIcones, List<string> ListaLegenda)
            {
                Icones = ListaIcones;
                Legendas = ListaLegenda;
                Context = context;
            }

            public override int Count
            {
                get
                {
                    return Icones.Count;
                }
            }

            public override Java.Lang.Object GetItem(int position)
            {
                return Icones[position];
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                int imagemAtual = Icones[position];
                string legendaAtual = Legendas[position];

                if (convertView == null)
                    convertView = LayoutInflater.From(Context)
                      .Inflate(Resource.Layout.main_grid_row, parent, false);

                ImageView imgIcone = convertView.FindViewById<ImageView>(Resource.Id.imgIcone);
                TextView lblLegenda = convertView.FindViewById<TextView>(Resource.Id.lblLegenda);

                imgIcone.SetImageResource(imagemAtual);
                lblLegenda.Text = legendaAtual;

                return convertView;
            }
        }

        private void AbrirExpediente()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Expediente));
            this.StartActivity(i);
        }

        private void FindViewsById()
        {
            grdMenu = FindViewById<GridView>(Resource.Id.grdMenu);
            tbToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.tbToolbar);
            lblVendedor = FindViewById<TextView>(Resource.Id.lblVendedor);
            lblData = FindViewById<TextView>(Resource.Id.lblData);
        }

        private class ThreadDescarga : AsyncTask
        {
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                try
                {
                    DateTime data_ultima_sincronizacao;
                    string codigoVendedorRevenda;

                    try
                    {
                        if (CSEmpresa.Current.IND_UTILIZA_FLEXX_GPS == "S")
                        {
                            CSGlobal.LimpaArquivosFlexxGPS("/sdcard/FLAGPS_BD/ENVIADOS", "*.*");
                            CSGlobal.LimpaArquivosFlexxGPS("/sdcard/FLAGPS_BD/ENVIADOS_GPS", "*.*");
                            CSGlobal.LimpaArquivosFlexxGPS("/sdcard/FLAGPS_BD/MENSAGEM/ENVIADOS", "*.*");
                            CSGlobal.LimpaArquivosFlexxGPS("/sdcard/FLAGPS_BD/MENSAGEM/LIDOS", "*.*");
                        }
                    }
                    catch (System.Exception)
                    {
                        MessageBox.AlertErro(CurrentActivity, "Erro ao tentar limpar arquivos enviados para o FlexX GPS. O processo de descarga continuará normalmente.");
                    }

                    if (!AtualizaDataDoSistema())
                    {
                        return 0;
                    }

                    codigoVendedorRevenda = CSConfiguracao.GetConfig("vendedor" + CSGlobal.COD_REVENDA);
                    if (string.IsNullOrEmpty(codigoVendedorRevenda))
                    {
                        codigoVendedorRevenda = CSConfiguracao.GetConfig("vendedorDefault");
                    }

                    if (!ValidaCodigoEmpregado(codigoVendedorRevenda))
                    {
                        return 0;
                    }

                    CSGlobal.COD_REVENDA = CSEmpresa.Current.CODIGO_REVENDA.PadLeft(8, '0');

                    UpdateProvider();

                    syncProvider.DataBaseFilePath = DatabaseFilePath;

                    var dsDescarga = CSDescarga.Descarga(VersaoAvante);

                    var verCompatibilidade = CSEmpresa.Current.VERSAO_AVANTE_SALES_COMPATIBILIDADE;
                    syncProvider.Descarga(dsDescarga, syncProvider.ProviderName, codigoVendedorRevenda, CSGlobal.COD_REVENDA, verCompatibilidade);

                    data_ultima_sincronizacao = DateTime.Now;

                    UpdateDataSincronizacao(data_ultima_sincronizacao, codigoVendedorRevenda);

                    try
                    {
                        string[] recebeFiles = Directory.GetFiles(CSGlobal.GetCurrentDirectory(), "Dados*");
                        DateTime dataCriacao;

                        foreach (string file in recebeFiles)
                        {
                            dataCriacao = File.GetCreationTime(file);

                            if (dataCriacao.Date != DateTime.Now.Date)
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.AlertErro(CurrentActivity, ex.Message);
                                }
                            }
                        }

                        AtualizarDadosDescarregados();

                        MessageBox.Alert(CurrentActivity, "Descarga realizada com sucesso.");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.AlertErro(CurrentActivity, ex.Message);
                    }
                }
                catch (System.Exception ex)
                {
                    if (!TratamentoErroEmail(ex))
                    {
                        MessageBox.AlertErro(CurrentActivity, ex.Message);
                    }
                    //else
                    //    UpdateDataSincronizacao(DateTime.Now, string.IsNullOrEmpty(CSConfiguracao.GetConfig("vendedor" + CSGlobal.COD_REVENDA)) ? CSConfiguracao.GetConfig("vendedorDefault") : CSConfiguracao.GetConfig("vendedor" + CSGlobal.COD_REVENDA));
                }

                return true;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                progress.Dismiss();
            }

            private void AtualizarDadosDescarregados()
            {
                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PEDIDO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PEDIDO_INDENIZACAO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PRODUTO_COLETA_ESTOQUE SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE ENTRADA_CREDITO_PRODUTO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE TMP_PEDIDO_EXCLUIDO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE HISTORICO_MOTIVO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE MONITORAMENTO_VENDEDOR_ROTA SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE RESPOSTA_PESQUISA_MERCADO SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE RESPOSTA_PESQUISA SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE MOTIVO_NAORESP_PESQ SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PESQUISA_MERCHAN_PDV SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PDV_EMAIL SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE MOTIVO_NAO_PESQUISA SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PDV_VISITA SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE PDV_PRODUTO_VALIDADE SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                CSDataAccess.Instance.ExecuteNonQuery("UPDATE GRUPO_MARKUP SET IND_DESCARREGADO = 1 WHERE IND_DESCARREGADO = 0 OR IND_DESCARREGADO IS NULL");

                if (CSEmpresa.ColunaExiste("INDENIZACAO", "COD_INDENIZACAO"))
                    CSDataAccess.Instance.ExecuteNonQuery("UPDATE INDENIZACAO SET IND_DESCARREGADO = 1");

                CSDataAccess.Instance.FechaConexao();
            }

            private static void UpdateProvider()
            {
                string empresaFile = System.IO.Path.Combine(CSGlobal.GetCurrentDirectory(), "empresas.xml");
                if (CSConfiguracao.GetConfig("syncProvider") != "" && CSConfiguracao.GetConfig("internetURL") != "" && File.Exists(empresaFile))
                {
                    syncProvider = new SQLLiteDirectProvider();

                    if (syncProvider == null)
                    {
                        MessageBox.AlertErro(CurrentActivity, "Erro criando o provider de sincronização. Por favor, verifique a integridade do sistema.");
                        return;
                    }

                    try
                    {
                        var internetURL = Descriptografar(CSConfiguracao.GetConfig("internetURL"));
                        Uri serverAdress;
                        if (Uri.TryCreate("http://" + internetURL + "/AvanteSales/WSSales/AvanteSales.asmx", UriKind.RelativeOrAbsolute, out serverAdress))
                        {
                            syncProvider.ServerAddress = serverAdress.ToString();
                        }
                        else
                        {
                            MessageBox.AlertErro(CurrentActivity, "Endereço do servidor inválido. Por favor configure-o novamente.");
                        }

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.AlertErro(CurrentActivity, ex.Message);
                    }
                }
            }

            private static bool TratamentoErroEmail(System.Exception ex)
            {
                if (ex.Message.Contains("E-mail não enviado. Dados incompletos no cadastro Flexx."))
                {
                    MessageBox.AlertErro(CurrentActivity, "Descarga realizada com e-mail não enviado: dados incompletos no cadastro Flexx.");
                    return true;
                }
                else if (ex.Message.Contains("E-mail não enviado. Dados incorretos no cadastro Flexx."))
                {
                    MessageBox.AlertErro(CurrentActivity, "Descarga realizada com e-mail não enviado: dados incorretos no cadastro Flexx.");
                    return true;
                }

                return false;
            }

            private static void UpdateDataSincronizacao(DateTime data_ultima_sincronizacao, string codigoVendedorRevenda)
            {
                CSDataAccess.Instance.ExecuteScalar("UPDATE INFORMACOES_SINCRONIZACAO SET DATA_ULTIMA_SINCRONIZACAO = datetime('" + data_ultima_sincronizacao.ToString("yyyy-MM-dd HH:mm:ss") +
                                                    "') WHERE COD_EMPREGADO = " + codigoVendedorRevenda);

                CSEmpresa.Current.DATA_ULTIMA_DESCARGA = data_ultima_sincronizacao;
            }
        }

        private static bool ValidaCodigoEmpregado(string strCodigoEmpregado)
        {
            try
            {
                int codigoEmpregado = 0;
                if (int.TryParse(strCodigoEmpregado, out codigoEmpregado))
                {
                    WebService.AvanteSales ws = new WebService.AvanteSales();
                    ws.Url = ws.Url.Replace("localhost", Descriptografar(CSConfiguracao.GetConfig("internetURL")));

                    if (!ws.IsEmpregado(codigoEmpregado, CSGlobal.COD_REVENDA))
                    {
                        MessageBox.AlertErro(CurrentActivity, "O código do vendedor não é válido!\nFavor informar um código válido.");
                        return false;
                    }

                    return true;
                }
                throw new System.Exception("O código do empregado não está com um valor numérico válido.");
            }
            catch (System.Exception)
            {
                MessageBox.AlertErro(CurrentActivity, "Empregado Inválido");
                return false;
            }
        }

        private static string Descriptografar(string conteudoCriptografado)
        {
            string conteudoDescriptografado = string.Empty;

            if (conteudoCriptografado.Contains(' '))
            {
                if (!conteudoCriptografado.Contains(' '))
                    return conteudoCriptografado;

                string[] charSenha = conteudoCriptografado.Split(' ').ToArray();

                for (int i = 0; i < charSenha.Length; i++)
                {
                    int byteAtual = Convert.ToInt32(charSenha[i]);

                    conteudoDescriptografado += Convert.ToChar(Convert.ToInt32(Convert.ToInt32(byteAtual) - 120));
                }
            }
            else
                conteudoDescriptografado = conteudoCriptografado;

            return conteudoDescriptografado;
        }

        private static bool AtualizaDataDoSistema()
        {
            // Altera a data do sistema
            try
            {
                WebService.AvanteSales ws = new WebService.AvanteSales();

                ws.Url = ws.Url.Replace("localhost", Descriptografar(CSConfiguracao.GetConfig("internetURL")));
                ws.Timeout = 15000;
                DateTime d = ws.GetServerDate();

                // Funcao para alterar a data
                CSGlobal.MudaData(d);

                return true;

            }
            catch (System.Exception)
            {
                MessageBox.AlertErro(CurrentActivity, "Falha na conexão");
                return false;
            }
        }
    }
}
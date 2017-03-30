using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AvanteSales.Pro.Activities;

namespace AvanteSales.Pro.Dialogs
{
    public class DialogFragmentProdutosNaoValidosEscalonamento : Android.Support.V4.App.DialogFragment
    {
        private List<string[]> ListaProdutosNaoValidos;
        ListView listProdutos;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.dialog_fragment_produtos_nao_validos_escalonamento, container, false);
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            FindViewsById(view);
            Eventos();
            return view;
        }

        private void Eventos()
        {
            listProdutos.ItemClick += ListProdutos_ItemClick;
        }

        private void ListProdutos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var codProduto = Convert.ToInt32(ListaProdutosNaoValidos[e.Position][0]);

            foreach (CSItemsPedido.CSItemPedido itemAtual in CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS)
            {
                if (itemAtual.STATE == ObjectState.DELETADO)
                    continue;

                var cod = itemAtual.PRODUTO.COD_PRODUTO;

                if (cod == codProduto)
                {
                    CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS.Current = itemAtual;

                    if (CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS.Current.PRODUTO != null)
                        CSProdutos.Current = CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS.Current.PRODUTO;

                    if (CSGlobal.PedidoSugerido)
                        CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS.Current.PRODUTO.EDITOU_DADOS = true;

                    if (CSTiposDistribPolicitcaPrecos.Current.COD_TIPO_DISTRIBUICAO_POLITICA != 2 && CSPDVs.Current.PEDIDOS_PDV.Current.ITEMS_PEDIDOS.Count > 0)
                        CSPDVs.Current.PEDIDOS_PDV.Current.DesfazRateioIndenizacao();

                    ((Cliente)Activity).NavegarEdicaoProduto();
                }
            }

            Dismiss();
        }

        private void FindViewsById(View view)
        {
            listProdutos = view.FindViewById<ListView>(Resource.Id.listProdutos);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            ListaProdutosNaoValidos = ((Cliente)Activity).GetListaProdutosNaoValidos();

            listProdutos.Adapter = new ProdutosAdapter(Activity, Resource.Layout.dialog_fragment_produtos_nao_validos_escalonamento_row, ListaProdutosNaoValidos);
        }

        class ProdutosAdapter : ArrayAdapter
        {
            Context context;
            IList<string[]> produtos;
            int resourceId;

            public ProdutosAdapter(Context c, int resource, List<string[]> objects)
                : base(c, resource, objects)
            {
                context = c;
                produtos = objects;
                resourceId = resource;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                string[] produto = produtos[position];

                LayoutInflater layout = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
                if (convertView == null)
                    convertView = layout.Inflate(resourceId, null);

                if (produto != null)
                {
                    TextView lblDescProduto = convertView.FindViewById<TextView>(Resource.Id.lblDescProduto);
                    TextView lblCodigo = convertView.FindViewById<TextView>(Resource.Id.lblCodigo);
                    TextView lblDescontoMaximo = convertView.FindViewById<TextView>(Resource.Id.lblDescontoMaximo);
                    TextView lblDescontoInformado = convertView.FindViewById<TextView>(Resource.Id.lblDescontoInformado);

                    lblDescProduto.Text = produto[2];
                    lblCodigo.Text = produto[1];
                    lblDescontoMaximo.Text = produto[3];
                    lblDescontoInformado.Text = produto[4];
                }
                return convertView;
            }
        }
    }
}
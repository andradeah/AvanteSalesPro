package md5e8a4f07b5509893786d75ec5872385e8;


public class CSProdutos_CSProdutoVencimento
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AvanteSales.CSProdutos+CSProdutoVencimento, AvanteSales.SystemFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CSProdutos_CSProdutoVencimento.class, __md_methods);
	}


	public CSProdutos_CSProdutoVencimento () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CSProdutos_CSProdutoVencimento.class)
			mono.android.TypeManager.Activate ("AvanteSales.CSProdutos+CSProdutoVencimento, AvanteSales.SystemFramework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
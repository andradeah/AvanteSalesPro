package md5e93e0e65cca7dbdc8c7c4a87fc493852;


public class ListaCliente_ResizeAnimation
	extends android.view.animation.Animation
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_applyTransformation:(FLandroid/view/animation/Transformation;)V:GetApplyTransformation_FLandroid_view_animation_Transformation_Handler\n" +
			"";
		mono.android.Runtime.register ("AvanteSales.Pro.Activities.ListaCliente+ResizeAnimation, AvanteSales.Pro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ListaCliente_ResizeAnimation.class, __md_methods);
	}


	public ListaCliente_ResizeAnimation () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ListaCliente_ResizeAnimation.class)
			mono.android.TypeManager.Activate ("AvanteSales.Pro.Activities.ListaCliente+ResizeAnimation, AvanteSales.Pro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public ListaCliente_ResizeAnimation (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == ListaCliente_ResizeAnimation.class)
			mono.android.TypeManager.Activate ("AvanteSales.Pro.Activities.ListaCliente+ResizeAnimation, AvanteSales.Pro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}

	public ListaCliente_ResizeAnimation (android.view.View p0, float p1, float p2, float p3, float p4) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ListaCliente_ResizeAnimation.class)
			mono.android.TypeManager.Activate ("AvanteSales.Pro.Activities.ListaCliente+ResizeAnimation, AvanteSales.Pro, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e:System.Single, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2, p3, p4 });
	}


	public void applyTransformation (float p0, android.view.animation.Transformation p1)
	{
		n_applyTransformation (p0, p1);
	}

	private native void n_applyTransformation (float p0, android.view.animation.Transformation p1);

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

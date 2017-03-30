using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;

namespace AvanteSales.Pro.Activities
{
    [Activity(Label = "TesteAnimation")]
    public class TesteAnimation : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.teste_animation);

            ImageView llTeste = FindViewById<ImageView>(Resource.Id.imgGrafico);

            var teste = new ResizeAnimation(llTeste, 0, 70, 300, 70);

            llTeste.StartAnimation(teste);
        }
    }

    public class ResizeAnimation : Animation
    {
        private View mView;
        private float mToHeight;
        private float mFromHeight;

        private float mToWidth;
        private float mFromWidth;

        public ResizeAnimation(View v, float fromWidth, float fromHeight, float toWidth, float toHeight)
        {
            mToHeight = toHeight;
            mToWidth = toWidth;
            mFromHeight = fromHeight;
            mFromWidth = fromWidth;
            mView = v;
            Duration = 3000;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            float height =
                (mToHeight - mFromHeight) * interpolatedTime + mFromHeight;
            float width = (mToWidth - mFromWidth) * interpolatedTime + mFromWidth;
            var p = mView.LayoutParameters;
            p.Height = (int)height;
            p.Width = (int)width;
            mView.RequestLayout();
        }
    }
}
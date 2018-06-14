using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using Android.Graphics;

namespace TicTacToe
{
    [Activity(Label = "TICTAETOE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Android.App.Activity, View.IOnClickListener
    {

        private Button buttonUserGame, buttonAndroidGame, buttonExit;
        private TextView textviewTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            RequestWindowFeature(WindowFeatures.NoTitle);
            Context mContext = Application.Context;
            Typeface localTypeface = Typeface.CreateFromAsset(mContext.Assets, "fonts/china1.ttf");
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            textviewTitle = FindViewById<TextView>(Resource.Id.txtvwTitle);
            textviewTitle.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonUserGame = FindViewById<Button>(Resource.Id.btnUserGame);
            buttonUserGame.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonUserGame.SetTextColor(Color.White);
            buttonUserGame.SetOnClickListener(this);
            buttonAndroidGame = FindViewById<Button>(Resource.Id.btnAndroidGame);
            buttonAndroidGame.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonAndroidGame.SetTextColor(Color.White);
            buttonAndroidGame.SetOnClickListener(this);
            buttonExit = FindViewById<Button>(Resource.Id.btnExit);
            buttonExit.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonExit.SetTextColor(Color.White);
            buttonExit.SetOnClickListener(this);
        }

        public void OnClick(View view)
        {
            switch (view.Id)
            {
                case Resource.Id.btnUserGame:
                    Intent gameUser = new Intent(this, typeof(MainScreenActivity));
                    StartActivity(gameUser);
                    break;
                case Resource.Id.btnAndroidGame:
                    Intent gameAndroid = new Intent(this, typeof(MainScreenActivity));
                    gameAndroid.PutExtra("TypeGame", "Android");
                    StartActivity(gameAndroid);
                    break;
                case Resource.Id.btnExit:
                    Finish();
                    break;
            }
        }
    }
}


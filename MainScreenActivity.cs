using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Java.Lang;
using Android.Views.Animations;
using Android.Animation;
using Android.Util;

using TicTacToe.Dialog;

namespace TicTacToe
{
    [Activity(Label = "MainScreen")]
    public class MainScreenActivity : Android.App.Activity, View.IOnClickListener, IDialogListener
    {
        private TableLayout tableLayout;
        private Button buttonNewGame, buttonMainScreen;
        private static int sizeArray = 10;
  
        public static Button[,] buttons = new Button[sizeArray, sizeArray];     
        public static TextView txtViewCount;     
        public static string typeGame = string.Empty;
        public static MainScreenActivity MainScrActivity;

        public MainScreenActivity()
        {
            
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            RequestWindowFeature(WindowFeatures.NoTitle);
            Context mContext = Application.Context;
            Typeface localTypeface = Typeface.CreateFromAsset(mContext.Assets, "fonts/china1.ttf");
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MainScreen);
            tableLayout = FindViewById<TableLayout>(Resource.Id.tablelayout);
            buttonNewGame = FindViewById<Button>(Resource.Id.btnNewGame);
            buttonNewGame.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonNewGame.SetTextColor(Color.White);
            buttonNewGame.SetOnClickListener(this);
            buttonMainScreen = FindViewById<Button>(Resource.Id.btnMainScreen);
            buttonMainScreen.SetTypeface(localTypeface, TypefaceStyle.Normal);
            buttonMainScreen.SetTextColor(Color.White);
            buttonMainScreen.SetOnClickListener(this);
            txtViewCount = FindViewById<TextView>(Resource.Id.txtvwCount);
            txtViewCount.SetTypeface(localTypeface, TypefaceStyle.Normal);
            typeGame = Intent.GetStringExtra("TypeGame");
            BuildGameField();
            MainScrActivity = this;
        }
        protected override void OnResume()
        {
            base.OnResume();

        }
        private void BuildGameField()
        {
           
        }

        public void OnClick(View v)
        {
            Button b = (Button)v;
            switch (v.Id)
            {
                case Resource.Id.btnNewGame:
                    
                    break;
                case Resource.Id.btnMainScreen:
                    Finish();
                    break;
            }
        }

        public void FinishDialogListener(string inputText)
        {
            if (inputText.Contains("New"))
            {             
                Listener.Refresh();
            }
        }

        public class Listener : Android.App.Activity, View.IOnClickListener
        {
            private int x = 0;
            private int y = 0;

            public Listener(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void OnClick(View view)
            {
               
            }
            public static void SetAndroidPosition()
            {
               
            }
            public static void SetAndroidPositionRandom()
            {
              
            }
            private bool MakeTurn(int x, int y)
            {
                return true;
            }
            static int cntRepeat;
            private static void WonAnimation(Java.Lang.Object value)
            {
                cntRepeat = 0;
                ObjectAnimator colorAnim = ObjectAnimator.OfInt(value, "textColor", Color.White, Color.Transparent);
                colorAnim.SetDuration(800);
                colorAnim.SetEvaluator(new ArgbEvaluator());
                colorAnim.RepeatCount = 3;
                colorAnim.RepeatMode = ValueAnimatorRepeatMode.Reverse;
                colorAnim.Start();
                BlockButton();
                colorAnim.AnimationEnd += ColorAnim_AnimationEnd;
            }
            
            private static void ColorAnim_AnimationEnd(object sender, EventArgs e)
            {
                cntRepeat++;
                if (cntRepeat == 5)
                {
                    ShowCustomDialog CustomDialog = new ShowCustomDialog();
                    CustomDialog.Show(MainScrActivity, "Повторить???");
                }
                //string text = "Player \"" + player.GetName() + "\" won!";
                //string text = "Try Again!!!";
                //Toast.MakeText(Application.Context, text, ToastLength.Short).Show();                              
                Log.Info("MainScreen", cntRepeat.ToString());          
            }

            private static void GameOver()
            {
                string text = "Draw";
                Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
                //game.Reset();
                //Refresh();
            }

            public static void BlockButton()
            {
            }

            public static void Refresh()
            {
                
            }
        }
    }
}
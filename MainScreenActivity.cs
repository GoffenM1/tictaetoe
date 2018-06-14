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

        public static Game game;     
        public static Button[,] buttons = new Button[sizeArray, sizeArray];     
        public static TextView txtViewCount;     
        public static string typeGame = string.Empty;
        public static MainScreenActivity MainScrActivity;

        public MainScreenActivity()
        {
            game = new Game(sizeArray);
            game.Start();
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
            Square[,] field = game.GetField();
            for (int i = 0; i < field.GetLength(0); i++)
            {
                TableRow tableRow = new TableRow(this); // создание строки таблицы
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    Button button = new Button(this);
                    button.SetSingleLine(true);
                    buttons[i, j] = button;
                    button.SetOnClickListener(new Listener(i, j));
                    //button.SetBackgroundColor(Android.Graphics.Color.Red);
                    button.SetBackgroundResource(Resource.Drawable.button_cell);
                    button.TextAlignment = TextAlignment.Center;
                    float factor = Application.Context.Resources.DisplayMetrics.Density;
                    button.TextSize = 10;
                    button.SetTextColor(Color.White);
                    TableRow.LayoutParams layPar = new TableRow.LayoutParams(TableRow.LayoutParams.WrapContent, TableRow.LayoutParams.WrapContent);
                    
                    layPar.SetMargins(0, 0, -2, -2);

                    if (i == field.GetLength(0) - 1)
                    {
                        layPar.SetMargins(0, 0, -2, 2);
                    }
                    if (j == field.GetLength(1) - 1)
                    {
                        layPar.SetMargins(0, 0, 2, -2);
                    }
                    
                    layPar.Width = Convert.ToInt32(31 * factor);
                    layPar.Height = Convert.ToInt32(31 * factor);

                    button.LayoutParameters = layPar;
                    tableRow.AddView(button, j);
                }
                tableLayout.AddView(tableRow, i);
            }
            //if (typeGame != null && typeGame.Contains("Android"))
            //{
            //    Listener.SetAndroidPosition();
            //}
        }

        public void OnClick(View v)
        {
            Button b = (Button)v;
            switch (v.Id)
            {
                case Resource.Id.btnNewGame:
                    Square[,] field = game.GetField();
                    for (int i = 0; i < field.GetLength(0); i++)
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            game.Reset();
                            Listener.Refresh();
                        }
                    }
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
                game.Reset();
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
                Button button = (Button)view;
                Game g = game;
                Player player = g.GetCurrentActivePlayer();
                if (MakeTurn(x, y))
                {
                    button.SetText(player.GetName().ToCharArray(), 0, player.GetName().Length);
                    txtViewCount.Text = "Ходов: " + game.GetCountFilledCell();
                    Player winner = g.IsPlayerWin();
                    if (winner != null)
                    {
                        GameOver(winner);                     
                    }
                    if (g.IsFieldFilled())
                    {  // в случае, если поле заполнено
                        GameOver();
                    }
                    if (typeGame != null && typeGame.Contains("Android"))
                    {
                        SetAndroidPosition();
                    }
                }               
            }
            public static void SetAndroidPosition()
            {
                Player player = game.GetCurrentActivePlayer();
                List<int> id = game.SetAndroidPosition();
                if (id.Count > 0)
                {
                    buttons[id[0], id[1]].SetText(player.GetName().ToCharArray(), 0, player.GetName().Length);
                    txtViewCount.Text = "Ходов: " + game.GetCountFilledCell();
                }
                Player winner = game.IsPlayerWin();
                if (winner != null)
                {
                    GameOver(winner);
                }
                else if (game.IsFieldFilled())
                {  // в случае, если поле заполнено
                    GameOver();
                }
                //else
                //{
                //    SetAndroidPositionRandom();
                //}            
            }
            public static void SetAndroidPositionRandom()
            {
                Player player = game.GetCurrentActivePlayer();
                List<int> id = game.SetAndroidPositionRandom();
                if (id.Count > 0)
                {
                    buttons[id[0], id[1]].SetText(player.GetName().ToCharArray(), 0, player.GetName().Length);
                    txtViewCount.Text = "Ходов: " + game.GetCountFilledCell();
                }
                Player winner = game.IsPlayerWin();
                if (winner != null)
                {
                    GameOver(winner);
                }
                else if (game.IsFieldFilled())
                {  // в случае, если поле заполнено
                    GameOver();
                }
                else
                {
                    SetAndroidPosition();                  
                }
            }
            private bool MakeTurn(int x, int y)
            {
                return game.MakeTurn(x, y);
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

            private static void GameOver(Player player)
            {
                int y = game.yStart;              
                if (game.xStart == game.xEnd)
                {
                    for (int i = game.yStart; i < game.yEnd; i++)
                    {
                        WonAnimation(buttons[game.xStart, i]);
                        //buttons[game.xStart, i].StartAnimation(anim);
                    }
                }
                else if (game.yStart > game.yEnd)
                {
                    y = y - 1;
                    for (int i = game.xStart; i < game.xEnd; i++)
                    {
                        WonAnimation(buttons[i, y]);
                        //buttons[i, y].StartAnimation(anim);
                        if (game.yStart != game.yEnd)
                        {
                            y--;
                        }
                    }
                }
                else
                {
                    for (int i = game.xStart; i < game.xEnd; i++)
                    {
                        WonAnimation(buttons[i, y]);
                        //buttons[i, y].StartAnimation(anim);
                        if (game.yStart != game.yEnd)
                        {
                            y++;
                        }
                    }
                }              
                //game.Reset();
                //Refresh();
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
                Square[,] field = game.GetField();
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (field[i, j].GetPlayer() == null)
                        {
                            buttons[i, j].Clickable = false;
                        }
                    }
                }
            }

            public static void Refresh()
            {
                Square[,] field = game.GetField();
                char[] c = string.Empty.ToCharArray();
                txtViewCount.Text = "Ходов: ";
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (field[i, j].GetPlayer() == null)
                        {
                            buttons[i, j].SetText(c, 0, c.Length);
                            buttons[i, j].Clickable = true;
                            //buttons[i, j].SetBackgroundResource(Resource.Drawable.button_cell);
                        }
                        else
                        {
                            buttons[i, j].SetText(field[i, j].GetPlayer().GetName().ToCharArray(), 0, field[i, j].GetPlayer().GetName().Length);
                        }
                    }
                }
            }
        }
    }
}
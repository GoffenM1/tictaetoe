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
using Android.Util;

namespace TicTacToe
{
    public class WinnerCheckerVertical : IWinnerChecker, IAndroidLogic
    {
        private Game game;
        private static string TAG = "WinnerCheckerVertical";
        public WinnerCheckerVertical(Game game)
        {
            this.game = game;
        }

        public void CalculateCellsValue()
        {
            int countX = 0;
            int totalValueX = 0;
            int countO = 0;
            int totalValueO = 0;

            Square[,] field = game.GetField();
            Log.Info(TAG, "FindCols");

            for (int i = 0;  i < field.GetLength(0); i++)
            {
                countX = 0;
                countO = 0;

                for (int j = 0; j < field.GetLength(1); j++)
                {
                    totalValueX = 0;
                    totalValueO = 0;
                    if (!field[i,j].IsFilled())
                    {
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countX = findLineSize(i, j - a, "X");
                            if (countX == 1) totalValueX += 1;
                            if (countX == 2) totalValueX += 10;
                            if (countX == 3) totalValueX += 100;
                            if (countX == 4) totalValueX += 1000;
                            if (countX == 5) totalValueX += 10000;

                        }
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countO = findLineSize(i, j - a, "O");
                            if (countO == 1) totalValueO += 1;
                            if (countO == 2) totalValueO += 10;
                            if (countO == 3) totalValueO += 100;
                            if (countO == 4) totalValueO += 1000;
                            if (countO == 5) totalValueO += 10000;

                        }
                    }
                    field[i, j].addValueO(totalValueO);
                    field[i, j].addValueX(totalValueX);
                    //Log.i(TAG, "totalValueO="+totalValueO+" totalValueX="+totalValueX+" i="+i+" j="+j);
                }

            }   

        }

        private int findLineSize(int x, int y, String player)
        {
            Square[,] field = game.GetField();
            int size = 0;
            int count = 0;
            for (int i = y; i < y + game.strSize; i++)
            {
                try
                {
                    if (field[x,i].IsFilled())
                        if (field[x,i].getPlayer().GetName().ToString().Contains(player))
                        {
                            size++;
                            count++;
                        }
                        else
                            size = 0;
                    else
                        size++;
                }
                catch (Exception e)
                {
                    //Log.i(TAG, "catch search emty cells x=" + x + " i=" + i);
                }

            }
            count++;
            if (size == game.strSize) return count;
            return 0;
        }

        public Player CheckWinner()
        {
            Square[,] field = game.GetField();
            Player currPlayer;
            Player lastPlayer = null;
            for (int i = 0, len = field.GetLength(0); i < len; i++)
            {
                lastPlayer = null;
                int successCounter = 1;
                for (int j = 0, len2 = field.GetLength(0); j < len2; j++)
                {
                    currPlayer = field[j, i].GetPlayer();
                    if (currPlayer == lastPlayer && (currPlayer != null && lastPlayer != null))
                    {
                        successCounter++;
                        if (successCounter == game.strSize || successCounter == len2)
                        {
                            return currPlayer;
                        }
                    }
                    lastPlayer = currPlayer;
                }
            }
            return null;
        }
    }
}
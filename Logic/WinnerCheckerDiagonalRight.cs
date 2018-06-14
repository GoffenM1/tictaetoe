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
    public class WinnerCheckerDiagonalRight : IWinnerChecker, IAndroidLogic
    {
        private Game game;
        private static string TAG = "WinnerCheckerDiagonalRight";

        public WinnerCheckerDiagonalRight(Game game)
        {
            this.game = game;
        }

        public void CalculateCellsValue()
        {
            int countX = 0;
            int totalValueX_up = 0;
            int totalValueX_down = 0;
            int countO = 0;
            int totalValueO_up = 0;
            int totalValueO_down = 0;

            Square[,] field = game.GetField();
            Log.Info(TAG, "FindDiagRight");

            for (int i = 0; i < field.GetLength(0); i++)
            {
                countX = 0;
                countO = 0;
                int b = i;
                for (int j = field.GetLength(0) - 1; j >= 0; j--)
                {
                    if (b > (field.GetLength(0) - 1)) continue;
                    totalValueX_up = 0;
                    totalValueO_up = 0;
                    totalValueX_down = 0;
                    totalValueO_down = 0;
                    if (!field[b,j].IsFilled())
                    {
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countX = findLineSize(b - a, j + a, "X");
                            totalValueX_up += getValue(countX);

                            countO = findLineSize(b - a, j + a, "O");
                            totalValueO_up += getValue(countO);

                        }

                    }
                    if (!field[b - i,j - i].IsFilled() & i > 0)
                    {
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countX = findLineSize(b - i - a, j - i + a, "X");
                            totalValueX_down += getValue(countX);

                            countO = findLineSize(b - i - a, j - i + a, "O");
                            totalValueO_down += getValue(countO);

                        }

                    }
                    field[b,j].addValueO(totalValueO_up);
                    field[b,j].addValueX(totalValueX_up);
                    field[b - i,j - i].addValueO(totalValueO_down);
                    field[b - i,j - i].addValueX(totalValueX_down);
                    Log.Info(TAG, "totalValueO=" + totalValueO_up + " totalValueX=" + totalValueX_up + " b=" + b + " j=" + j);
                    b++;
                }

            }   

        }

        private int getValue(int count)
        {
            if (count == 1) return 1;
            if (count == 2) return 10;
            if (count == 3) return 100;
            if (count == 4) return 1000;
            if (count == 5) return 10000;
            return 0;
        }

        private int findLineSize(int x, int y, String player)
        {
            Square[,] field = game.GetField();
            int size = 0;
            int count = 0;
            for (int i = 0; i < game.strSize; i++)
            {
                try
                {
                    if (field[x + i,y - i].IsFilled())
                        if (field[x + i,y - i].getPlayer().GetName().ToString().Contains(player))
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
            int successCounter = 1;
            int len = field.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                currPlayer = field[i, len - (i + 1)].GetPlayer();
                if (currPlayer != null)
                {
                    if (lastPlayer == currPlayer)
                    {
                        successCounter++;
                        if (successCounter == game.strSize || successCounter == len)
                        {
                            return currPlayer;
                        }
                    }
                }
                lastPlayer = currPlayer;
            }
            return null;
        }
    }
}
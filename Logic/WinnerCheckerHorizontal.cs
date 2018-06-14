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

namespace TicTacToe
{
    public class WinnerCheckerHorizontal : IWinnerChecker, IAndroidLogic
    {
        private Game game;
        private static string TAG = "WinnerCheckerHorizontal";

        public WinnerCheckerHorizontal(Game game)
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

            for (int i = 0; i < field.GetLength(0); i++)
            {
                countX = 0;
                countO = 0;

                for (int j = 0; j < field.GetLength(0); j++)
                {
                    totalValueX = 0;
                    totalValueO = 0;
                    if (!field[j,i].IsFilled())
                    {
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countX = findLineSize(j - a, i, "X");
                            if (countX == 1) totalValueX += 1;
                            if (countX == 2) totalValueX += 10;
                            if (countX == 3) totalValueX += 100;
                            if (countX == 4) totalValueX += 1000;
                            if (countX == 5) totalValueX += 10000;

                        }
                        for (int a = 0; a < game.strSize; a++)
                        {
                            countO = findLineSize(j - a, i, "O");
                            if (countO == 1) totalValueO += 1;
                            if (countO == 2) totalValueO += 10;
                            if (countO == 3) totalValueO += 100;
                            if (countO == 4) totalValueO += 1000;
                            if (countO == 5) totalValueO += 10000;

                        }
                    }
                    field[j,i].addValueO(totalValueO);
                    field[j,i].addValueX(totalValueX);
                    //Log.i(TAG, "totalValueO="+totalValueO+" totalValueX="+totalValueX+" i="+i+" j="+j);	        			
                }
            }               

        }

        private int findLineSize(int x, int y, String player)
        {
            Square[,] field = game.GetField();
            int size = 0;
            int count = 0;
            for (int i = x; i < x + game.strSize; i++)
            {
                try
                {
                    if (field[i,y].IsFilled())
                        if (field[i,y].getPlayer().GetName().ToString().Contains(player))
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
                    currPlayer = field[i,j].GetPlayer();
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
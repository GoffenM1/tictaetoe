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
    public class Game
    {
        private static string TAG = "Game";	
        public int strSize;
        /**
         * игроки
         */
        private Player[] players;
        /**
         * поле
         */
        private Square[,] field;
        /**
         * начата ли игра?
         */
        private bool started;
        /**
         * текущий игрок
         */
        private Player activePlayer;
        /**
         * Считает колличество заполненных ячеек
         */
        private int filled;
        /**
         * Всего ячеек
         */
        private int countX;
        private int countY;
        private int squareCount;
        private IAndroidLogic[] androLogic;
        private IWinnerChecker[] winnerCheckers;
        /**
         * Конструктор
         *
         */
        public Game(int sizeArray)
        {
            strSize = sizeArray > 5 ? 5 : sizeArray;
            field = new Square[sizeArray, sizeArray];
            squareCount = 0;
            // заполнение поля
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i,j] = new Square();
                    squareCount++;
                }
            }
            winnerCheckers = new IWinnerChecker[4];
            winnerCheckers[0] = new WinnerCheckerHorizontal(this);
            winnerCheckers[1] = new WinnerCheckerVertical(this);
            winnerCheckers[2] = new WinnerCheckerDiagonalLeft(this);
            winnerCheckers[3] = new WinnerCheckerDiagonalRight(this);
            players = new Player[2];
            started = false;
            activePlayer = null;
            filled = 0;

            Log.Info(TAG, "new AndroLogic");
            androLogic = new IAndroidLogic[4];
            androLogic[0] = new WinnerCheckerVertical(this);
            androLogic[1] = new WinnerCheckerHorizontal(this);
            androLogic[2] = new WinnerCheckerDiagonalLeft(this);
            androLogic[3] = new WinnerCheckerDiagonalRight(this);
            Log.Info(TAG, "---------------------");

        }

        public void Start()
        {
            ResetPlayers();
            started = true;
        }

        private void ResetPlayers()
        {
            players[0] = new Player("X");
            players[1] = new Player("O");
            SetCurrentActivePlayer(players[0]);
        }

        public Square[,] GetField()
        {
            return field;
        }
        public int GetCountFilledCell()
        {
            return filled;
        }
        public Player CheckWinner()
        {
            foreach (IWinnerChecker winnerChecker in winnerCheckers)
            {
                Player winner = winnerChecker.CheckWinner();
                if (winner != null)
                {
                    return winner;
                }
            }
            return null;
        }
        private void SetCurrentActivePlayer(Player player)
        {
            activePlayer = player;
        }

        public bool MakeTurn(int x, int y)
        {
            Log.Info(TAG, "MakeTurn");
            if (field[x,y].IsFilled())
            {
                return false;
            }
            field[x,y].Fill(GetCurrentActivePlayer());
            filled++;
            SwitchPlayers();
            return true;
        }

        public List<int> SetAndroidPosition()
        {
            int maxX = 0;
            int maxO = 0;
            int xX = 0, yX = 0;
            int xO = 0, yO = 0;
            List<int> id = new List<int>();

            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i,j].setValueO(0);
                    field[i,j].setValueX(0);
                }
            Log.Info(TAG, "androLogic.length=" + androLogic.Length + " " + androLogic);
            foreach (var androLog in androLogic)
            {
                androLog.CalculateCellsValue();
            }
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (maxX < field[i,j].getValueX())
                    {
                        maxX = field[i,j].getValueX();
                        xX = i; yX = j;
                        Log.Info(TAG, "maxX=" + maxX + " i=" + i + " j=" + j);

                    }
                    if (maxO < field[i,j].getValueO())
                    {
                        maxO = field[i,j].getValueO();
                        xO = i; yO = j;
                        Log.Info(TAG, "maxO=" + maxO + " i=" + i + " j=" + j);

                    }
                }
            if (strSize == 5 & (maxX < 2000 || maxO > 10000) || strSize == 3 & (maxX < 100 || maxO >= 100))
            {
                if (!field[xO, yO].IsFilled())
                {
                    MakeTurn(xO, yO);
                    id.Add(xO);
                    id.Add(yO);
                }
            }
            else
            {
                if (!field[xX, yX].IsFilled())
                {
                    MakeTurn(xX, yX);
                    id.Add(xX);
                    id.Add(yX);
                }
            }

            if (GetCurrentActivePlayer() == players[1])
            {
                for (int i = 0; i < field.GetLength(0); i++)
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (!field[i, j].IsFilled())
                        {
                            Log.Info(TAG, "GetCurrentActivePlayer");
                            MakeTurn(i, j);
                            id.Add(i);
                            id.Add(j);
                            return id;
                        }
                    }
            }
            return id;
        }
        public List<int> SetAndroidPositionRandom()
        {
            List<int> id = new List<int>();
            Random rnd = new Random();
            while (filled <= squareCount)
            {
                int i = rnd.Next(0, field.GetLength(0));
                int j = rnd.Next(0, field.GetLength(0));
                if (!field[i, j].IsFilled())
                {
                    field[i, j].Fill(GetCurrentActivePlayer());
                    filled++;
                    SwitchPlayers();
                    id.Add(i);
                    id.Add(j);
                    break;
                }
            }
            return id;
        }

        public int xStart = 0;
        public int yStart = 0;

        public int xEnd = 0;
        public int yEnd = 0;
        public Player IsPlayerWin()
        {
            int col;
            int str;
            int diag1_up;
            int diag1_down;
            int diag2_up;
            int diag2_down;
            Log.Info(TAG, "IsPlayerWin");
            for (int i = 0; i < field.GetLength(0); i++)
            {
                col = 0;
                str = 0;
                diag1_up = 0;
                diag1_down = 0;
                diag2_up = 0;
                diag2_down = 0;

                for (int j = 0; j < field.GetLength(0); j++)
                {
                    if (field[i, j].getPlayer() != GetCurrentActivePlayer() &
                            field[i, j].getPlayer() != null)
                    {
                        col++;
                        if (col == strSize)
                        {
                            Log.Info(TAG, string.Format("x1={0} y1={1} x2={2} y2={3}", i, j - strSize + 1, i, j + 1));
                            xStart = i;
                            yStart = j - strSize + 1;
                            xEnd = i;
                            yEnd = j + 1;
                            return field[i, j].getPlayer();
                        }
                    }
                    else col = 0;
                    if (field[j, i].getPlayer() != GetCurrentActivePlayer() &
                            field[j, i].getPlayer() != null)
                    {
                        str++;
                        if (str == strSize)
                        {                          
                            xStart = j - strSize + 1;
                            yStart = i;
                            xEnd = j + 1;
                            yEnd = i;
                            return field[j, i].getPlayer();
                        }
                    }
                    else str = 0;
                }
                int a = i;
                for (int j = 0; j < field.GetLength(0); j++)
                {
                    if (a > (field.GetLength(0) - 1)) continue;
                    if (field[a,j].getPlayer() != GetCurrentActivePlayer() &
                            field[a,j].getPlayer() != null)
                    {
                        diag1_up++;
                        if (diag1_up == strSize)
                        {
                            Log.Info(TAG, string.Format("x1={0} y1={1} x2={2} y2={3}", a - strSize + 1, j - strSize + 1, a + 1, j + 1));
                            xStart = a - strSize + 1;
                            yStart = j - strSize + 1;
                            xEnd = a + 1;
                            yEnd = j + 1;
                            return field[a, j].getPlayer();
                        }
                    }
                    else diag1_up = 0;
                    if (field[j,a].getPlayer() != GetCurrentActivePlayer() &
                            field[j,a].getPlayer() != null)
                    {
                        diag1_down++;
                        if (diag1_down == strSize)
                        {
                            Log.Info(TAG, string.Format("x1={0} y1={1} x2={2} y2={3}", j - strSize + 1, a - strSize + 1, j + 1, a + 1));
                            xStart = j - strSize + 1;
                            yStart = a - strSize + 1;
                            xEnd = j + 1;
                            yEnd = a +1;                          
                            return field[j, a].getPlayer();
                        }
                    }
                    else diag1_down = 0;
                    a++;
                }

                int b = i;
                for (int j = field.GetLength(0) - 1, l2 = -1; j > l2; j--)
                {
                    if (b > field.GetLength(0) - 1) continue;
                    if (field[b,j].getPlayer() != GetCurrentActivePlayer() &
                            field[b,j].getPlayer() != null)
                    {
                        diag2_down++;
                        if (diag2_down == strSize)
                        {
                            Log.Info(TAG, string.Format("x1={0} y1={1} x2={2} y2={3}", b - strSize + 1, j + strSize, b + 1, j));
                            xStart = b - strSize + 1;
                            yStart = j + strSize;
                            xEnd = b + 1;
                            yEnd = j;
                            return field[b, j].getPlayer();
                        }
                    }
                    else diag2_down = 0;
                    if (field[b - i,j - i].getPlayer() != GetCurrentActivePlayer() &
                            field[b - i,j - i].getPlayer() != null)
                    {
                        diag2_up++;
                        if (diag2_up == strSize)
                        {
                            Log.Info(TAG, string.Format("x1={0} y1={1} x2={2} y2={3}", b - strSize + 1, j - i + strSize, b - i + 1, j - i));                          
                            xStart = b - i - strSize + 1;
                            yStart = j - i + strSize;
                            xEnd = b - i + 1;
                            yEnd = j - i;
                            return field[b - i, j - i].getPlayer();
                        }
                    }
                    else diag2_up = 0;
                    b++;
                }
            }
            return null;
        }
        private void SwitchPlayers()
        {
            activePlayer = (activePlayer == players[0]) ? players[1] : players[0];
        }
        public Player GetCurrentActivePlayer()
        {
            return activePlayer;
        }
        public bool IsFieldFilled()
        {
            return squareCount == filled;
        }
        public void Reset()
        {
            ResetField();
            ResetPlayers();
        }
        private void ResetField()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i,j].Fill(null);
                }
            }
            filled = 0;
        }
    }
}
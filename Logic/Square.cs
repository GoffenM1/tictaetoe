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
    public class Square
    {
        private Player player = null;
        private int valueX = 0;
        private int valueO = 0;

        public void Fill(Player player)
        {
            this.player = player;
        }

        public bool IsFilled()
        {
            if (player != null)
            {
                return true;
            }
            return false;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public Player getPlayer()
        {
            return player;
        }

        public int getValueX()
        {
            return valueX;
        }

        public void setValueX(int value)
        {
            this.valueX = value;
        }

        public int getValueO()
        {
            return valueO;
        }

        public void setValueO(int valueO)
        {
            this.valueO = valueO;
        }

        public void addValueO(int valueO)
        {
            this.valueO += valueO;
        }

        public void addValueX(int valueX)
        {
            this.valueX += valueX;
        }

    }
}
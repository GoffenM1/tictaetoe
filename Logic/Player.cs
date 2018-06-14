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
    public class Player
    {
        private String name;

        public Player(String name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
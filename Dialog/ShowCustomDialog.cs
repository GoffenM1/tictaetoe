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

using TicTacToe.Dialog;
namespace TicTacToe
{
    public class ShowCustomDialog
    {
        public void Show(MainScreenActivity obj, string textMessage)
        {          
            FragmentTransaction ft = obj.FragmentManager.BeginTransaction();
            //Remove fragment else it will crash as it is already added to backstack           
            Fragment prev = obj.FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);
            // Create and show the dialog.
            CustomDialogFragment newFragment = CustomDialogFragment.NewInstance(null, textMessage);
            //Add fragment
            newFragment.SetListener(obj);
            newFragment.Show(ft, "dialog");
        }
    }
}
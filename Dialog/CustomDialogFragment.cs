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
using Java.Lang;

namespace TicTacToe
{
    class CustomDialogFragment : DialogFragment
    {
        private IDialogListener _mListener;
        private static string _textMessage;
        public static CustomDialogFragment NewInstance(Bundle bundle, string textMessage)
        {
            _textMessage = textMessage;
            CustomDialogFragment fragment = new CustomDialogFragment();
            fragment.Arguments = bundle;           
            return fragment;
        }
        public void SetListener(IDialogListener listener)
        {
            _mListener = listener;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Dialog.SetCanceledOnTouchOutside(false);
            this.Dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            View view = inflater.Inflate(Resource.Layout.DialogWindow, container, false);
            Button buttonOk = view.FindViewById<Button>(Resource.Id.btnOk);
            Button buttonClose = view.FindViewById<Button>(Resource.Id.btnClose);
            TextView textvwMessage = view.FindViewById<TextView>(Resource.Id.txtvwMessage);
            textvwMessage.Text = _textMessage;
            buttonOk.Click += delegate {
                _mListener.FinishDialogListener("New");
                Dismiss();
               // Toast.MakeText(Activity, "Dialog fragment dismissed!", ToastLength.Short).Show();
            };
            buttonClose.Click += delegate {
                MainScreenActivity.MainScrActivity.Finish();
            };
            return view;
        }
    }
}
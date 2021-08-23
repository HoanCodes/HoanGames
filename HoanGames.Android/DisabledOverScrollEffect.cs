using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoanGames.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("HoanGames")]
[assembly: ExportEffect(typeof(DisabledOverScrollEffect), "DisabledOverScroll")]
namespace HoanGames.Droid
{
    class DisabledOverScrollEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                Control.OverScrollMode = OverScrollMode.Never;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;

namespace App.Android
{
	[Application]
	public class Global : Application {
		public IList<Post> Posts { get; set;}

//		public string WalkrDbPath { get; set;}

		public static Global Current { get; private set; }

		public CommonClient client { get; set;}


		public Global(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
			: base(handle, transfer) {
			Current = this;
		}

		public override void OnCreate()
		{
			base.OnCreate();

//			var sqliteFilename = "Walkr.db3";
//			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//			WalkrDbPath = Path.Combine(libraryPath, sqliteFilename);
		}
	}
}


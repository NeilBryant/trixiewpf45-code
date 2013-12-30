/****************************************************************************
	Trixie - Tricks for IE
	アプリケーションクラス(COM登録)

	セルフレジスタなのでregasmしなくてらくちん

	Copyright (C) 2013 Mizutama(水玉 ◆qHK1vdR8FRIm)
	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
****************************************************************************/
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Trixie
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
		/// <summary>
		/// バージョン文字列
		/// オプションダイアログで見る事ができる
		/// </summary>
		public static string UserAgent
		{
			get
			{
				return string.Format( "{0}/{1}.{2} ({3}-{4} {5})"
						, VerInfo.Name
						, VerInfo.Version.Major , VerInfo.Version.Minor
						, VerInfo.DevPhase , VerInfo.Config
						, VerInfo.Version );
			}
		}

		/// <summary>
		/// セルフレジスタ
		/// アプリケーションとしてはここだけ実行して終わる
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStartup( object sender , StartupEventArgs e )
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			RegistrationServices reg = new RegistrationServices();

			var args = Environment.GetCommandLineArgs();
			if ( args.Length > 2 )
			{
				if ( args[1].Equals( "/u" ) )
				{
					// Unregister with COM
					reg.UnregisterAssembly( asm );
					Console.Write( "Trixeの登録を解除しました。" );
				}
				else if ( args[1].Equals( "/r" ) )
				{
					// Register with COM
					reg.RegisterAssembly( asm , AssemblyRegistrationFlags.SetCodeBase );
					Console.Write( "Trixieを登録しました。" );
				}
				Shutdown();
				return;
			}

			Type installed = Type.GetTypeFromProgID( "Trixie.Bho" );
			if ( installed != null )
			{
				var result = MessageBox.Show( "Trixieの登録を解除します" , "Trixie" , MessageBoxButton.YesNo );
				if ( result == MessageBoxResult.Yes )
				{
					// Unregister with COM
					if ( reg.UnregisterAssembly( asm ) )
					{
						MessageBox.Show( "Trixieの登録解除をしました" );
					}
					else
					{
						MessageBox.Show( "Trixieの登録解除に失敗しました" );
					}
				}
			}
			else
			{
				var result = MessageBox.Show( "Trixieを登録します" , "Trixie" , MessageBoxButton.YesNo );
				if ( result == MessageBoxResult.Yes )
				{
					// Register with COM
					if ( reg.RegisterAssembly( asm , AssemblyRegistrationFlags.SetCodeBase ) )
					{
						MessageBox.Show( "Trixieの登録をしました" );
					}
					else
					{
						MessageBox.Show( "Trixieの登録に失敗しました" );
					}
				}
			}
			Shutdown();
		}
	}
}

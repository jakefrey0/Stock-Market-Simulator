/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 1/4/2021
 * Time: 2:22 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace StockMarketSimulator {
	
	/// <summary>
	/// The class with program entry point.
	/// </summary>
	internal sealed class Program {
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main (String[] args) {
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
			
		}
		
	}
	
}
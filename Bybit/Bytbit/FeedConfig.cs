using UAPI;

namespace Bytbit
{
	/// <summary>
	/// The FeedConfig class is required for registration of your Feed in UA.
	/// Without it, your Feed will not be able to be configured for use.
	/// 
	/// It defines items such as the unique name of the Feed and Menu Layout.
	/// 
	/// In general you will not need to alter this.
	/// </summary>
	public static class FeedConfig
	{
		/// <summary>The unique FeedName is BYTBIT</summary>		
		public static string FeedName() { return "BYTBIT"; }
		/// <summary>The readable FeedName is Bytbit</summary>		
		public static string Name() { return "Bytbit"; }
		/// <summary>The Data Source Description is Bytbit Data</summary>		
		public static string Source() { return "Bytbit Data"; }
		/// <summary>The Module output is BYTBIT.dll</summary>		
		public static string Module() { return "BYTBIT.dll"; }
		/// <summary>Appears on the SystemTray Menu under the Data Feeds Sub Menu</summary>
		public static string TrayMenu() { return Constants.Menu[Constants.MenuKind.DataFeeds]; }
		/// <summary>Menu Level</summary>		
		public static int Level() { return 1; }
		/// <summary>The Feed Configuration Area is Data Feeds</summary>		
		public static int Group() { return (int)Constants.ConfigurationGroup.DataFeeds; }
		/// <summary>Url Link for Feed Information during Feed Configuration</summary>		
		public static string Url() { return "http://www.updata.co.uk"; }
		/// <summary>Url Display Text for Feed Information during Configuration</summary>	
		public static string UrlMessage() { return "click here for more information"; }

		/// <summary>Feed Configuration Ini File Entry Header</summary>	
		public static string[] IniHeader()
		{
			string[] sRet = new string[]
			{
				$"menu={TrayMenu()}",
				$"menu-name={Name()}"
			};
			return sRet;
		}

		/// <summary>Feed Configuration Ini File Entry</summary>	
		public static string[] IniEntry()
		{
			return new string[]
			{
				$"NAME={FeedName()}",
				$"Source={Name()}",
				"TYPE=EXE",
				$"TITLE=IOHOST-{FeedName()}",
				"MODULE=iohost.exe",
				$"CACHE={FeedName()}",
				$"FolderId={FeedName()},",
				"SUPPORT-CLOSEONEXIT=1",
				"API=1",
				"API-TYPE=0",
				$"APIMODULE={Module()}",
				$"menu={TrayMenu()}",
				$"menu-name={Name()}",
				"SUPPORT-ADDFAVOURITES=1"
			};
		}
	}
}

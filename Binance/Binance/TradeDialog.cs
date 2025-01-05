using System;
using System.Windows.Forms;
using UAPI;

namespace Binance
{
	/// <summary>
	/// When UA triggers a trade to be subitted, your feed's <see cref="UapiTradeDialog"/> dialog is displayed
	/// 
	/// It is required that your Custom Order Entry dialog inherits from <see cref="UapiTradeDialog"/>
	/// This works in the same way as the <see cref="UapiInstrumentSelector"/> where
	/// an Opened method is provided and a function to submit.
	/// </summary>
	public partial class TradeDialog : UapiTradeDialog
	{
		public TradeDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// As the Trade Dialog is displayed it gives your control a chance to populate its
		/// controls based on the Instrument Identifier contained in the <see cref="Order.InId"/>.
		/// </summary>
		/// <param name="TradeAction">This is the mode of the dialog, most of the time it is <see cref="Order.ActionKind.Submit"/></param>
		/// <param name="DataSourceDescription">A description of the data provider (chart data) that caused your Trade Dialog to open, most likely from an Alert</param>
		/// <param name="Order">Contains the Instrument Identifier for which to submit an order, and the details of a pre-populated order</param>
		public override void Opened(Order.ActionKind TradeAction, string DataSourceDescription, Order Order)
		{
			Log.Dbg("Opened Trade Dialog [{0}] {1}", TradeAction, Order?.InId);

			if (Order != null)
			{
				lbInstrument.Text = Order.InId.CodeName;
				lbDataProvider.Text = DataSourceDescription;
				lbPlatform.Text = Binance.I.Source;
			}
		}

		/*************************************************************************************************\

		Below is a working example of submitting an order where upon calling Submit()
		your feed's override for SubmitOrder(RequestParameters, Order) will be called.

		\*************************************************************************************************/

		private void btSubmitBuy_Click(object sender, EventArgs e)
		{
			// Prepare an Order to be submitted based on user input
			Order Buy = new Order()
			{
				Side = Order.SideKind.Buy,
				Price = (double)nudOrderPrice.Value,
				Amount = (double)nudOrderAmount.Value
			};

			// Submit the Buy Order as above
			SubmitOrder(Buy);
		}

		private void btSubmitSell_Click(object sender, EventArgs e)
		{
			// Prepare an Order to be submitted based on user input
			Order Sell = new Order()
			{
				Side = Order.SideKind.Sell,
				Price = (double)nudOrderPrice.Value,
				Amount = (double)nudOrderAmount.Value
			};

			// Submit the Sell Order as above
			SubmitOrder(Sell);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UAPI;

namespace Bytbit
{
	/// <summary>
	/// The Instrument Selector Control is displayed in UA when the user needs to select an instrument
	/// for data display.
	/// 
	/// It is required that your Selector control inherits from <see cref="UapiInstrumentSelector"/>
	/// There is an override <see cref="Opened(SelectionKind, TimeFrame)"/> which is called each time
	/// your Selector is displayed in UA, it lets your Selector know which selection mode it is opened for.
	/// 
	/// This could be if the user is selecting an Instrument, a Chart, or a Forward Curve.
	/// Your control will need to adjust how and what is selected based on the mode, but most of the time
	/// it will be for returning a single <see cref="InstrumentIdentifier"/> according to the user's selection.
	/// 
	/// Once an <see cref="InstrumentIdentifier"/> or <see cref="InstrumentFuturesIdentifier"/> has been constructed
	/// and ready to return based on the user's selection, return it to UA with one of the variants of
	/// <see cref="UapiInstrumentSelector.SelectInstruments(InstrumentIdentifier[], Currency, TimeZoneInfo)"/>
	/// <see cref="UapiInstrumentSelector.SelectInstruments(InstrumentFuturesIdentifier[], Currency, TimeZoneInfo)"/>
	/// <see cref="UapiInstrumentSelector.SelectTimeSeries(InstrumentIdentifier[], TimeSeriesParameters, Currency, TimeZoneInfo)"/>
	/// <see cref="UapiInstrumentSelector.SelectChainTimeSeries(InstrumentFuturesIdentifier[], ChainTimeSeriesParameters, Currency, TimeZoneInfo)"/>
	/// 
	/// or if the user cancels selection
	/// <see cref="UapiInstrumentSelector.CancelSelect(string)"/>
	/// </summary>
	public partial class Selector : UapiInstrumentSelector
	{
		public Selector()
		{
			InitializeComponent();
		}

		/// <summary>
		/// UA has displayed your Instrument Selector with the Selection Mode as per <paramref name="SelectionMode"/>
		/// </summary>
		/// <param name="SelectionMode">The Mode for which the Selector has been displayed</param>
		/// <param name="DefaultTimeFrame">Not in use. For future implemention</param>
		protected override void Opened(SelectionKind SelectionMode, TimeFrame DefaultTimeFrame)
		{
			/*************************************************************************************************\

			Alter the controls of your selector to allow the user to choose the appropriate instrument.

			\*************************************************************************************************/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btSelect_Click(object sender, EventArgs e)
		{
			/*************************************************************************************************\

			Return your built instrument identifier(s) to UA for data requests.			

			\*************************************************************************************************/

			var SelectedInId = InstrumentIdentifier.Build(FeedConfig.FeedName(), tbInstrument.Text);

			// this will return your instruments and close the Instrument Selector display in UA.
			SelectInstruments(new InstrumentIdentifier[] { SelectedInId });

			/*************************************************************************************************\

			Some examples of variations of Instrument Identifier contruction is provided.
			All the information contained in an Instrument Identifier is able to be retrieved 
			in your Feed when UA makes data requests with it.

			\*************************************************************************************************/

			// Custom Meta Data included as part of the identifier
			var MetaData = new Dictionary<string, string>()
			{
				{ "MyMetaId", "TheMetaIdValue" },
			};
			var MetaInId = InstrumentIdentifier.Build(FeedConfig.FeedName(), tbInstrument.Text, MetaData);

			// A Currency included in identification
			/// <see cref="Currency"/> for reference on using the Currency construct.
			var CurrencyInId = InstrumentIdentifier.Build(FeedConfig.FeedName(), tbInstrument.Text, "USD");

			// A Futures identifier with contract definition and expiry date at the end of 2030
			var ExpiryInFuId = InstrumentFuturesIdentifier.Build(FeedConfig.FeedName(), tbInstrument.Text,
				InstrumentFuturesIdentifier.IdentifierKind.Contract, 0, new DateTime(2030, 12, 31));

			// A logical Futures identifier for the 'Front + 1' contract
			var FrontPlusOneInFuId = InstrumentFuturesIdentifier.Build(FeedConfig.FeedName(), tbInstrument.Text,
				InstrumentFuturesIdentifier.IdentifierKind.Front, 1);
		}
	}
}

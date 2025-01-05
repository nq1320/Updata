using System;
using System.Collections.Generic;
using UAPI;

namespace Ctrader
{
	/// <summary>
	/// Welcome to the Updata API (UAPI), the comments provided in this file forms the majority of the Documentation of the API.
	/// 
	/// With this API you can return static and realtime data to Updata Analytics (UA).
	/// As the user interacts with UA, depending on the required display, UA will make data requests to your feed
	/// which calls one of the overrides of the UAPI.UAPI class below.
	/// 
	/// All data returned to UA is cached, you would only need to cache specialized data relevent to your feed.
	/// TimeSeries data aggregations are also handled entirely by UA, so there is also no need for aggregating TimeSeries in your feed.
	/// 
	/// Most of the data return can be done Synchronously or Asynchronously. UA will allow for more time for
	/// Asynchronous data return before timing out, while Synchronous data return is less forgiving as it blocks
	/// other data requests so time out is quicker. Methods of synchronous and asynchronous data return are explained in the comments below.
	/// 
	/// 3 Dialogs are called upon from UA, the Instrument Selector, the Trading Dialog, and a Feed Settings Dialog:
	/// 
	/// [required] The Instrument Selector is inherited by your Feed from the <see cref="UapiInstrumentSelector"/> control and is to be
	/// created for the user to select Instruments to return to UA for making data requests. Its output is
	/// either <see cref="InstrumentIdentifier"/> or <see cref="InstrumentFuturesIdentifier"/> which are unique identifiers for instruments in UA.
	/// These are sent back to your feed in most of the data request functions below.
	/// 
	/// [optional] The Trading Dialog is inherited by your Feed from the <see cref="UapiOrderActions"/> Control.
	/// UA displays the Feed Trading Dialog when requesting to submit an order, if your Feed Supports Trading.
	/// When your feed's trading dialog submits an order, the <see cref="SubmitOrder(RequestParameters, Order, ref Order.Result)"/> is called
	/// to perform the actual work in submitting the order and returning its result to UA.
	/// 
	/// [required] A settings dialog is to be shown for your Feed when UA requests it via <see cref="SettingsDialog"/>.
	/// This dialog is hosted and managed entirely by your Feed, as compared to the Instrument Selector and the Trading Dialog which are inherited controls.
	/// 
	/// The basic running of a Feed is as follows:
	///  1. UA starts up and runs your Feed in its own process - the Initialize function is called <see cref="Initialize(out InitializeParameters)"/>
	///  2. UA registers your running Feed and connects to it calling the ClientConnected function <see cref="ClientConnected"/>
	///  3. UA checks if your feed is ready to return data calling the RequestStatus where your Feed returns whether or not it can receive data requests <see cref="RequestStatus"/>
	///  4. Once your feed has communicated to UA that it is ready to receive data requests (this can be done at any point asynchronously) it will be requested to return data as required.
	///  5. UA displays your instrument selector for the user to select an instrument where your Feed builds an <see cref="InstrumentIdentifier"/> to return
	///  6. UA makes data requests with the <see cref="InstrumentIdentifier"/> and waits for your Feed to return the data or a response notifying when it is unable to return the data
	///  7. With each data returned UA caches it, displays it and in the case of TimeSeries data it will make subsequent requests to get more.
	///  8. Your Feed may subscribe to realtime updates and these can be pushed to UA.
	///  9. When an Instrument's data is no longer being used in UA or if a Realtime price display or TimeSeries display is closed, the <see cref="CloseInstrument(InstrumentIdentifier, UpdateFlags)"/> is called
	/// 10. When UA is shutdown your Feed is notified via the <see cref="Shutdown"/> override
	/// 11. Finally as your process is terminating the <see cref="UAPI.UAPI.Terminating"/> override is called.
	/// 
	/// The functions and comments that follow document the typical and optional events and procedures for a Feed.
	/// </summary>
	public class Ctrader : UAPI.UAPI
	{
		/// <summary>
		/// The instance of this Feed used by the Selector, Settings and Order Entry dialogs to access methods and fields for display
		/// See <see cref="UapiInstrumentSelector"/>, <see cref="Settings"/> and <see cref="UapiOrderActions"/>.
		/// </summary>
		public static Ctrader I { get; private set; } = null;

		/// <summary>
		/// This is populated at runtime with the Assembly Version
		/// </summary>
		public string Version { get; private set; } = "";

		/// <summary>
		/// Sent to Updata Analytics (UA) this description of the Data Source of this Feed
		/// </summary>
		public override string Source { get { return FeedConfig.Source(); } }

		/// <summary>
		/// A Settings Dialog to show when UA requests to see the feed settings.
		/// This dialog is displayed in the <see cref="Ctrader.SettingsDialog"/> override below. 
		/// </summary>
		Settings Settings = new Settings();

		#region Setup

		public Ctrader() : base(FeedConfig.FeedName())
		{
			I = this;

			try
			{
				// Query the version of the Assembly at runtime and save
				var VersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
				Version = VersionInfo.FileVersion;
			}
			catch (Exception e)
			{
				Log.Err("Failed to retrieve version info - {0}", e.ToString());
			}

			/*************************************************************************************************\

			When Debugging, force the Log Level to be Verbose
			
			**Note: When a debugger is attached to Visual Studio the UAPI.Log only
			outputs to the Visual Studio Output and it does not write any log files.

			\*************************************************************************************************/
#if DEBUG
			Log.Level = LogLevel.Verbose;
#else
			// Recommended to load/save this from a settings file
			Log.Level = LogLevel.Info;
#endif

			Log.Dbg($"Starting {FeedConfig.Name()} - Version {Version}");
		}
		#endregion

		#region Startup

		/// <summary>
		/// When your Feed is started this is called to convey configuration information to UA.
		/// It is a good place to perform external Feed Initialization and Connection Setup.
		/// </summary>
		/// <param name="Params">Parameters sent back to UA defining how your Feed interacts</param>
		/// <returns>Return a response message if initialization fails, otherwise return true</returns>
		public override Response Initialize(out InitializeParameters Params)
		{
			Log.Vbs("Initialize");

			// Populate the parameters with your Feed's requirements
			Params = new InitializeParameters()
			{
				// Configuration for how your Feed interacts with Data
				Config = new Configuration()
				{
					// This is displayed on Charts to describe the Data Source of your Feed
					Description = Source,
					// When intraday charts are updated, the TimeStamp of the candle is taken at either the start or the end of the candle plot
					CandleTimeStampStyle = CandleTimeStampStyleKind.EndOfCandle,
					// Set the Close value of the latest candle on each chart to match the latest Quote price returned from your feed for the Instrument
					AdjustFinalCandleToQuote = false,
					// Set this flag to making trading available to the instruments of this feed in UA
					IsTradable = true
				},
				// Create an object to represent which TimeFrames are supported - this is populated below
				TimeFrameSupported = new TimeFrameSupport(),
				// Pass your Instrument Selection Control to UA to display on the Instrument Selector
				InstrumentSelectorControl = new Selector(),
				// Pass an instance of your Trade Dialog to UA so it may be displayed for Trading
				TradeDialog = new TradeDialog(),
				// Let UA know to call the public override void OnDailyRefresh(), defined in the #region Maintenance, at 1AM UTC
				DailyRefreshTimeUTC = new TimeSpan(1, 0, 0),
			};

			// Populate which TimeFrames are supported by your Feed
			// Below is an example of Supporing Daily and 5 Minute TimeFrames only.
			Params.TimeFrameSupported.Support(
				new TimeFrame[]
				{
					TimeFrame.IntervalKind.Day,
					new TimeFrame(TimeFrame.IntervalKind.Minute, 5)
				});

			/*************************************************************************************************\

			At this point it is recommended that any external Feed setup be done

			Along with reading any Settings or Configuration for your Feed
			
			Usually return from this function with the result of Feed Setup
			Let UA know that initialization succeeded. If it failed, you could return a message like this
			
			return "Feed Initialization Failed - Access Denied."

			\*************************************************************************************************/

			// Let UA know that Feed Initialization succeeded
			return true;
		}

		/// <summary>
		/// When UA connects to your Feed running in this process this Function Notifies.
		/// It is recommended to return the Feed Data Request Status to UA at this point as shown
		/// in the example comments below
		/// </summary>
		public override void ClientConnected()
		{
			Log.Vbs("Connected to Updata");

			/*************************************************************************************************\

			Let UA know that the Feed is ready to receive requests and return data.
			UA will not send any data requests to the Feed until this status has been sent through as AbleToRequest.
			
			raise_OnUpdateRequestStatus(true, $"{FeedConfig.Name()} is connected and ready to return data.");

			\*************************************************************************************************/
		}

		/// <summary>
		/// UA calls this as it connects to your Feed to determine if your Feed is ready to return data for requests.
		/// </summary>
		/// <returns>A Command Status Update to let UA know if the Feed is able to request data</returns>
		public override Command RequestStatus()
		{
			/*************************************************************************************************\
			
			If your Feed was not ready to return data you would return something similar to the code below
			
			return new Command(Command.Kind.UnableToRequest, $"{FeedConfig.Name()} is still initializing.");

			\*************************************************************************************************/

			return new Command(Command.Kind.AbleToRequest, $"{FeedConfig.Name()} is ready to return data.");
		}

		#endregion

		#region Data Requests

		/// <summary>
		/// This is called when a single instrument is required to be displayed for its latest prices and display details
		/// Usually the first step in data request for an Instrument as it sets up things like decimal places,
		/// the descriptive name of the instrument, trading information, timezone, and other necessary details.
		/// 
		/// Your feed should subscribe to realtime price updates for the instrument here.
		/// <see cref="CloseInstrument(InstrumentIdentifier, UpdateFlags)"/> where the Instrument should be unsubscribed
		/// and clear out of any resources related to it.
		/// </summary>
		public override Response OpenInstrument(RequestParameters Request, InstrumentIdentifier InId, ref InstrumentInfo Info)
		{
			/************************************************************************************************\

			To return the InstrumentInfo requested for the Instrument there are 2 options:
			Request and return it Synchronously inline here in the function, or
			Asynchronously, which is prefferred, where you would process requesting and returning the Info in another thread.
			Both scenarios are explained below.
			
			The Info Parameter will contain any cached information from previous sessions. This can alleviate having
			to request details such as Description, Exchange and AssetType (amongst others) that will not likely change.
			
			Checking the Info.InfoDateTime will provide the DateTime of when the last Info was saved, and the
			Info.QuoteDateTime will provide when the latest quote was last saved.
			*Note that the Info.Quote will not be populated with cached Price values, only the DateTime of when they were 
			last saved, as it is assumed that these will always be requested, or if the instrument 
			is subscribed to realtime updates, these values will be on hand to return here right away.

			Returning Synchronously 
			You would make your feed requests for the Instrument details and realtime price subscription here.
			Then update the Info Parameter with the Quote and Details, and return true from this function.
			*Note that you cannot create a new InstrumentInfo to return

			Returning Asynchronously
			The Request Parameter contains the AsyncId of this request which is the Id to be used when returning the data later.
			You would build an InstrumentInfo object populated with the Details and Quote in your own thread,
			then return that to UA with the raise_OnInstrumentInfoFinished(AsyncId, InstrumentInfo). The AsyncId is
			passed back in the raise function to identify that it originated from this request.
			Finally you would need to let UA know that you are returning the data Asynchronously
			by returning Response.Kind.Async from this function.
			
			Should any asynchronous data request fail, the raise_OnRequestFinished(long AsyncId, Response Response) must be
			called to let UA know that the request is finished and to display or log the result if it failed.

			The subsequent Data Request Overrides below all operate in the same way allowing for a synchronous or asynchronous return.
			*Note if you try to use both, the asynchronous return will be ignored as the request is no longer 'alive' once
			it has received the data requested.

			\************************************************************************************************/

			return true;
		}

		/// <summary>
		/// This is the <see cref="OpenInstrument(RequestParameters, InstrumentIdentifier, ref InstrumentInfo)"/> 
		/// supporting multiple simultaneous instrument details/quote/subscription request. 
		/// This is usually called from a Watchlist in UA to get multiple instruments at once, saving potentially hundreds of
		/// individual OpenInstrument requests.
		/// 
		/// This data cannot be returned Synchronously. The Response returned from the function is merely an indicator
		/// as to whether or not the entire goup of instruments can be requested or not.
		/// </summary>
		public override Response OpenInstruments(Tuple<RequestParameters, InstrumentIdentifier, InstrumentInfo>[] OpenRequests, ConstituentsKind ConstituentsKind)
		{
			/************************************************************************************************\

			Refer to the OpenInstrument function above for more details about the contained parameters of OpenRequests;
			RequestParameters, InstrumentIdentifier, and InstrumentInfo.
			
			The OpenRequests Array Parameter groups individual OpenInstrument requests in Tuples.
			Each Tuple Item will contain the RequestParameters for the AsyncId, the Instrument to be requested
			and the InstrumentInfo will contain any cached Info from previous sessions.
			
			With each Tuple in OpenRequests, call the raise_OnInstrumentInfoFinished(AsyncId, InstrumentInfo)
			function using the AsyncId from the RequestParameters and your created InstrumentInfo object with the values.
			
			Most of the time you will return true from this function, unless none of the requested instruments can be processed,
			for instance, if your Feed is offline.

			\************************************************************************************************/

			return true;
		}

		/// <summary>
		/// When a Chart is opened in UA, the OpenInstrumentWithTimeSeries request is called upon.
		/// All of the Instrument Details, Latest Quote, Price Subscription for Quotes and Chart Updates, and
		/// the latest TimeSeries history data is required.
		/// 
		/// The request is a combination of <see cref="OpenInstrument(RequestParameters, InstrumentIdentifier, ref InstrumentInfo)"/> and 
		/// <see cref="RequestTimeSeries(RequestParameters, InstrumentIdentifier, TimeSeriesParameters, ref ITimeSeriesValues)"/>.
		/// 
		/// The Parameters include both RequestParameters for the OpenInstrument and TimeSeriesRequest, each with their own
		/// AsyncId for Asynchronous return of each.
		/// As there are essentially two requests happening at once, the Response for the OpenInstrument request is the one returned
		/// from the function because it is usually going to be a quicker request with a small amount of data to be returned.
		/// The Response to the TimeSeries Request is passed to the Function as <paramref name="TimeSeriesResponse"/> which must be
		/// set before returning from the function. For instance you could return the InstrumentInfo synchronously and the 
		/// TimeSeries asynchronously by setting the TimeSeriesResponse to <see cref="Response.Kind.Async"/> and returning true.
		/// </summary>
		/// <param name="OpenInstrumentRequest">Request Parameters including the AsyncId for the Open Instrument Operation</param>
		/// <param name="InId">The Instrument for which to open a chart</param>
		/// <param name="TimeSeriesRequest">Request Parameters for the TimeSeries Request including the AsyncId for it</param>
		/// <param name="Params">The parameters of the TimeSeries Requested, such as TimeFrame</param>
		/// <param name="Info">Populated with the cached Info and DateTime of the last saved Quote</param>
		/// <param name="ReturnValues">When returning the TimeSeries Synchronously, add them to this container</param>
		/// <param name="TimeSeriesResponse">
		/// Set the response for the TimeSeries part of this Request, 
		/// set it to <see cref="Response.Kind.Async"/> when returning the TimeSeries Asynchronously
		/// </param>
		/// <returns>Return the Response for the OpenInstrument part of the compound request</returns>
		public override Response OpenInstrumentWithTimeSeries(RequestParameters OpenInstrumentRequest, InstrumentIdentifier InId, RequestParameters TimeSeriesRequest, TimeSeriesParameters Params, ref InstrumentInfo Info, ref ITimeSeriesValues ReturnValues, out Response TimeSeriesResponse)
		{
			/************************************************************************************************\

			The TimeSeriesParameters Params contains information about what kind of TimeSeries is to be requested for the chart.
			This denotes whether the TimeSeries being requested is for a specific DateTime Range, how many points to request,
			whether this is filling a Gap in the Cache or is requesting more History at the end of the Cache.
			For the OpenInstrumentWithTimeSeries function, however, the TimeSeriesParams will always be asking for the latest
			TimeSeries points, it will be a 'Front' Fill meaning the latest TimeSeries up to this instance in time.
			
			When opening a chart we want to get a small amount of the latest TimeSeries in order to display the chart quickly,
			then the RequestTimeSeries function is called repeatedly to fill out the TimeSeries history which is pushed onto the chart
			while its displayed.
			
			The TimeSeriesParameters will also contain the TimeFrame for the TimeSeries to be requested.
			Aggregation is handled by UA, and the TimeFrame requested will always be one of those set in the Initialize function
			for the InitializeParameters. UA will work out the best TimeFrame to use for each request and aggregate if your
			feed does not support the TimeFrame natively. This can only be done for TimeFrames of lower granularity than those supported.
			For example if your feed supports only as low as a 5 Minute TimeFrame, UA will not be able to display a 1 Minute chart, but
			it will be able to support all intervals that are multiples of 5 Minutes up to Daily and beyond..
			
			When returning TimeSeries Points synchronously, add them to the ReturnValues parameter and set the
			TimeSeriesResponse to true. Otherwise to return asynchronously later, set the TimeSeriesResponse to Response.Kind.Async and
			return a new ITimeSeriesValues object (created from Params.TimeFrame.CreateValues()) with
			raise_OnRequestTimeSeriesFinished(TimeSeriesRequest.AsyncId, Values).
			Use the raise_OnRequestFinished(long AsyncId, Response Response) if the data request fails to let UA know that the 
			request is complete - this will display your response message and will prevent the App in UA from waiting until the request times out.
			
			The TimeSeriesParameters are covered in more detail in the 
			RequestTimeSeries(RequestParameters Request, InstrumentIdentifier InId, TimeSeriesParameters Params, ref ITimeSeriesValues ReturnValues)
			function below.
			
			Request the Instrument's latest Quote and Details, along with subscribing to realtime quote and chart updates.
			Either request and populate the Info parameter with the Instrument (Identified by the InId parameter)
			Quote and Details values and return true to this function, or return the Info asynchronously.
			
			For Asynchronous InstrumentInfo return, use the raise_OnInstrumentInfoFinished with the OpenInstrumentRequest.AsyncId
			and return Response.Kind.Async to this function.
			See the OpenInstrument function above for more details on returning InstrumentInfo.

			\************************************************************************************************/

			TimeSeriesResponse = Response.Kind.Async;
			return true;
		}

		/// <summary>
		/// When opening a Chart in UA, the <see cref="OpenInstrumentWithTimeSeries(RequestParameters, InstrumentIdentifier, RequestParameters, TimeSeriesParameters, ref InstrumentInfo, ref ITimeSeriesValues, out Response)"/>
		/// is called to request the Instrument Quote, Details and initial TimeSeries.
		/// But, sometimes when other Apps, such as the Watchlist are open for the same instrument, the OpenInstrumentWithTimeSeries request
		/// is not needed and either a RequestTimeSeries is called or just the OpenInstrument request.
		/// When that happens, this request is made in order to make sure that Chart Price Updates are subscribed or at least the feed is notified
		/// that Chart Updates are required and the Instrument should be subscribed for them here.
		/// </summary>
		/// <param name="TimeFrame">Chart TimeFrame making the Request for TimeSeries Updates</param>
		/// <returns></returns>
		public override Response OpenTimeSeries(RequestParameters Request, InstrumentIdentifier InId, TimeFrame TimeFrame)
		{
			/************************************************************************************************\
			
			Most of the time this will not be needed, but for feeds that have separate Quote and Chart update subscriptions
			you should subscribe to Chart updates here and return the result.

			There is no corresponding asynchronous function to return the Result of subscription 
			so returning Response.Kind.Async here is meaningless.
			
			\************************************************************************************************/

			return true;
		}

		/// <summary>
		/// Requesting static TimeSeries is gained via RequestTimeSeries. This is purely for TimeSeries data return.
		/// The <see cref="TimeSeriesParameters"/> contains a definition of the TimeSeries to be requested.
		/// Your Feed will base its TimeSeries request on the values in the TimeSeriesParameters.
		/// These parameters are able to be configured by the user and saved in a configuration file for your Feed.
		/// 
		/// For synchronous TimeSeries Return:
		/// ReturnValues can be populated with either <see cref="Tick"/> for Tick TimeSeries, 
		/// or <see cref="DataPoint"/> objects for Candles, and then return true to the function.
		/// 
		/// For asyncronous TimeSeries Return (recommended):
		/// Return <see cref="Response.Kind.Async"/> to this function and then in your own thread
		/// once you have the TimeSeries data, create an <see cref="ITimeSeriesValues"/> container, using the
		/// <see cref="TimeSeriesParameters.TimeFrame"/> contained in <paramref name="Params"/> to call
		/// <see cref="TimeFrame.CreateValues(TimeZoneInfo, AggregationParameters)"/>.
		/// 
		/// Populate your ITimeSeriesValues container with either DataPoints or Ticks depending on the TimeFrame
		/// and return it with <see cref="UAPI.UAPI.raise_OnRequestTimeSeriesFinished(long, ITimeSeriesValues)"/>
		/// providing the <see cref="RequestParameters.AsyncId"/> from <paramref name="Request"/>.
		/// 
		/// If you cannot get the values, call <see cref="UAPI.UAPI.raise_OnRequestFinished(long, Response)"/>
		/// with your reponse message to display in UA, again providing the AsyncId from the Request Parameter provided here.
		/// 
		/// When returning <see cref="Response.Kind.Async"/> to these functions, UA will wait for your raise_On... response with the
		/// AsyncId provided. After a user configured amount of time, the request will Time Out which is not optimal, as the user will
		/// see a loading status for the duration until a TimeOut message is displayed.
		/// 
		/// For charts, this function is called repeatedly to cache more history for the instrument, each time moving the
		/// <see cref="TimeSeriesParameters.UtcDateTimeRange"/> further back in time.
		/// 
		/// The Cache is requested backwards from the latest points back in time to the oldest.
		/// If enough time has passed since the TimeSeries was last requested, and the front of the chart is now requested,
		/// there may be a gap in the cache between where it left off and the latest values. This is an expected scenario and
		/// preferred over trying to request a larger amount of history in one go, thereby delaying showing the chart with the latest values
		/// quickly.
		/// 
		/// The RequestTimeSeries function is repeatedly called with differernt TimeSeriesParameters to define where and how much
		/// data to request. The CacheMode is either Front, Gap or Back to denote where the values fit in relation to what is already cached.
		/// The RequestType is either Latest or DateTimeRange meaning that either UA would like to just get the latest values however few or many,
		/// ot it is trying to fill a range of time series where speed is less of a priority and getting the data in fewer requests is more important.
		/// 
		/// The UtcDateTimeRange will contain no values for Front requests, it will contain the DateTime range of the entire Gap, or it
		/// will have an indication of from where to request from and up to the end of the cache for Back requests.
		/// It is always UTC so you will need to convert this to the TimeZone of your Feed.
		/// 
		/// Typically the OpenInstrumentWithTimeSeries function will be called with the Front (update to get the latest TimeSeries History)
		/// specification for the <see cref="TimeSeriesParameters.CacheMode"/>, Update specified for the
		/// <see cref="TimeSeriesParameters.RequestMode"/> and Latest for the <see cref="TimeSeriesParameters.RequestType"/>.
		/// The <see cref="TimeSeriesParameters.UtcDateTimeRange"/> will have no value as it is only needing the latest values.
		/// If UA already has Quote and Price Updates for the instrument, then the RequestTimeSeries will be called to
		/// update or request the latest TimeSeries History the TimeSeriesParameters specified as above.
		/// 
		/// Once the Chart is updated with the latest TimeSeries and it is displaying, subsequent requests are made to fill out the history.
		/// If there is a Gap between where the latest TimeSeries was returned and the existing Cache then the Gap mode will be requested here
		/// until the Gap is closed. Thereafter the Back mode is requested here to fill out the back history.
		/// 
		/// When there is no more time series avialable (but there could be in the future) return an empty ITimeSeriesValues from the function.
		/// After UA does not get values back a few times (configurable by the user) it will stop requesting.
		/// 
		/// When you know that there will definitely not be any more back history you can send back an ITimeSeriesValues container with the
		/// <see cref="ITimeSeriesValues.Complete"/> flag checked. UA will no longer try to back fill the time series in the future.
		/// 
		/// When the CacheMode is <see cref="TimeSeriesParameters.CacheModeKind.Range"/>, this denotes that UA is only trying to request
		/// a range of time series, the UtcDateTimeRange will contain the full range required, but this should not be requested in one take,
		/// rather refer to the PointsLimit and UA will make subsequent requests until the range is closed or there is no more data.
		/// 
		/// *Note it is important to set the <see cref="ITimeSeriesValues.TimeZone"/> of the time series in order for UA to correctly
		/// match up realtime updates applied to the time series.
		/// </summary>
		public override Response RequestTimeSeries(RequestParameters Request, InstrumentIdentifier InId, TimeSeriesParameters Params, ref ITimeSeriesValues ReturnValues)
		{
			/************************************************************************************************\

			To complement the explanation above, some typical example code is provided here to illustrate
			breaking down the TimeSeriesParameters into an appropriate Feed TimeSeries request.
			It can be compiled if uncommented.

			It uses some DateTime operations provided by the TimeFrame class to work out DateTime Ranges
			and counting Data Points.
			
			This is for a Feed in UTC TimeZone so no TimeZone conversion is needed.

			\************************************************************************************************/


			/************************************************************************************************\

			// Lets say this feed only goes back to 01 January 2000
			var MinimumDateTime = new DateTime(2000, 1, 1, 0, 0, 0);

			// first check if the cache contains all the available history,
			if (Params.UtcDateTimeRange.IsValid && Params.UtcDateTimeRange.Max <= MinimumDateTime)
				return true;

			// convert the InId into the identifier format for your feed
			var Instrument = InId.CodeName;

			// convert the UAPI timeframe into the format required by your feed - this is illustrating a feed
			// that treats all time frames in minutes, so a daily timeframe would be 1440 minutes
			int RequestedTimeFrameTotalMinutes = Params.TimeFrame.Minutes;

			// start with requesting up to the latest time
			var UpToDateTime = DateTime.UtcNow;

			// this feed doesnt support more than 1440 data points at a time so cut off the user configuration from UA if needed
			var PointsToRequest = Math.Min(1440, Params.PointsLimit);

			// now check what part of the cache is being requested and adjust how to request this based on the feed requirements
			switch (Params.CacheMode)
			{
				case TimeSeriesParameters.CacheModeKind.Front:
					if (Params.UtcDateTimeRange.IsValid)
					{
						var Range = Params.UtcDateTimeRange;
						var PointsToMaxRange = ((int)(UpToDateTime - Range.Max).TotalMinutes) / RequestedTimeFrameTotalMinutes;
						if (PointsToMaxRange < PointsToRequest)
							PointsToRequest = PointsToMaxRange;
					}
					break;
				case TimeSeriesParameters.CacheModeKind.Gap:
					if (Params.UtcDateTimeRange.IsValid)
					{
						var Range = Params.UtcDateTimeRange;
						var PointsInGap = Params.TimeFrame.PointsIn(Range.TimeSpan);
						PointsToRequest = Math.Min(Math.Min(PointsInGap.Points, PointsToRequest), 1440);
						UpToDateTime = Range.Max;
					}
					break;
				case TimeSeriesParameters.CacheModeKind.Back:
					if (Params.UtcDateTimeRange.IsValid)
					{
						UpToDateTime = Params.UtcDateTimeRange.Max;
					}
					break;
				case TimeSeriesParameters.CacheModeKind.Range:
					if (Params.UtcDateTimeRange.IsValid)
					{
						UpToDateTime = Params.TimeFrame.Prev(Params.UtcDateTimeRange.Min);
						PointsToRequest = Math.Min(Params.TimeFrame.PointsIn(Params.UtcDateTimeRange.TimeSpan).Points, 1440);
					}
					break;
			}

			// Make sure its not going to try request beyond what is supported
			if (UpToDateTime <= MinimumDateTime)
			{
				return true;
			}
			else
			{
				// check if we need to adjust the amount of points to request so it doesnt go too far back in time
				var PointsGoBackToDateTime = Params.TimeFrame.AddPoints(UpToDateTime, -PointsToRequest);
				if (PointsGoBackToDateTime <= MinimumDateTime)
				{
					// now work out how many points we should request preventing going too far back in time
					TimeFrame.TimeFrameSpan PointsInRequestingDateTimeRange = Params.TimeFrame.PointsIn(UpToDateTime - MinimumDateTime);
					PointsToRequest = PointsInRequestingDateTimeRange.Points;
				}
			}
						
			// Illustration of your feed requesting for the instrument, with timeframe, the amount of points up to the specified DateTime
			Log.Inf($"Feed.RequestTimeSeries({Instrument}, {RequestedTimeFrameTotalMinutes}, {PointsToRequest}, {UpToDateTime});");

			\************************************************************************************************/

			return Response.Kind.NotImplemented;
		}

		/// <summary>
		/// CloseInstrument is called when UA is no longer using the Realtime updates of an instrument.
		/// You should unsubscribe according to the <paramref name="Flags"/> cited.
		/// There is no asynchronous method for CloseInstrument.
		/// </summary>
		/// <param name="InId">The instrument to unsubscribe</param>
		/// <param name="Flags">This could denote QuoteScreens, Chart Updates or both</param>		
		public override Response CloseInstrument(InstrumentIdentifier InId, UpdateFlags Flags)
		{
			/*************************************************************************************************\

			Unsubscribe from Updates for the InId according to the Flags.

			\*************************************************************************************************/

			return true;
		}

		/// <summary>
		/// RequestConstituents is called when the user requests either
		/// Forward Curve
		/// Index Members
		/// 
		/// The <paramref name="Kind"/> defines if this is going to be <see cref="ConstituentsKind.ForwardChain"/> or <see cref="ConstituentsKind.IndexMembers"/>
		/// Once the constituent Instrument Identifiers have been constructed from the received constituents returned by your feed, they must be returned
		/// in a sort order as the container to return them is a SortedList.
		/// 
		/// Return asynchronously via the <see cref="UAPI.UAPI.raise_OnRequestConstituentsFinished(long, SortedList{int, InstrumentIdentifier})"/>.
		/// </summary>
		public override Response RequestConstituents(RequestParameters Request, InstrumentIdentifier InId, ConstituentsKind Kind, ref SortedList<int, InstrumentIdentifier> Constituents)
		{
			return base.RequestConstituents(Request, InId, Kind, ref Constituents);
		}

		/// <summary>
		/// UA supports Instrument Identifiers that are logical Futures Contracts to denote the 'Front Contract'
		/// or the 'Front + 1' Contract. These are stored as <see cref="InstrumentFuturesIdentifier"/> which are
		/// a subclass of <see cref="InstrumentIdentifier"/> and so InstrumentIdentifiers can be cast to InstrumentFuturesIdentifiers.
		/// To do this safely, the InstrumentIdentifier stores a Type which can be checked before casting
		/// <see cref="InstrumentIdentifier.Type"/> which is the class name of the actual underlying type.
		/// FuturesInstrumentIdentifiers will have Type = "FuturesInstrumentIdentifier"
		/// 
		/// When UA needs to request data for a logical Futures Instrument it needs to determine the actual underlying instrument
		/// according to the feed. Most of the time the Contract will remain, but once the 'Front' contract rolls to the next,
		/// it will need to be determined what that is.
		/// 
		/// This request is made between data requests in order to gain the underlying actual Contract instrument for the 
		/// logical Futures Instrument. Your feed should determine what is the current 'Front + X' Contract and send it
		/// back in the <paramref name="FuturesInstrument"/>. This can also be done asynchronously via <see cref="UAPI.UAPI.raise_OnRequestFuturesInstrumentFinished(long, string)"/>
		/// </summary>
		/// <param name="InFuId">The Futures Instrument for which we need to request the actual underlying contract from <see cref="InstrumentFuturesIdentifier.FrontLevel"/></param>
		/// <param name="FuturesInstrument">The returned actual contract instrument</param>
		public override Response RequestFuturesInstrument(RequestParameters Request, InstrumentFuturesIdentifier InFuId, ref InstrumentIdentifier FuturesInstrument)
		{
			return base.RequestFuturesInstrument(Request, InFuId, ref FuturesInstrument);
		}

		/// <summary>
		/// Request for the history of each of the underlying Contract instrument for a Near Continuous Chart.
		/// The ChainParams defines which contracts and how much time series to request for each.
		/// It is highly recommended that this data is returned asynchronously via <see cref="UAPI.UAPI.raise_OnRequestNearChainFinished(long, List{Contract})"/>
		/// as there is potentially going to be many feed sub-requests required to attain this data.
		/// This is due to the likelyhood that each Contract is an individual instrument requiring its own time series request.
		/// </summary>
		/// <param name="Request">Request Priority and AsyncId for returning Asyncronously</param>
		/// <param name="InFuId">The futures instrument which has underlying contracts</param>
		/// <param name="ChainParams">Parameters to control which Contracts and what range of TimeSeries to request for each</param>
		/// <param name="Contract">Each Contract's TimeSeries populated in a <see cref="Contract"/> object returned as a List</param>
		/// <returns></returns>
		public override Response RequestNearChain(RequestParameters Request, InstrumentFuturesIdentifier InFuId, ChainTimeSeriesParameters ChainParams, ref List<Contract> Contract)
		{
			return base.RequestNearChain(Request, InFuId, ChainParams, ref Contract);
		}

		#endregion

		#region Trading

		/// <summary>
		/// When UA alerts your Feed to Trade (such as from a Trade Alert setup on a trendline),
		/// it displays your Feed's <see cref="UapiOrderActions"/> Dialog - see the Control setup in SubmitOrder.cs
		/// 
		/// Once your Feed's Order Control submits an order, this function is called in order for your Feed to
		/// perform the work of converting the parameters of the <see cref="Order"/> into its format and
		/// actually submitting the order to the Feed.
		/// 
		/// UA is expecting a result of the order submission which can be returned in <paramref name="Result"/> or
		/// this can be returned asynchronously with <see cref="UAPI.UAPI.raise_OnOrderResult(Order.Result)"/>.
		/// </summary>
		/// <param name="Order">The parameters of the Order to be submitted, such as Buy/Sell Side, Price, Amount, Expiry etc..</param>
		/// <param name="Result">The resutl of submitting the order returned from your feed to be related to UA</param>
		/// <returns></returns>
		public override Response SubmitOrder(Order Order, ref Order.Result Result)
		{
			return base.SubmitOrder(Order, ref Result);
		}

		#endregion

		#region Maintenance

		/// <summary>
		/// This is called when your configured <see cref="InitializeParameters.DailyRefreshTimeUTC"/> passes.
		/// Perform any re-requesting <see cref="UAPI.UAPI.raise_OnReRequest(InstrumentIdentifier, UpdateFlags)"/>
		/// as necessary to update any daily close values etc..
		/// 
		/// You could also get UA to completely restart your feed calling <see cref="UAPI.UAPI.raise_OnRestartMe"/>
		/// UA will make all the necessary data requests to your feed to get the data it is currently displaying.
		/// </summary>
		public override void OnDailyRefresh()
		{
			base.OnDailyRefresh();
		}
		
		/// <summary>
		/// When UA disconnects from your Feed the ClientDisconnected is called in order for you to
		/// handle unsubscribing everything so that data is not lost.
		/// 
		/// Either the <see cref="ClientConnected"/> will be called when UA reconnects or this has been
		/// issued because UA is shutting down where the same procedure would apply for disconnection.
		/// </summary>
		public override void ClientDisconnected()
		{
			Log.Vbs("Disconnected from Updata");
		}

		#endregion

		#region Shutdown

		/// <summary>
		/// During UA shutdown this is called for your Feed to also disconnect and shutdown.
		/// </summary>
		public override Response Shutdown()
		{
			Log.Vbs("Shutdown");

			Settings.ShuttingDown = true;
			Settings.Close();

			return true;
		}
		#endregion

		#region Settings
		/// <summary>
		/// When the user requests the Feed Settings from UA or from the system tray icon, this is called for
		/// you to display your settings dialog. A simple dialog has been created containing basic diagnostic info
		/// and a setting to adjust the Log Level.
		/// 
		/// Loading and Saving of Feed Settings has not been implemented.
		/// <see cref="Ctrader.Settings"/>
		/// </summary>
		public override void SettingsDialog()
		{
			Log.Vbs("Settings");

			try
			{
				if (!Settings.Visible)
					Settings.Show();
			}
			catch { }
		}
		#endregion
	}
}

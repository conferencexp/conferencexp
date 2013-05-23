using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;


namespace MSR.LST.Net.Rtp
{
    /// <summary>
    /// This class is used to fire events on a separate thread using a queue of events to fire with a First In, First Out algorithm.
    /// Rather than use the ThreadPool which would not guarantee FIFO since there are 25+ threads to service the events in the queue,
    /// we have a custom object that uses a single thread to service the queue and guarantees FIFO ordering.
    /// 
    /// The reason this class exists is that two initial approaches failed.
    /// 
    /// Approach 1:  Fire events on the thread that discovered them.  For instance, the RtcpListener thread would detect a new
    /// Rtp Participant and would fire RtpParticipantAdded.  The client, upon catching the event, would draw a new Participant in the UI,
    /// but to do so it would have to make a web service call to the Venue Service to get the Participant's icon.  This would take
    /// up to a second and the RtcpListener thread would block while this synchronous call was occuring.  This caused a number of
    /// incoming Rtcp packets to be dropped while the thread was blocked and in high stress conditions would even cause Rtp Participant
    /// timeouts to occur due to a lack of received Rtcp packets for the participant.
    /// 
    /// Approach 2:  Have the main thread forward off events to the ThreadPool for firing.  The problem here was that events would
    /// get queued and then serviced by the 25 threads sitting in the thread pool.  Due to the intricacies of when threads get serviced
    /// by the CPU, we couldn't guarantee the order in which the events would get serviced.  This caused freaky and very rare race
    /// conditions where the client might receive an RtpStreamAdded event before the RtpParticipantAdded event for the RtpParticipant that the
    /// RtpStream belonged to was received, especially prevalent under stress conditions of large numbers of streams/participants or high CPU
    /// utilization.
    /// </summary>
    internal class EventThrower
    {
        #region Private Static Properties
        private static Thread eventThread = new Thread(new ThreadStart(EventThread));
        private static Queue syncWorkItems = Queue.Synchronized(new Queue()); // of WorkItem structs
        private static RtpEL eventLog = new RtpEL(RtpEL.Source.EventThrower);
        private static AutoResetEvent newWorkItem = new AutoResetEvent(false);
        private static int peakQueueLength = 0;
        #endregion
        #region Public Static Properties
        static public int WorkItemQueueLength
        { get { return EventThrower.syncWorkItems.Count; } }
        static public int PeakEventQueueLength
        { get { return EventThrower.peakQueueLength; } }
        #endregion
        #region Constructors
        static EventThrower()
        {
            eventThread.IsBackground = true;
            eventThread.Name = "EventThrower";
            eventThread.Start();
        }
        #endregion
        #region Thread Method
        /// <summary>
        /// Thread that services the workItems queue
        /// </summary>
        private static void EventThread()
        {
            while (true)
            {
                try
                {
                    if(newWorkItem.WaitOne())
                    {
                        while(syncWorkItems.Count > 0)
                        {
                            WorkItem wi = (WorkItem)syncWorkItems.Dequeue();
                            wi.method(wi.parameters);
                        }
                    }
                }
                catch (ThreadAbortException) {}
                catch (Exception e)
                {
                    eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error, 99);
                }
            }
        }

        #endregion
        #region Internal Methods
        /// <summary>
        /// Queue a work item to be called by a background thread
        /// </summary>
        /// <param name="waitCallback">WaitCallback delegate to be invoked</param>
        /// <param name="parameters">Array of objects to pass as parameters</param>
        internal static void QueueUserWorkItem(RtpEvents.RaiseEvent del, object[] parameters)
        {
            syncWorkItems.Enqueue(new WorkItem(del, parameters));

            if( peakQueueLength < syncWorkItems.Count )
                peakQueueLength = syncWorkItems.Count;
            
            newWorkItem.Set();
        }
        #endregion
        #region Private Structs
        /// <summary>
        /// A WorkItem consisting of the delegate to be called and an array of objects to pass in as parameters
        /// </summary>
        private struct WorkItem
        {
            public RtpEvents.RaiseEvent method;
            public object[] parameters;

            public WorkItem(RtpEvents.RaiseEvent method, object[] parameters)
            {
                this.method = method;
                this.parameters = parameters;
            }
        }
        #endregion
    }
}

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// $Id: MessageDispatcher.java,v 1.30 2004/09/02 14:00:40 belaban Exp $

using System;
using System.Collections;
using System.Threading;
using Alachisoft.NGroups;
using Alachisoft.NGroups.Util;
using Alachisoft.NGroups.Protocols;
using Alachisoft.NGroups.Stack;
using Alachisoft.NCache.Common.Net;
using Alachisoft.NCache.Common;
using Alachisoft.NCache.Common.Stats;
using Alachisoft.NCache.Common.Util;
using Alachisoft.NCache.Common.Logger;

using Runtime = Alachisoft.NCache.Runtime;

namespace Alachisoft.NGroups.Blocks
{
	public class MsgDispatcher : RequestHandler, UpHandler
	{
		private Channel				channel;
		private RequestCorrelator	corr;
		private MessageListener		msg_listener;
		private MembershipListener	membership_listener;
		private RequestHandler		_req_handler;
        private ArrayList _members;
        private MessageResponder _msgResponder;
        private object _statSync = new  object();

		protected internal bool					concurrent_processing;
		protected internal bool					deadlock_detection;
        private Hashtable syncTable = new Hashtable();
        private TimeStats _stats = new TimeStats();
        private long profileId = 0;
        //public NewTrace nTrace = null;
        //public string _cacheName = null;
        private ILogger _ncacheLog = null;
        public ILogger NCacheLog
        {
            get{return _ncacheLog; }
        }

        private HPTimeStats _avgReqExecutionTime = new HPTimeStats();
        private HPTimeStats _operationOnCacheTimeStats = new HPTimeStats();
        private bool useAvgStats = false;

		public MsgDispatcher(Channel channel, MessageListener l, MembershipListener l2, RequestHandler req_handler, MessageResponder responder):this(channel, l, l2, req_handler, responder, false)
		{
		}
		
		public MsgDispatcher(Channel channel, MessageListener l, MembershipListener l2, RequestHandler req_handler, MessageResponder responder, bool deadlock_detection):this(channel, l, l2, req_handler, responder, deadlock_detection, false)
		{
		}
		
		public MsgDispatcher(Channel channel, MessageListener l, MembershipListener l2, RequestHandler req_handler, MessageResponder responder, bool deadlock_detection, bool concurrent_processing)
		{
			this.channel = channel;
            this._ncacheLog = ((GroupChannel)channel).NCacheLog;
			this.deadlock_detection = deadlock_detection;
			this.concurrent_processing = concurrent_processing;
			msg_listener = l;
			membership_listener = l2;
			_req_handler = req_handler;
            _msgResponder = responder;

		
			channel.UpHandler = this;
			start();
		}

        public virtual void  start()
		{
			if (corr == null)
			{
				corr = new RequestCorrelator("MsgDisp", channel, this, deadlock_detection, channel.LocalAddress, concurrent_processing, this._ncacheLog);
				corr.start();
                if (System.Configuration.ConfigurationSettings.AppSettings["useAvgStats"] != null)
                {
                    useAvgStats = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["useAvgStats"]);
                }
			}
		}

		public virtual void  stop()
		{
			if (corr != null)
			{
				corr.stop();
			}
		}

        public void StopReplying()
        {
            if (corr != null) corr.StopReplying();
        }

		/// <summary> Called by channel (we registered before) when event is received. This is the UpHandler interface.</summary>
		public void  up(Event evt)
		{
			try
			{


				if (corr != null)
				{
					// message handled
                    if (corr.receive(evt))
                    {                        
                        return;
                    }
				}
                
				passUp(evt);
			}
            catch (NullReferenceException) { }
			catch (Exception e)
			{
				NCacheLog.Error("MsgDispatcher.up()",   "exception=" + e);
			}
		}

		/// <summary> Called by request correlator when message was not generated by it. We handle it and call the message
		/// listener's corresponding methods
		/// </summary>
		protected void passUp(Event evt)
		{
			switch (evt.Type)
			{
			case Event.MSG: 
				if (msg_listener != null)
				{
                    HPTimeStats reqHandleStats = null;
                   

					msg_listener.receive((Message) evt.Arg);

                    if (reqHandleStats != null)
                    {
                        reqHandleStats.EndSample();
                        if (!useAvgStats)
                        {
                            
                        }
                        else
                        {
                            
                        }
                    }
                    
				}
				break;

            case Event.HASHMAP_REQ:
                if (_msgResponder != null)
                {
                    NCacheLog.Debug("MessageDispatcher.PassUp()",  "here comes the request for hashmap");
                    
                    object map = null;
                    try
                    {
                        map = _msgResponder.GetDistributionAndMirrorMaps(evt.Arg);
                    }
                    catch (Exception e)
                    {
                         NCacheLog.CriticalInfo("MsgDispatcher.passUP", "An error occurred while getting new hashmap. Error: " + e.ToString());
                    }
                    Event evnt = new Event();
                    evnt.Type = Event.HASHMAP_RESP;
                    evnt.Arg = map;
                    channel.down(evnt);
                    NCacheLog.Debug("MessageDispatcher.PassUp()",   "sending the response for hashmap back...");
                }
                break;

          

			case Event.VIEW_CHANGE: 
				View v = (View) evt.Arg;
				ArrayList new_mbrs = v.Members;
				if (membership_listener != null)
				{
                    NCacheLog.Debug("MessageDispatcher.passUp", "Event.VIEW_CHANGE-> Entering: " + v.ToString());
					membership_listener.viewAccepted(v);
                    NCacheLog.Debug("MessageDispatcher.passUp", "Event.VIEW_CHANGE->Done" + v.ToString());
                   
				}
				break;
            case Event.ASK_JOIN:
                if (membership_listener != null)
                {
                    Event et = new Event();
                    et.Type = Event.ASK_JOIN_RESPONSE;
                    et.Arg = membership_listener.AllowJoin();
                    channel.down(et);
                    
                }
                   
                break;
			case Event.SET_LOCAL_ADDRESS: 
				break;
				
			case Event.SUSPECT: 
				if (membership_listener != null)
				{
					membership_listener.suspect((Address) evt.Arg);
                  
				}
				break;
				
			case Event.BLOCK: 
				if (membership_listener != null)
				{
					membership_listener.block();
                    
				}
				break;
			}
		}
			
		public virtual void  send(Message msg)
		{
			if (channel != null)
			{
				channel.send(msg);
			}
			else
			{
				NCacheLog.Error("channel == null");
			}
		}
		
		
		/// <summary> Cast a message to all members, and wait for <code>mode</code> responses. The responses are returned in a response
		/// list, where each response is associated with its sender.<p> Uses <code>GroupRequest</code>.
		/// 
		/// </summary>
		/// <param name="dests">  The members to which the message is to be sent. If it is null, then the message is sent to all
		/// members
		/// </param>
		/// <param name="msg">    The message to be sent to n members
		/// </param>
		/// <param name="mode">   Defined in <code>GroupRequest</code>. The number of responses to wait for: <ol> <li>GET_FIRST:
		/// return the first response received. <li>GET_ALL: wait for all responses (minus the ones from
		/// suspected members) <li>GET_MAJORITY: wait for a majority of all responses (relative to the grp
		/// size) <li>GET_ABS_MAJORITY: wait for majority (absolute, computed once) <li>GET_N: wait for n
		/// responses (may block if n > group size) <li>GET_NONE: wait for no responses, return immediately
		/// (non-blocking) </ol>
		/// </param>
		/// <param name="timeout">If 0: wait forever. Otherwise, wait for <code>mode</code> responses <em>or</em> timeout time.
		/// </param>
		/// <returns> RspList A list of responses. Each response is an <code>Object</code> and associated to its sender.
		/// </returns>
		public virtual RspList castMessage(ArrayList dests, Message msg, byte mode, long timeout)
		{
			GroupRequest _req = null;
			ArrayList real_dests;

            ArrayList clusterMembership = channel.View.Members != null ? (ArrayList)channel.View.Members.Clone() : null;
			// we need to clone because we don't want to modify the original
			// (we remove ourselves if LOCAL is false, see below) !
            real_dests = dests != null ? (ArrayList)dests.Clone() : clusterMembership;
			
			
			// if local delivery is off, then we should not wait for the message from the local member.
			// therefore remove it from the membership
			if (channel != null && channel.getOpt(Channel.LOCAL).Equals(false))
			{
				real_dests.Remove(channel.LocalAddress);
			}
			
			// don't even send the message if the destination list is empty
            if (NCacheLog.IsInfoEnabled) NCacheLog.Info("MsgDispatcher.castMessage()",  "real_dests=" + Global.CollectionToString(real_dests));
			
			if (real_dests == null || real_dests.Count == 0)
			{
                if (NCacheLog.IsInfoEnabled) NCacheLog.Info("MsgDispatcher.castMessage()",  "destination list is empty, won't send message");
                
				return new RspList(); // return empty response list
			}
			
			_req = new GroupRequest(msg, corr, real_dests, clusterMembership, mode, timeout, 0, this._ncacheLog);


            _req.execute();

           
            if(mode != GroupRequest.GET_NONE)
                ((GroupChannel)channel).Stack.perfStatsColl.IncrementClusteredOperationsPerSecStats();
  

            RspList rspList = _req.Results;

            if(rspList != null)
            {
                for(int i = 0; i< rspList.size(); i++)
                {
                    Rsp rsp = rspList.elementAt(i) as Rsp;
                    if (rsp != null)
                    {
                        if (!rsp.wasReceived() && !rsp.wasSuspected())
                        {
                            if (corr.CheckForMembership((Address)rsp.sender))
                                rsp.suspected = true;

                        }
                    }
                }
            }
            return rspList;
		}

        public void SendResponse(long resp_id, Message response)
        {
            corr.SendResponse(resp_id, response);
        }
		/// <summary> Multicast a message request to all members in <code>dests</code> and receive responses via the RspCollector
		/// interface. When done receiving the required number of responses, the caller has to call done(req_id) on the
		/// underlyinh RequestCorrelator, so that the resources allocated to that request can be freed.
		/// 
		/// </summary>
		/// <param name="dests"> The list of members from which to receive responses. Null means all members
		/// </param>
		/// <param name="req_id">The ID of the request. Used by the underlying RequestCorrelator to correlate responses with
		/// requests
		/// </param>
		/// <param name="msg">   The request to be sent
		/// </param>
		/// <param name="coll">  The sender needs to provide this interface to collect responses. Call will return immediately if
		/// this is null
		/// </param>
		public virtual void  castMessage(ArrayList dests, long req_id, Message msg, RspCollector coll)
		{
			ArrayList real_dests;
			if (msg == null)
			{
				NCacheLog.Error("MsgDispatcher.castMessage()",   "request is null");
				return ;
			}
			
			if (coll == null)
			{
				NCacheLog.Error("MessageDispatcher.castMessage()",   "response collector is null (must be non-null)");
				return ;
			}
			
			// we need to clone because we don't want to modify the original
			// (we remove ourselves if LOCAL is false, see below) !
			real_dests = dests != null?(ArrayList) dests.Clone():(ArrayList) channel.View.Members.Clone();
			
			// if local delivery is off, then we should not wait for the message from the local member.
			// therefore remove it from the membership
			if (channel != null && channel.getOpt(Channel.LOCAL).Equals(false))
			{
				real_dests.Remove(channel.LocalAddress);
			}
			
			// don't even send the message if the destination list is empty
			if (real_dests.Count == 0)
			{
                NCacheLog.Debug("MsgDispatcher.castMessage()",  "destination list is empty, won't send message");
				return ;
			}
			
			corr.sendRequest(req_id, real_dests, msg, coll);
		}


        public virtual void done(long req_id)
		{
			corr.done(req_id);
		}
		
		
		/// <summary> Sends a message to a single member (destination = msg.dest) and returns the response. The message's destination
		/// must be non-zero !
		/// </summary>
		public virtual object sendMessage(Message msg, byte mode, long timeout)
		{
			RspList rsp_list = null;
			object dest = msg.Dest;
			Rsp rsp;
			GroupRequest _req = null;
			
			if (dest == null)
				return null;
			
			ArrayList mbrs = ArrayList.Synchronized(new ArrayList(1));
			mbrs.Add(dest); // dummy membership (of destination address)

            ArrayList clusterMembership = channel.View.Members != null ? (ArrayList)channel.View.Members.Clone() : null;

			_req = new GroupRequest(msg, corr, mbrs, clusterMembership, mode, timeout, 0, this._ncacheLog);
           
            _req.execute();

			if (mode == GroupRequest.GET_NONE)
			{
				return null;
			}

            ((GroupChannel)channel).Stack.perfStatsColl.IncrementClusteredOperationsPerSecStats();

			rsp_list = _req.Results;
			
			if (rsp_list.size() == 0)
			{
                NCacheLog.Warn("MsgDispatcher.sendMessage()",  " response list is empty");
				return null;
			}
			if (rsp_list.size() > 1)
			{
                NCacheLog.Warn("MsgDispatcher.sendMessage()",  "response list contains more that 1 response; returning first response !");
			}
			rsp = (Rsp) rsp_list.elementAt(0);
			if (rsp.wasSuspected())
			{
				throw new SuspectedException(dest);
			}
			if (!rsp.wasReceived())
			{
                //we verify for the destination whether it is still part of the cluster or not.
                if (corr.CheckForMembership((Address)rsp.Sender))

                    throw new Runtime.Exceptions.TimeoutException("operation timeout");

                else
                {
                    rsp.suspected = true;
                    throw new SuspectedException(dest);
                }
			}
			return rsp.Value;
		}
		
			
		/* ------------------------ RequestHandler Interface ---------------------- */
		public virtual object handle(Message msg)
		{
			if (_req_handler != null)
			{

                object result = _req_handler.handle(msg);
                return result;
			}
			return null;
		}

        public virtual object handleNHopRequest(Message msg, out Address destination, out Message replicationMsg)
        {
            destination = null;
            replicationMsg = null;

            if (_req_handler != null)
            {
                object result = _req_handler.handleNHopRequest(msg, out destination, out replicationMsg);
                return result;
            }

            return null;
        }

	}
	
}

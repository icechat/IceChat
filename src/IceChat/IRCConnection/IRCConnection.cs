/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2016 Paul Vanderzee <snerf@icechat.net>
 *                                    <www.icechat.net> 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 * Please consult the LICENSE.txt file included with this project for
 * more details
 *
\******************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Security.Authentication;

namespace IceChat
{
    public partial class IRCConnection : IDisposable
    {
        private Socket serverSocket = null;
        private NetworkStream socketStream = null;
        private SslStream sslStream = null;
        
        private bool m_PendingWriteSSL = false;

        private string dataBuffer = "";
        private Queue<string> sendBuffer;

        private bool disconnectError = false;
        private bool attemptReconnect = true;

        private System.Timers.Timer reconnectTimer;
        private System.Timers.Timer buddyListTimer;
        public int buddiesIsOnSent = 0;
        
        public List<string> OpenChannels = new List<string>();

        public bool ShowDebug = true;   //show in the debug window

        private ServerSetting serverSetting;
        private bool fullyConnected = false;
        private ArrayList commandQueue;
        private List<IrcTimer> ircTimers;

        private System.Timers.Timer pongTimer;
        private System.Timers.Timer autoAwayTimer;

        private int whichAddressinList = 1;
        private int whichAddressCurrent = 1;
        private int totalAddressinDNS = 0;
        
        //private const int bytesperlong = 4; // 32 / 8
        //private const int bitsperbyte = 8;

        //private SslStream sslStream;
        //private NetworkStream socketStream;
        private bool proxyAuthed;
        private byte[] readBuffer;

        //private const int BUFFER_SIZE = 1024;
        private const int BUFFER_SIZE = 4096;

        public delegate string ParseIdentifierDelegate(IRCConnection connection, string message);
        public event ParseIdentifierDelegate ParseIdentifier;

        public IRCConnection(ServerSetting ss)
        {
            commandQueue = new ArrayList();
            sendBuffer = new Queue<string>();
            serverSetting = ss;

            reconnectTimer = new System.Timers.Timer(ss.ReconnectTime * 1000);
            reconnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnReconnectTimerElapsed);

            if (!ss.MonitorSupport)
            {
                buddyListTimer = new System.Timers.Timer(60000);
                buddyListTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnBuddyListTimerElapsed);
            }

            if (ss.PingTimerMinutes < 1)    //force to 1 minute minimum
                ss.PingTimerMinutes = 1;
            
            pongTimer = new System.Timers.Timer(60000 * ss.PingTimerMinutes);    //5 minutes
            pongTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnPongTimerElapsed);

            autoAwayTimer = new System.Timers.Timer();
            autoAwayTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnAutoAwayTimerElapsed);
            autoAwayTimer.Enabled = false;

            ircTimers = new List<IrcTimer>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (serverSocket != null)
                        serverSocket.Disconnect(false);

                }
                catch
                {
                }

                reconnectTimer.Stop();
                reconnectTimer.Dispose();

                pongTimer.Stop();
                pongTimer.Dispose();

                autoAwayTimer.Stop();
                autoAwayTimer.Dispose();

                if (buddyListTimer != null)
                {
                    buddyListTimer.Stop();
                    buddyListTimer.Dispose();
                }

                foreach (IrcTimer t in ircTimers)
                {
                    t.Stop();
                    t.Dispose();
                }

                if (socketStream != null)
                    socketStream.Dispose();

                if (sslStream != null)
                    sslStream.Dispose();
            }
        }

        public string Parse(string msg)
        {
            if (ParseIdentifier != null)
                msg = ParseIdentifier(this, msg);
            
            return msg;
        }

        public void MonitorListCheck()
        {
            if (serverSetting.BuddyListEnable)
            {
                string isOn = string.Empty;
                string isNotOn = string.Empty;

                foreach (BuddyListItem buddy in serverSetting.BuddyList)
                {
                    if (!buddy.IsOnSent)
                    {
                        if (!buddy.Nick.StartsWith(";"))
                        {
                            if (isOn == string.Empty)
                                isOn = buddy.Nick;
                            else
                                isOn += "," + buddy.Nick;

                            buddy.IsOnSent = true;
                            buddy.IsOnReceived = false;
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("remove:" + buddy.Nick + ":" + buddy.IsOnSent + ":" + buddy.IsOnReceived + ":" + buddy.Connected);

                            //remove from list --if it was 
                            if (buddy.IsOnReceived)
                            {
                                if (isNotOn == string.Empty)
                                    isNotOn = buddy.Nick.Substring(1);
                                else
                                    isNotOn += "," + buddy.Nick.Substring(1);

                                buddy.IsOnSent = true;
                                buddy.IsOnReceived = false;
                            
                                //need to remove from the buddy list
                                BuddyRemove(this, buddy);
                            }
                            
                        }
                    }
                    else
                    {
                        //buddy has been sent, see if it has been disabled
                        if (buddy.Nick.StartsWith(";"))
                        {
                            System.Diagnostics.Debug.WriteLine("Buddy Sent but Disabled:" + buddy.Nick);
                        }

                    }

                }
                if (isOn != string.Empty)
                {
                    System.Diagnostics.Debug.WriteLine("MONITOR + " + isOn + " *");
                    SendData("MONITOR + " +isOn+ " *");
                }
                
                if (isNotOn != string.Empty)
                {
                    System.Diagnostics.Debug.WriteLine("MONITOR - " + isNotOn + " *");
                    SendData("MONITOR - " + isNotOn + " *");
                }
            }
        }

        public void BuddyListCheck()
        {
            try
            {
                if (serverSetting.BuddyListEnable && serverSetting.BuddyList != null)
                {
                    string ison = string.Empty;

                    foreach (BuddyListItem buddy in serverSetting.BuddyList)
                    {
                        if (ison.Length > 200)
                            break;
                        else if (!buddy.IsOnSent)
                        {
                            if (!buddy.Nick.StartsWith(";"))
                            {
                                ison += " " + buddy.Nick;

                                buddy.IsOnSent = true;
                                buddy.IsOnReceived = false;
                            }

                            buddiesIsOnSent++;
                        }
                    }

                    if (ison != string.Empty)
                    {
                        ison = "ISON" + ison;

                        SendData(ison);
                    }
                    else
                        BuddyListClear(this);

                    buddyListTimer.Stop();

                }
                else
                    BuddyListClear(this);

            }
            catch (Exception ex)
            {
                WriteErrorFile(this, "BuddyListCheck", ex);
            }

        }

        private void OnBuddyListTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BuddyListCheck();
        }

        private void OnReconnectTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (attemptReconnect)
            {
                whichAddressinList++;

                ServerReconnect(this);
                
                this.ConnectSocket();
            }
        }

        private void OnPongTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //pong has not received, re-connect server
                //disable for the time being
                //send a ping to the server
                //System.Diagnostics.Debug.WriteLine(DateTime.Now +  ":send pong timer:" + socketStream.CanRead + ":" + socketStream.CanWrite);
                SendData("PING :TIMEOUTCHECK");

            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine(se.Message);
            }
        }


        #region Public Properties and Methods

        public void AddToCommandQueue(string command)
        {
            commandQueue.Add(command);
        }

        public ServerSetting ServerSetting
        {
            get
            {
                return serverSetting;
            }
            set
            {
                serverSetting = value;
            }
        }

        public System.Timers.Timer ReconnectTimer
        {
            get { return this.reconnectTimer; }
        }

        public bool IsConnected
        {
            get
            {
                if (serverSocket == null)
                    return false;

                if (serverSocket.Connected)
                    return true;
                else
                    return false;
            }

        }

        public bool DisconnectError
        {
            get
            {
                return disconnectError;
            }
            set
            {
                this.disconnectError = value;
            }
        }

        public bool IsFullyConnected
        {
            get
            {
                return fullyConnected;
            }
        }

        public bool AttemptReconnect
        {
            get { return attemptReconnect; }
            set { attemptReconnect = value; }
        }

        #endregion

        #region Socket Events and Methods

        /// <summary>
        /// Event for Server Disconnection
        /// </summary>
        /// <param name="ar"></param>
        private void OnDisconnect(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);

            ServerDisconnect(this);

        }

        private void OnDisconnected()
        {
            int howFar = 0;
            
            System.Diagnostics.Debug.WriteLine("OnDisconnected:" + this.ServerSetting.ServerName);
            
            try
            {
                RefreshServerTree(this);

                howFar = 2;

                if (serverSetting.UseBNC == true)
                    StatusText(this, serverSetting.CurrentNickName + " disconnected (" + serverSetting.BNCIP + ")");
                else
                    StatusText(this, serverSetting.CurrentNickName + " disconnected (" + serverSetting.ServerName + ")");

                howFar = 3;

                serverSocket = null;
                serverSetting.ConnectedTime = DateTime.Now;

                howFar = 4;

                commandQueue.Clear();
                pongTimer.Enabled = false;

                howFar = 5;
                if (buddyListTimer != null)
                    buddyListTimer.Stop();

                howFar = 6;
                
                BuddyListClear(this);

                howFar = 6;

                if (serverSetting.BuddyList != null)
                {
                    foreach (BuddyListItem buddy in serverSetting.BuddyList)
                    {
                        buddy.Connected = false;
                        buddy.PreviousState = false;
                        buddy.IsOnSent = false;
                        buddy.IsOnReceived = false;
                    }
                }

                howFar = 7;

                initialLogon = false;

                serverSetting.TriedAltNick = false;
                serverSetting.ChannelJoins.Clear();
                serverSetting.LastChannelsParted.Clear();

                howFar = 8;

                fullyConnected = false;

                if (serverSetting.IAL != null)
                    serverSetting.IAL.Clear();

                howFar = 9;

                serverSetting.Away = false;
                serverSetting.RealServerName = "";
                serverSetting.CurrentNickName = "";

                howFar = 10;

                pongTimer.Stop();
                autoAwayTimer.Stop();

                howFar = 11;

                if (serverSetting.UseProxy)
                    proxyAuthed = false;

                howFar = 12;

                //disable and remove all timers
                foreach (IrcTimer t in ircTimers)
                    t.DisableTimer();

                howFar = 13;

                ircTimers.Clear();
                
                howFar = 14;

            }
            catch (Exception ex)
            {
                ServerError(this, "OnDisconnected Exception Error: " + ex.Message.ToString() + ":" + howFar, false);
            }
        }

        /// <summary>
        /// Event for Server Connection
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnectionReady(IAsyncResult ar)
        {
            reconnectTimer.Stop();
            attemptReconnect = true;

            if (serverSocket == null)
            {
                if (ServerError != null)
                    ServerError(this, "Null Socket - Can not Connect", false);
                return;
            }

            try
            {
                serverSocket.EndConnect(ar);
            }
            catch (SocketException se)
            {
                ServerError(this, "Socket Exception Error: " + se.Message, false);

                disconnectError = true;
                ForceDisconnect();
                return;
            }
            catch (Exception e)
            {
                ServerError(this, "Exception Error: " + e.Message.ToString(), false);

                disconnectError = true;
                ForceDisconnect();
                return;
            }
            
            try
            {
                socketStream = new NetworkStream(serverSocket);               
            }
            catch (SocketException se)
            {

                if (ServerError != null)
                    ServerError(this, "Socket Exception Error1: " + se.Message, false);

                disconnectError = true;
                ForceDisconnect();
                return;
            
            }

            if (serverSetting.UseSSL)
            {
                try
                {
                    sslStream = new SslStream(socketStream, true, this.RemoteCertificateValidationCallback);

                    SslProtocols enabledSslProtocols;

                    #if USE_NET_45
                        enabledSslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls12 | SslProtocols.Tls11;
                    #else
                        enabledSslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls;
                    #endif

                    // allow for a private key ??
                    sslStream.AuthenticateAsClient(serverSetting.ServerName, null, enabledSslProtocols, true);

                    ServerMessage(this, "*** You are connected to this server with " + sslStream.SslProtocol.ToString().ToUpper() + "-" + sslStream.CipherAlgorithm.ToString().ToUpper() + sslStream.CipherStrength + "-" + sslStream.HashAlgorithm.ToString().ToUpper() + "-" + sslStream.HashStrength + "bits", "");
                }
                catch (System.Security.Authentication.AuthenticationException ae)
                {
                    if (ServerError != null)
                        ServerError(this, "SSL Authentication Error :" + ae.Message.ToString(), false);

                    disconnectError = true;
                    ForceDisconnect();
                    return;
                }
                catch (System.IO.IOException ex)
                {
                    if (ServerError != null)
                        ServerError(this, "SSL IO Exception Error :" + ex.Message.ToString(), false);

                    disconnectError = true;
                    ForceDisconnect();
                    return;

                }
                catch (Exception e)
                {
                    if (ServerError != null)
                        ServerError(this, "SSL Exception Error :" + e.Message.ToString(), false);

                    disconnectError = true;
                    ForceDisconnect();
                    return;
                }
            }
            

            try
            {
                if (serverSetting.UseSSL)
                {
                    if (sslStream != null && sslStream.CanRead)
                    {
                        readBuffer = new byte[BUFFER_SIZE];
                        sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                    }
                }
                else
                {
                    if (socketStream != null && socketStream.CanRead)
                    {
                        readBuffer = new byte[BUFFER_SIZE];
                        socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);
                    }
                }
                
                this.serverSetting.ConnectedTime = DateTime.Now;

                RefreshServerTree(this);

                if (serverSetting.UseProxy)
                {
                    // http://www.koders.com/csharp/fid0AAABC16896220BEB8D69A3C5035C89D3AA5ACF6.aspx?s=cdef%3Asocket

                    if (serverSetting.ProxyType == 2)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        System.IO.BinaryWriter wr = new System.IO.BinaryWriter(ms);

                        wr.Write((byte)4);
                        wr.Write((byte)1);
                        wr.Write(IPAddress.HostToNetworkOrder(Convert.ToInt32(serverSetting.ServerPort)));
                        //get the ip address of the server
                        IPHostEntry hostEntry = Dns.GetHostEntry(serverSetting.ServerName);
                        if (hostEntry.AddressList.Length >0 )
                        {
                            wr.Write(hostEntry.AddressList[0].GetAddressBytes());
                            wr.Write((byte)0);
                            wr.Close();

                            byte[] d = ms.ToArray();

                            try
                            {
                                if (serverSetting.UseSSL)                                
                                    sslStream.BeginWrite(d, 0, d.Length, new AsyncCallback(OnSendData), sslStream);
                                else
                                    socketStream.BeginWrite(d, 0, d.Length, new AsyncCallback(OnSendData), socketStream);

                                if (ServerMessage != null)
                                    ServerMessage(this, "Socks 4 Connection Established with " + serverSetting.ProxyIP + " , waiting for Authorization", "");
                            }
                            catch (SocketException)
                            {
                                System.Diagnostics.Debug.WriteLine("Error Sending Proxy v4 Data");

                            }
                            catch (Exception)
                            {
                                System.Diagnostics.Debug.WriteLine("proxy v4 exception");
                            }
                        }
                        else
                        {
                            wr.Close();
                            //error. IP ADDRESS of server does not resolve
                            ServerMessage(this, "Socks 4 Connection could not be established with " + serverSetting.ProxyIP, "");
                            ForceDisconnect();
                        }
                        
                    }
                    
                    else if (serverSetting.ProxyType == 3)
                    {
                        //socks v5 code
                        byte[] d = new byte[256];
                        ushort nIndex = 0;
                        d[nIndex++] = 0x05;

                        if (serverSetting.ProxyUser.Length > 0)
                        {
                            d[nIndex++] = 0x02;
                            d[nIndex++] = 0x00;
                            d[nIndex++] = 0x02;
                        }
                        else
                        {
                            d[nIndex++] = 0x01;
                            d[nIndex++] = 0x00;
                        }

                        try
                        {
                            if (serverSetting.UseSSL)
                                sslStream.BeginWrite(d, 0, nIndex, new AsyncCallback(OnSendData), sslStream);
                            else
                                socketStream.BeginWrite(d, 0, nIndex, new AsyncCallback(OnSendData), socketStream);
                            
                            if (ServerMessage != null)
                                ServerMessage(this, "Socks 5 Connection Established with " + serverSetting.ProxyIP, "");
                        }
                        catch (SocketException)
                        {
                            System.Diagnostics.Debug.WriteLine("Error Sending Proxy Data");
                            ForceDisconnect();
                        }
                        catch (Exception)
                        {
                            System.Diagnostics.Debug.WriteLine("proxy exception");
                            ForceDisconnect();
                        }
                    }
                }
                else
                {
                    ServerPreConnect(this);

                    if (serverSetting.Password != null && serverSetting.Password.Length > 0)
                        SendData("PASS " + serverSetting.Password);

                    if (serverSetting.UseBNC == true && serverSetting.BNCPass != null && serverSetting.BNCPass.Length > 0)
                        SendData("PASS " + serverSetting.BNCPass);

                    //check for IRCv3 capability
                    SendData("CAP LS 302");

                    //send the USER / NICK stuff                    
                    
                    SendData("NICK " + serverSetting.NickName);

                    if (serverSetting.UseBNC == true && serverSetting.BNCUser != null && serverSetting.BNCUser.Length > 0)
                        SendData("USER " + serverSetting.BNCUser + " \"localhost\" \"" + serverSetting.BNCIP + "\" :" + serverSetting.FullName);
                    else
                        SendData("USER " + serverSetting.IdentName + " \"localhost\" \"" + serverSetting.ServerName + "\" :" + serverSetting.FullName);
                    
                    ServerMessage(this, "Sending User Registration Information", "");
                    

                    whichAddressinList = whichAddressCurrent;
                    
                    if (serverSetting.UseBNC == true)
                        this.fullyConnected = true;
                    
                    this.pongTimer.Start();
                }
            }

            catch (SocketException se)
            {
                if (ServerError != null)
                    ServerError(this, "Socket Exception Error:" + se.Message.ToString() + ":" + se.ErrorCode, false);

                disconnectError = true;
                ForceDisconnect();
            }
            catch (Exception e)
            {
                if (ServerError != null)
                    ServerError(this, "Exception Error:" + serverSetting.UseBNC + ":" + e.Message.ToString(), false);

                disconnectError = true;
                ForceDisconnect();
            }

        }

        /*
        private bool SetKeepAlive(Socket sock, ulong time, ulong interval)
        {
            try
            {
                // resulting structure
                byte[] SIO_KEEPALIVE_VALS = new byte[3 * bytesperlong];

                // array to hold input values
                ulong[] input = new ulong[3];

                // put input arguments in input array
                if (time == 0 || interval == 0) // enable disable keep-alive
                    input[0] = (0UL); // off
                else
                    input[0] = (1UL); // on

                input[1] = (time); // time millis
                input[2] = (interval); // interval millis

                // pack input into byte struct
                for (int i = 0; i < input.Length; i++)
                {
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 3] = (byte)(input[i] >> ((bytesperlong - 1) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 2] = (byte)(input[i] >> ((bytesperlong - 2) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 1] = (byte)(input[i] >> ((bytesperlong - 3) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 0] = (byte)(input[i] >> ((bytesperlong - 4) * bitsperbyte) & 0xff);
                }
                // create bytestruct for result (bytes pending on server socket)
                byte[] result = BitConverter.GetBytes(0);
                // write SIO_VALS to Socket IOControl
                sock.IOControl(IOControlCode.KeepAliveValues, SIO_KEEPALIVE_VALS, result);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        */

        /// <summary>
        /// Function for sending RAW IRC Data to the Server Connection
        /// </summary>
        /// <param name="data"></param>
        public void SendData(string data)
        {

            try
            {

                //check if the socket is still connected
                if (serverSocket == null)
                {
                    if (ServerError != null)
                        ServerError(this, "Error: You are not connected (Socket not created) - Can not send", false);

                    return;
                }

                if (socketStream == null)
                {
                    System.Diagnostics.Debug.WriteLine("senddata null stream");
                    return;
                }

                if (serverSetting == null)
                {
                    System.Diagnostics.Debug.WriteLine("senddata null serversetting");
                    return;
                }

                //set the proper encoding            
                byte[] bytData = Encoding.GetEncoding(serverSetting.Encoding).GetBytes(data + "\r\n");
                if (data.Length > 0)
                {
                    if (serverSetting.UseSSL == true)
                    {
                        if (sslStream.CanWrite)
                        {
                            try
                            {
                                //raise an event for the debug window
                                if (m_PendingWriteSSL == false)
                                {
                                    if (sendBuffer.Count > 0)
                                        m_PendingWriteSSL = true;

                                    sslStream.BeginWrite(bytData, 0, bytData.Length, new AsyncCallback(OnSendData), sslStream);

                                    if (RawServerOutgoingData != null)
                                        RawServerOutgoingData(this, data);
                                }
                                else
                                {
                                    sendBuffer.Enqueue(data);
                                }

                            }
                            catch (SocketException se)
                            {
                                //some kind of a socket error
                                if (ServerError != null)
                                    ServerError(this, "You are not Connected - Can not send (SSL):  " + se.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                System.Diagnostics.Debug.WriteLine("SSL Connection Error");
                                ForceDisconnect();
                            }
                            catch (NotSupportedException)
                            {
                                //BeginWrite failed because of already trying to send, so add to the sendBuffer Queue
                                //System.Diagnostics.Debug.WriteLine("nse SSL:" + nse.Message);
                                sendBuffer.Enqueue(data);
                            }
                            catch (Exception ex)
                            {
                                if (ServerError != null)
                                    ServerError(this, "SSL Exception Error - Can not send:" + ex.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                ForceDisconnect();
                            }
                        }
                        else
                        {
                            if (ServerError != null)
                                ServerError(this, "You are not Connected (SSL Socket Disconnected) - Can not send:" + data, false);

                            System.Diagnostics.Debug.WriteLine("ssl not connected error");

                            attemptReconnect = true;
                            disconnectError = true;
                            ForceDisconnect();
                        }
                    }
                    else
                    {
                        if (socketStream.CanWrite && socketStream.CanRead)
                        {
                            try
                            {
                                socketStream.BeginWrite(bytData, 0, bytData.Length, new AsyncCallback(OnSendData), socketStream);

                                //raise an event for the debug window
                                if (RawServerOutgoingData != null)
                                    RawServerOutgoingData(this, data);
                            }
                            catch (SocketException se)
                            {
                                //some kind of a socket error
                                if (ServerError != null)
                                    ServerError(this, "You are not Connected - Can not send: " + se.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                ForceDisconnect();
                            }
                            catch (NotSupportedException)
                            {
                                //BeginWrite failed because of already trying to send, so add to the sendBuffer Queue
                                sendBuffer.Enqueue(data);
                            }
                            catch (Exception ex)
                            {
                                if (ServerError != null)
                                    ServerError(this, "Exception Error - Can not send:" + ex.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                ForceDisconnect();
                            }
                        }
                        else
                        {
                            if (ServerError != null)
                                ServerError(this, "You are not connected (Socket Disconnected) - Can not send:" + data, false);

                            attemptReconnect = true;
                            disconnectError = true;
                            ForceDisconnect();
                        }


                    }

                }
            }
            catch (Exception e)
            {
                WriteErrorFile(this, "SendData(s) Error:" + data, e);
            }
        }

        public void SendData(byte[] bytData)
        {
            try
            {
                //check if the socket is still connected
                if (serverSocket == null)
                {
                    if (ServerError != null)
                        ServerError(this, "Error: You are not Connected (Socket not created) - Can not send", false);
                    return;
                }

                if (socketStream == null)
                {
                    System.Diagnostics.Debug.WriteLine("senddata null stream");
                    return;
                }

                if (serverSetting == null)
                {
                    System.Diagnostics.Debug.WriteLine("senddata null serversetting");
                    return;
                }

                //get the proper encoding            
                if (bytData.Length > 0)
                {
                    if (serverSetting.UseSSL == true)
                    {
                        if (sslStream.CanWrite == true)
                        {
                            try
                            {
                                //raise an event for the debug window
                                string strData = Encoding.GetEncoding(serverSetting.Encoding).GetString(readBuffer);
                                if (RawServerOutgoingData != null)
                                    RawServerOutgoingData(this, strData);

                                sslStream.BeginWrite(bytData, 0, bytData.Length, new AsyncCallback(OnSendData), sslStream);

                            }
                            catch (SocketException se)
                            {
                                //some kind of a socket error
                                if (ServerError != null)
                                    ServerError(this, "You are not Connected - Can not send (SSL):" + se.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                ForceDisconnect();
                            }
                            catch (NotSupportedException)
                            {
                                //BeginWrite failed because of already trying to send, so add to the sendBuffer Queue
                                //sendBuffer.Enqueue(data);
                            }
                        }
                        else
                        {
                            if (ServerError != null)
                                ServerError(this, "You are not Connected (SSL Socket Disconnected) - Can not send:" + bytData.ToString(), false);

                            attemptReconnect = true;
                            disconnectError = true;
                            ForceDisconnect();
                        }

                    }
                    else
                    {
                        if (socketStream.CanWrite == true)
                        {
                            try
                            {
                                //raise an event for the debug window
                                string strData = Encoding.GetEncoding(serverSetting.Encoding).GetString(readBuffer);
                                if (RawServerOutgoingData != null)
                                    RawServerOutgoingData(this, strData);

                                socketStream.BeginWrite(bytData, 0, bytData.Length, new AsyncCallback(OnSendData), socketStream);

                            }
                            catch (SocketException se)
                            {
                                //some kind of a socket error
                                if (ServerError != null)
                                    ServerError(this, "You are not Connected - Can not send:" + serverSetting.UseBNC + ":" + se.Message, false);

                                attemptReconnect = true;
                                disconnectError = true;
                                ForceDisconnect();
                            }
                            catch (NotSupportedException)
                            {
                                //BeginWrite failed because of already trying to send, so add to the sendBuffer Queue
                                //sendBuffer.Enqueue(data);
                            }
                        }
                        else
                        {
                            if (ServerError != null)
                                ServerError(this, "You are not Connected (Socket Disconnected) - Can not send:" + bytData.ToString(), false);

                            attemptReconnect = true;
                            disconnectError = true;
                            ForceDisconnect();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorFile(this, "SendData(b) Error:" + bytData.Length, e);
            }

        }

        /// <summary>
        /// Event fire when Data needs to be sent to the Server Connection
        /// </summary>
        private void OnSendData(IAsyncResult ar)
        {
            SslStream sl = null;
            NetworkStream ns = null;

            if (serverSetting.UseSSL == true)
                sl = (SslStream)ar.AsyncState;
            else
                ns = (NetworkStream)ar.AsyncState;

            try
            {
                //int bytesSent = handler.EndSend(ar);
                if (serverSetting.UseSSL)
                    sl.EndWrite(ar);
                else
                    ns.EndWrite(ar);

                //Check if anything in the sendBuffer Queue, if so, send it
                if (sendBuffer.Count > 0)
                {
                    m_PendingWriteSSL = false;
                    SendData(sendBuffer.Dequeue());
                }

            }
            catch (SocketException se)
            {
                if (ServerError != null)
                    ServerError(this, "SendData Socket Error:" + se.Message.ToString(), false);
            }
            catch (Exception e)
            {
                if (ServerError != null)
                    ServerError(this, "SendData Error:" + e.Message.ToString(), false);

                attemptReconnect = true;
                disconnectError = true;
                ForceDisconnect();
            }
        }

        /// <summary>
        /// Event fired when data is received from the Server Connection
        /// </summary>
        /// <param name="ar"></param>
        private void OnReceivedData(IAsyncResult ar)
        {
            int bytesRead;
            
            try
            {
                if (serverSetting.UseSSL == true)
                    bytesRead = sslStream.EndRead(ar);
                else
                    bytesRead = socketStream.EndRead(ar);

                if (serverSetting.UseProxy && !proxyAuthed)
                {
                    System.Diagnostics.Debug.WriteLine("proxy data:" + bytesRead + ":" + readBuffer.Length);
                    if (serverSetting.ProxyType == 2)
                    {
                        if (bytesRead > 1)
                        {
                            /*
                            #define SOCKS4_REP_SUCCEEDED	90	 rquest granted (succeeded) 
                            #define SOCKS4_REP_REJECTED	    91	 request rejected or failed
                            #define SOCKS4_REP_IDENT_FAIL	92	 cannot connect identd 
                            #define SOCKS4_REP_USERID	    93	user id not matched 
                            */
                            //
                            if ((int)readBuffer[1] == 90)  //connection succeeded
                            {
                                //send the USER info
                                proxyAuthed = true;

                                ServerMessage(this, "Socks 4 Connection Successfull", "");

                                ServerPreConnect(this);

                                if (serverSetting.Password != null && serverSetting.Password.Length > 0)
                                    SendData("PASS " + serverSetting.Password);

                                SendData("CAP LS 302");

                                //send the USER / NICK stuff
                                SendData("NICK " + serverSetting.NickName);
                                SendData("USER " + serverSetting.IdentName + " \"localhost\" \"" + serverSetting.ServerName + "\" :" + serverSetting.FullName);

                                whichAddressinList = whichAddressCurrent;

                                ServerMessage(this, "Sending User Registration Information", "");

                                whichAddressinList = whichAddressCurrent;

                                readBuffer = new byte[BUFFER_SIZE];
                                if (serverSetting.UseSSL)
                                    sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                                else
                                    socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);

                            }
                            else
                            {
                                ServerError(this, "Proxy Server Error: Connection Failed: Code " + (int)readBuffer[1], false);
                                ForceDisconnect();

                            }
                        }
                        else
                        {
                            ServerError(this, "Proxy Server Error: Connection Failed", false);
                            ForceDisconnect();
                        }

                        //System.Diagnostics.Debug.WriteLine("got:" + (int)strData[0] + ":" + (int)strData[1] + ":" + strData.Length);                    
                    }                    
                    else if (serverSetting.ProxyType == 3)
                    {
                        if (bytesRead == 2)
                        {
                            //System.Diagnostics.Debug.WriteLine("got:" + (int)strData[0] + ":" + (int)strData[1]);

                            if (readBuffer[1] == 0xFF)
                            {
                                if (ServerError != null)
                                    ServerError(this, "Proxy Server Error: None of the authentication method was accepted by proxy server.", false);
                                ForceDisconnect();
                            }
                            else if (readBuffer[1] == 0x00)  //send proxy information
                            {
                                byte[] proxyData = new byte[7 + serverSetting.ServerName.Length];
                                proxyData[0] = 0x05;
                                proxyData[1] = 0x01;
                                proxyData[2] = 0x00;
                                proxyData[3] = 0x03;    //0x03 for a domain name
                                proxyData[4] = Convert.ToByte(serverSetting.ServerName.Length);
                                byte[] rawBytes = new byte[serverSetting.ServerName.Length];
                                rawBytes = Encoding.Default.GetBytes(serverSetting.ServerName);
                                //System.Diagnostics.Debug.WriteLine(rawBytes.Length + ":" + serverSetting.ServerName.Length);
                                rawBytes.CopyTo(proxyData, 5);
                                proxyData[proxyData.Length - 2] = (byte)((Convert.ToInt32(serverSetting.ServerPort) & 0xFF00) >> 8);
                                proxyData[proxyData.Length - 1] = (byte)(Convert.ToInt32(serverSetting.ServerPort) & 0xFF);
                                ServerMessage(this, "Sending Proxy Verification", "");
                                //System.Diagnostics.Debug.WriteLine(Convert.ToInt16(proxyData[proxyData.Length - 2]));
                                SendData(proxyData);

                                if (serverSetting.UseSSL)
                                    sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                                else
                                    socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);

                            }
                            else if (readBuffer[1] == 0x02)  //send proxy information with user/pass
                            {
                                ushort nIndex = 0;

                                byte[] proxyData = new byte[256];
                                proxyData[nIndex++] = 0x05;
                                proxyData[nIndex++] = 0x01;
                                proxyData[nIndex++] = 0x00;
                                proxyData[nIndex++] = 0x03;
                                proxyData[nIndex++] = Convert.ToByte(serverSetting.ServerName.Length);

                                byte[] rawBytes = new byte[256];
                                rawBytes = Encoding.ASCII.GetBytes(serverSetting.ServerName);
                                rawBytes.CopyTo(proxyData, nIndex);
                                nIndex += (ushort)rawBytes.Length;

                                proxyData[proxyData.Length - 2] = (byte)((Convert.ToInt32(serverSetting.ServerPort) & 0xFF00) >> 8);
                                proxyData[proxyData.Length - 1] = (byte)(Convert.ToInt32(serverSetting.ServerPort) & 0xFF);
                                ServerMessage(this, "Sending Proxy Verification (user/pass)", "");
                                SendData(proxyData);

                                if (serverSetting.UseSSL)
                                    sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                                else
                                    socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);
                            }
                        }
                        else if (bytesRead == 10)
                        {
                            //System.Diagnostics.Debug.WriteLine("got10:" + (int)strData[0] + ":" + (int)strData[1]);
                            if (readBuffer[1] == 0x00)
                            {
                                proxyAuthed = true;

                                ServerMessage(this, "Socks 5 Connection Successfull", "");

                                ServerPreConnect(this);

                                if (serverSetting.Password != null && serverSetting.Password.Length > 0)
                                    SendData("PASS " + serverSetting.Password);

                                //check for ircv3 capabilities
                                SendData("CAP LS 302");

                                //send the USER / NICK stuff
                                SendData("NICK " + serverSetting.NickName);
                                SendData("USER " + serverSetting.IdentName + " \"localhost\" \"" + serverSetting.ServerName + "\" :" + serverSetting.FullName);

                                whichAddressinList = whichAddressCurrent;

                                ServerMessage(this, "Sending User Registration Information", "");

                                whichAddressinList = whichAddressCurrent;

                                readBuffer = new byte[BUFFER_SIZE];
                                if (serverSetting.UseSSL)
                                    sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                                else
                                    socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);
                            }
                            else
                            {
                                ServerMessage(this, "Socks 5 Connection Error : " + (int)readBuffer[1], "");
                                ForceDisconnect();
                            }
                        }
                    }
                }
                else
                {
                    if (bytesRead > 0)
                    {
                        //check for UTF8 coding here

                        String strData = String.Empty;
                        
                        //check if AutoDetect of Encoding is enabled
                        //System.Diagnostics.Debug.WriteLine(serverSetting.Encoding.ToLower());
                        //if (this.serverSetting.AutoDecode == false)
                        if (this.serverSetting.Encoding.ToLower() == "utf-8")
                        {
                            
                            Decoder decoder = Encoding.UTF8.GetDecoder();
                            char[] chars = new char[decoder.GetCharCount(readBuffer, 0, bytesRead)];
                            int charLen = decoder.GetChars(readBuffer, 0, bytesRead, chars, 0);
                            strData = new String(chars,0,charLen);

                            //byte[] bytes = Encoding.Default.GetBytes(readBuffer,0,bytesRead);
                            //System.Diagnostics.Debug.WriteLine(readBuffer.ToString());


                            //this makes ? marks
                            //UTF8Encoding utf8 = new UTF8Encoding();
                            //strData = utf8.GetString(readBuffer, 0, bytesRead);

                            //this works well - but is it right? doesnt look like it
                            //strData = Encoding.Default.GetString(readBuffer, 0, bytesRead);

                            //no (UTF8Encoding)
                            //System.Diagnostics.Debug.WriteLine("1:" + new UTF8Encoding().GetString(readBuffer,0,bytesRead));
                            //yes
                            //System.Diagnostics.Debug.WriteLine("2:" + Encoding.Default.GetString(readBuffer, 0, bytesRead));
                            //no
                            //System.Diagnostics.Debug.WriteLine("3:" + Encoding.UTF8.GetString(readBuffer, 0, bytesRead));
                            //big no
                            //System.Diagnostics.Debug.WriteLine("4:" + Encoding.Unicode.GetString(readBuffer, 0, bytesRead));

                            //Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytesRead, 0, bytesRead);

                        }
                        else
                        {
                            strData = Encoding.GetEncoding(serverSetting.Encoding).GetString(readBuffer,0,bytesRead);
                            while (strData.EndsWith(Convert.ToChar(0x0).ToString()))
                                strData = strData.Substring(0, strData.Length - 1);
                        }

                        strData = strData.Replace("\r", string.Empty);

                        //System.Diagnostics.Debug.WriteLine(strData);
                        //System.Diagnostics.Debug.WriteLine(DateTime.Now + "!" + dataBuffer.Length + ":" + dataBuffer);

                        if (!strData.EndsWith("\n"))
                        {
                            //create a buffer
                            dataBuffer += strData;
                            readBuffer = new byte[BUFFER_SIZE];
                            //System.Diagnostics.Debug.WriteLine("get more:" + dataBuffer);
                            if (serverSetting.UseSSL)
                                sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                            else
                                socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);
                            return;
                        }

                        if (dataBuffer.Length > 0)
                        {
                            strData = dataBuffer + strData;
                            dataBuffer = string.Empty;
                        }

                        //split into lines and stuff
                        if (strData.IndexOf('\n') > -1)
                        {
                            string[] Data = strData.Split('\n');
                            foreach (string Line in Data)
                            {
                                if (Line.Length > 0)
                                {
                                    string strLine = Line.Replace("\0", string.Empty);
                                    //System.Diagnostics.Debug.WriteLine("Parse1:" + strLine);
                                    ParseData(strLine);
                                }
                            }
                        }
                        else
                        {
                            if (strData.Length > 0)
                            {
                                //strip out NULL chars
                                strData = strData.Replace("\0", string.Empty);
                                //System.Diagnostics.Debug.WriteLine("Parse2:" + strData);
                                ParseData(strData);
                            }
                        }
                        
                        readBuffer = new byte[BUFFER_SIZE];
                        if (serverSetting.UseSSL)
                            sslStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), sslStream);
                        else
                            socketStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(OnReceivedData), socketStream);
                    
                        
                    }
                    else
                    {
                        //connection lost                    
                        if (ServerError != null)
                            ServerError(this, "Connection Lost", false);
                        
                        ForceDisconnect();


                    }
                }
            }
            catch (SocketException se)
            {
                ServerError(this, "OnReceivedData Socket Exception Error #" + se.SocketErrorCode + " : " + se.Message, false);
                disconnectError = true;
                attemptReconnect = true;
                
                ForceDisconnect();
            }
            catch (Exception e)
            {
                ServerError(this, "OnReceivedData Exception Error:" + e.Message, false);
                //ServerError(this, "OnReceivedData Exception Error:" + e.StackTrace, false);
                //ServerError(this, "OnReceivedData Exception Error:" + e.InnerException.Message, false);
                //ServerError(this, "OnReceivedData Exception Error:" + socketStream.CanRead + ":" + socketStream.DataAvailable, false);
                
                disconnectError = true;
                attemptReconnect = true;

                ForceDisconnect();
            }
        }

        /// <summary>
        /// Method for starting a Server Connection
        /// </summary>
        public void ConnectSocket()
        {
            disconnectError = false;

            IPHostEntry hostEntry = null;
            try
            {
                // Get host related information.
                if (serverSetting.UseProxy == true)
                {
                    whichAddressCurrent = 1;
                    totalAddressinDNS = 1;
                    IPAddress proxyIP = null;

                    try
                    {
                        proxyIP = IPAddress.Parse(serverSetting.ProxyIP);
                    }
                    catch (FormatException)
                    {
                        hostEntry = Dns.GetHostEntry(serverSetting.ProxyIP);

                        if (IPAddress.TryParse(serverSetting.ProxyIP, out proxyIP))
                        {
                            //resolves just fine
                        }
                        else
                        {
                            if (hostEntry.AddressList.Length > 0)
                            {
                                proxyIP = hostEntry.AddressList[0];
                            }
                            else
                            {
                                ServerError(this, "Can not resolve Proxy Server " + serverSetting.ProxyIP, false);
                            }
                        }
                    }

                    try
                    {
                        IPEndPoint proxyEndPoint = new IPEndPoint(proxyIP, Convert.ToInt32(serverSetting.ProxyPort));
                        Socket proxySocket = new Socket(proxyEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        proxySocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                        hostEntry = Dns.GetHostEntry(serverSetting.ServerName);
                        if (hostEntry.AddressList.Length > 0)
                        {
                            ServerConnect(this, hostEntry.AddressList[0].ToString());
                            serverSocket = proxySocket;
                            proxySocket.BeginConnect(proxyEndPoint, new AsyncCallback(OnConnectionReady), proxySocket);
                        }
                        else
                        {
                            //cant resolve IP for server
                            ServerConnect(this, serverSetting.ServerName);
                            serverSocket = proxySocket;
                            proxySocket.BeginConnect(proxyEndPoint, new AsyncCallback(OnConnectionReady), proxySocket);
                        }
                    }
                    catch (SocketException se)
                    {
                        ServerError(this, "Connect Socket Error (Proxy) " + se.Message,false);
                        System.Diagnostics.Debug.WriteLine("Socket Exception Proxy Connect:" + se.Message);
                    }
                    catch (Exception e)
                    {
                        ServerError(this, "Connect Exception Error (Proxy) " + e.Message, false);
                        System.Diagnostics.Debug.WriteLine("Exception Proxy Connect:" + e.Message + ":" + e.StackTrace);
                    }
                }
                else if (serverSetting.UseBNC == true)
                {
                    //start connection with BNC server
                    whichAddressCurrent = 1;
                    totalAddressinDNS = 1;
                    IPAddress bncIP = null;

                    try
                    {
                        bncIP = IPAddress.Parse(serverSetting.BNCIP);
                    }
                    catch (FormatException)
                    {
                        hostEntry = Dns.GetHostEntry(serverSetting.BNCIP);

                        if (IPAddress.TryParse(serverSetting.BNCIP, out bncIP))
                        {
                            //resolves just fine
                        }
                        else
                        {
                            if (hostEntry.AddressList.Length > 0)
                            {
                                bncIP = hostEntry.AddressList[0];
                            }
                            else
                            {
                                ServerError(this, "Can not resolve BNC Server " + serverSetting.BNCIP, false);
                            }
                        }
                    }

                    try
                    {
                        IPEndPoint bncEndPoint = new IPEndPoint(bncIP, Convert.ToInt32(serverSetting.BNCPort));
                        Socket bncSocket = new Socket(bncEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        bncSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
                        //bncSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

                        ServerConnect(this, bncIP.ToString());

                        serverSocket = bncSocket;
                        bncSocket.BeginConnect(bncEndPoint, new AsyncCallback(OnConnectionReady), bncSocket);
                    }
                    catch (SocketException)
                    {
                        System.Diagnostics.Debug.WriteLine("Socket Exception BNC Connect");
                    }
                    catch (Exception)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception BNC Connect");
                    }

                }
                else if (serverSetting.UseIPv6 == true)
                {
                    //connect to an IPv6 Server
                    System.Diagnostics.Debug.WriteLine("try ipv6:" + Socket.OSSupportsIPv6);
                    
                    
                    hostEntry = Dns.GetHostEntry(serverSetting.ServerName);
                    IPHostEntry hosts = Dns.GetHostEntry(serverSetting.ServerName);
                    
                    whichAddressCurrent = 1;
                    totalAddressinDNS = hostEntry.AddressList.Length;

                    if (whichAddressinList > totalAddressinDNS)
                        whichAddressinList = 1; 

                    foreach (IPAddress address in hosts.AddressList)
                    {                        
                        System.Diagnostics.Debug.WriteLine(address.AddressFamily.ToString() + ":" + address.ToString());
                        try
                        {
                            if (whichAddressCurrent == whichAddressinList)
                            {
                                if (address.AddressFamily == AddressFamily.InterNetworkV6)
                                {
                                    System.Diagnostics.Debug.WriteLine("Connect to ipv6:" + address.ToString() + ":" + whichAddressCurrent);
                                    IPEndPoint ipe = new IPEndPoint(address, Convert.ToInt32(serverSetting.ServerPort));
                                    serverSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                                    serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                                    serverSetting.ServerIP = address.ToString();
                                    ServerConnect(this, address.ToString());

                                    serverSocket.BeginConnect(ipe, new AsyncCallback(OnConnectionReady), serverSocket);
                                    break;
                                }
                            }
                            
                            whichAddressCurrent++;
                            
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                    whichAddressCurrent = 1;

                            
                        }
                        catch (Exception e)
                        {
                            if (ServerError != null)
                                ServerError(this, "Connect - IPV6 Exception Error:" + e.InnerException.ToString() + ":" + e.Source + ":" + e.StackTrace.ToString() + ":" + e.Message.ToString(), false);

                            whichAddressCurrent++;
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                whichAddressCurrent = 1;

                            disconnectError = true;
                            ForceDisconnect();
                        }
                        finally
                        {
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                whichAddressCurrent = 1;
                        }
                        
                    }

                    //String[] alias = hostEntry.Aliases;
                    /*
                    IPAddress[] local = Dns.GetHostAddresses(serverSetting.ServerName);                    
                    for (int index = 0; index < local.Length; index++)
                    {
                        //System.Diagnostics.Debug.WriteLine("::" + local[index] + ":" + local[index].IsIPv6LinkLocal + ":" +local[index].AddressFamily.ToString());
                    }
                    */
                    System.Diagnostics.Debug.WriteLine("Host name: " + hostEntry.HostName + ":" + hostEntry.AddressList.Length + ":" + hostEntry.Aliases.Length);
                    /*
                    //System.Diagnostics.Debug.WriteLine("Aliases :");
                    for (int index = 0; index < alias.Length; index++)
                    {
                        //System.Diagnostics.Debug.WriteLine(alias[index]);
                    }
                    System.Diagnostics.Debug.WriteLine("IP address list:");
                    for (int index = 0; index < address.Length; index++)
                    {
                        //System.Diagnostics.Debug.WriteLine(address[index] + ":" + address[index].IsIPv6LinkLocal + ":" + address[index].AddressFamily.ToString());
                    }
                    */
                    //FAULT IN THIS CODE WILL NOT PROPERLY PARSE IPv6
                    /* // this below fails on IPV6
                    IPAddress ipAddress = null;
                    if (IPAddress.TryParse(serverSetting.ServerName, out ipAddress))
                    {
                        if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            System.Diagnostics.Debug.WriteLine("success parse as ipv6 address");

                            IPEndPoint ipe = new IPEndPoint(ipAddress, Convert.ToInt32(serverSetting.ServerPort));
                            serverSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

                            serverSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, 0);
                            serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);                            

                            System.Diagnostics.Debug.WriteLine("ipv6 connect:" + ipe.AddressFamily.ToString() + ":" + ipAddress.ToString());

                            ServerConnect(this, ipAddress.ToString());

                            serverSetting.ServerIP = ipAddress.ToString();

                            System.Diagnostics.Debug.WriteLine("start ipv6 connect here:" + ipe.Address + ":" + ipAddress);
                            serverSocket.BeginConnect(ipe, new AsyncCallback(OnConnectionReady), serverSocket);
                        }
                        return;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("can not get ip of ipv6 address");
                        ServerError(this, "Can not resolve IPV6 Address ("+serverSetting.ServerName+")", false);
                        disconnectError = true;                        
                        ForceDisconnect();

                    }
                    */ 
                }
                else
                {
                    //this will fail on an IPv6 address
                    IPAddress ipAddress = null;
                    if (IPAddress.TryParse(serverSetting.ServerName, out ipAddress))
                    {
                        IPEndPoint ipe = new IPEndPoint(ipAddress, Convert.ToInt32(serverSetting.ServerPort));
                        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

                        serverSetting.ServerIP = ipAddress.ToString();
                        ServerConnect(this, ipAddress.ToString());

                        serverSocket.BeginConnect(ipe, new AsyncCallback(OnConnectionReady), serverSocket);
                        return;
                    }
                    hostEntry = Dns.GetHostEntry(serverSetting.ServerName);

                    whichAddressCurrent = 1;
                    totalAddressinDNS = hostEntry.AddressList.Length;

                    if (whichAddressinList > totalAddressinDNS)
                        whichAddressinList = 1; 


                    // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
                    // an exception that occurs when the host IP Address is not compatible with the address family
                    // (typical in the IPv6 case).
                    foreach (IPAddress address in hostEntry.AddressList)
                    {
                        try
                        {
                            if (whichAddressCurrent == whichAddressinList)
                            {

                                IPEndPoint ipe = new IPEndPoint(address, Convert.ToInt32(serverSetting.ServerPort));
                                serverSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                                serverSetting.ServerIP = address.ToString();
                                ServerConnect(this, address.ToString());

                                serverSocket.BeginConnect(ipe, new AsyncCallback(OnConnectionReady), serverSocket);
                                break;
                            }
                            whichAddressCurrent++;
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                whichAddressCurrent = 1;
                        }
                        catch (Exception e)
                        {
                            if (ServerError != null)
                                ServerError(this, "Connect - Exception Error:" + e.InnerException.ToString() + ":" + e.Source + ":" + e.StackTrace.ToString() + ":" + e.Message.ToString(), false);

                            whichAddressCurrent++;
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                whichAddressCurrent = 1;

                            disconnectError = true;
                            ForceDisconnect();
                        }
                        finally
                        {
                            if (whichAddressCurrent > hostEntry.AddressList.Length)
                                whichAddressCurrent = 1;
                        }
                    }
                }
            }
            catch (NotSupportedException nse)
            {
                System.Diagnostics.Debug.WriteLine("not supported:" + nse.Message);
            }
            catch (SocketException se)
            {
                if (ServerError != null)
                    ServerError(this, "Socket Error " + se.ErrorCode + " :" + se.Message + " - http://msdn.microsoft.com/en-us/library/ms740668.aspx for further information", false);

                System.Diagnostics.Debug.WriteLine(se.StackTrace);
                
                disconnectError = true;
                ForceDisconnect();
            }
        }

        private bool RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                return true;
            }
            
            //check if you allow it to accept connections with invalid certificates
            if (serverSetting.SSLAcceptInvalidCertificate)
                return true;

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }


        public void ForceDisconnect()
        {
            try
            {
                if (sslStream != null)
                {
                    //System.Diagnostics.Debug.WriteLine("ssl stream not null:" + sslStream.CanRead);
                    sslStream.Close();
                    socketStream.Close();
                    
                    ServerDisconnect(this);

                }
                
                else if (serverSocket != null)
                {
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.BeginDisconnect(false, new AsyncCallback(OnDisconnect), serverSocket);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FD:" + ex.Message);
            }

            OnDisconnected();
            ServerForceDisconnect(this);

        }

        #endregion


        #region Timer Events

        public void CreateTimer(string id, int reps, double interval, string command)
        {
            IrcTimer timer = new IrcTimer(id, reps, interval * 1000, command);
            timer.OnTimerElapsed += new IrcTimer.TimerElapsed(OnTimerElapsed);
            ircTimers.Add(timer);
            timer.Start();
        }

        public List<IrcTimer> IRCTimers
        {
            get { return this.ircTimers; }
        }

        public void DestroyTimer(string id)
        {
            IrcTimer timer = ircTimers.Find(
                delegate(IrcTimer t)
                {
                    return t.TimerID == id;
                }
            );

            if (timer != null)
            {
                timer.Stop();
                ircTimers.Remove(timer);
            }

        }

        private void OnTimerElapsed(string timerID, string command)
        {
            OutGoingCommand(this, command);
        }

        public void SetAutoAwayTimer(int minutes)
        {
            autoAwayTimer.Stop();
            autoAwayTimer.Interval = minutes * 60000;
            autoAwayTimer.Start();
        }

        public void DisableAutoAwayTimer()
        {
            autoAwayTimer.Stop();
        }

        private void OnAutoAwayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (AutoAwayTrigger != null)
            {
                if (serverSetting.Away == false)
                {
                    AutoAwayTrigger(this);
                    autoAwayTimer.Stop();
                }
            }
        }

        #endregion
    }

}

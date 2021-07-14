/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2021 Paul Vanderzee <snerf@icechat.net>
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
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace IceChat
{
    public class IdentServer
    {

        private TcpListener identServer;
        private Thread identThread;

        public IdentServer()
        {
            identThread = new Thread(new ThreadStart(Listen))
            {
                Name = "IdentServerThread"
            };
            identThread.Start();
        }

        private void Listen()
        {
            try
            {
                identServer = new TcpListener(IPAddress.Any, 113);
                identServer.Start();

                while (true)
                {
                    TcpClient client = identServer.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(IncomingData))
                    {
                        Name = "IdentClientThread"
                    };
                    clientThread.Start(client);
                }

            }
            catch (ThreadAbortException)
            {
                //FormMain.Instance.WriteErrorFile("IdentServer ThreadAbort Error:" + ex.Message, ex.StackTrace);
            }
            catch (SocketException)
            {
                System.Diagnostics.Debug.WriteLine("Ident socket exception listen error");
            }
            catch (Exception)
            {
                //FormMain.Instance.WriteErrorFile("IdentServer Exception Error:" + e.Message, e.StackTrace);
            }
        }

        private void IncomingData(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;
                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                string msg = encoder.GetString(message, 0, bytesRead);
                //System.Diagnostics.Debug.WriteLine("IdentMSG: " + msg);

                if (msg.Contains(","))
                {
                    try
                    {
                        string[] ident = msg.Split(',');
                        string sendIdent = ident[0].Trim() + ", " + ident[1].Trim();
                        sendIdent += " : USERID : UNIX : " + FormMain.Instance.InputPanel.CurrentConnection.ServerSetting.IdentName;
                        byte[] buffer = encoder.GetBytes(sendIdent + "\n");

                        clientStream.Write(buffer, 0, buffer.Length);
                        clientStream.Flush();
                    }
                    catch (NullReferenceException)
                    {
                        /* Occurs if another IRC client is running and connects to a server. */
                    }
                }
            }
            
            tcpClient.Close();
        }

        public void Stop()
        {
            try
            {
                if (identServer != null)
                    identServer.Stop();

                if (identThread != null)
                    if (identThread.IsAlive)
                        identThread.Join(1000);

            }
            catch (SocketException)
            {
                //
            }
            catch (Exception)
            {
                //
            }
        }
    }
}

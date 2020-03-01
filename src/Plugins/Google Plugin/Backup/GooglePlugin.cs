/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2011 Paul Vanderzee <snerf@icechat.net>
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
using System.Windows.Forms;
using System.ComponentModel;

namespace IceChatPlugin
{
    public class Plugin : IPluginIceChat
    {
        
        //all the events get declared here
        public override event OutGoingCommandHandler OnCommand;

        //declare the standard properties
        private string m_Name;
        private string m_Author;
        private string m_Version;

        public override string Name { get { return m_Name; } }
        public override string Version { get { return m_Version; } }
        public override string Author { get { return m_Author; } }

        public Plugin()
        {
            //set your default values here
            m_Name = "Google Plugin";
            m_Author = "Snerf";
            m_Version = "1.5";
        }

        //declare the standard methods

        public override void Dispose()
        {

        }

        public override void Initialize()
        {

        }

        struct ParseArgs
        {
            public PluginArgs args;
            public string url;
            public bool self;
        }

        //if you want to add a new method to override, use public override

        public override PluginArgs ChannelMessage(PluginArgs args)
        {
            if (args.Extra.StartsWith("!google"))
            {
                String search = args.Extra.Substring(8);
                search = search.Replace(" ","%20");
                search = search.Replace("&", "&amp;");
                string url = "http://www.google.ca/search?q=" + search;


                ParseArgs pa = new ParseArgs();
                pa.args = args;
                pa.url = url;
                pa.self = false;

                //ParseGoogleResults(args, url, false);
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerAsync(pa);    

            }
            return base.ChannelMessage(args);
        }

        public override PluginArgs InputText(PluginArgs args)
        {
            if (args.Command.StartsWith("!google"))
            {
                String search = args.Command.Substring(8);
                search = search.Replace(" ", "%20");
                search = search.Replace("&", "&amp;");
                string url = "http://www.google.ca/search?q=" + search;
                
                args.Command = "/say " + args.Command;
                OnCommand(args);
               
                
                ParseArgs pa = new ParseArgs();
                pa.args = args;
                pa.url = url;
                pa.self = true;
                
                BackgroundWorker bgw = new BackgroundWorker();
                bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                bgw.RunWorkerAsync(pa);    
                
                //ParseGoogleResults(args, url, true);
                
                args.Command = "";
            }
            
            return base.InputText(args);
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            ParseArgs pa = (ParseArgs) e.Argument;
            ParseGoogleResults(pa.args, pa.url, pa.self);
        }

        private void ParseGoogleResults(PluginArgs args, String url, bool self)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            string html = web.DownloadString(url);

            string previousLink = "";

            // put a max timer on this
            int tickCount = System.Environment.TickCount;

            if (html.Length > 0)
            {
                //int searchDiv = html.IndexOf("Search Results");
                //                
                //int searchDiv = html.IndexOf("id=\"topstuff\"");
                // All Results
                int searchDiv = html.IndexOf("id=\"main\"");                
                
                System.Diagnostics.Debug.WriteLine("searchDiv:" + searchDiv);
                if (searchDiv > -1)
                {
                    //System.Diagnostics.Debug.WriteLine(html.Substring(searchDiv));
                    //do a loop
                    int counter = 0;
                    PluginArgs a = new PluginArgs(args.Connection);
                    do
                    {
                        // class="g" nolonger
                        int i = html.IndexOf("<div class=\"jfp3ef\"", searchDiv);
                        
                        System.Diagnostics.Debug.WriteLine("search g:" + i + ":" + counter);

                        if (i > 0)
                        {
                            //counter++;

                            if (System.Environment.TickCount - tickCount > 10000)
                            {
                                break;
                            }


                            int x = html.IndexOf("<a href=\"/url?q=", i + 1) + 16;
                            int y = html.IndexOf("\"", x + 1);
                            System.Diagnostics.Debug.WriteLine("x=" + x);
                            //extract the url
                            System.Diagnostics.Debug.WriteLine("link=" + html.Substring(x, y - x));
                            string link = html.Substring(x, y - x);
                            //now cut off the link to the 1st &amp;

                            link = link.Substring(0, link.IndexOf("&amp;"));
                            link = link.Replace("%3F", "?");
                            link = link.Replace("%3D", "=");

                            int y2 = html.IndexOf(">", y) + 1;
                            
                            //int y2 = html.IndexOf("AP7Wnd\">", y) + 8;

                            int z = html.IndexOf("</a>", y2);

                            System.Diagnostics.Debug.WriteLine("z:" + html.Substring(y2, z - y2));
                            
                            // z:<div class="BNeawe vvjwJb AP7Wnd">How
                            // z:<div class="BNeawe vvjwJb AP7Wnd">IceChat IRC Client :: IRC Client Extra-Ordinaire :: The Chat Cool ...</div><div class="BNeawe UPmit AP7Wnd">https://icechat.net</div>
                            // z:<span class="XLloXe AP7Wnd">Downloads</span>
                            // z:<img class="lhgpye" alt="" src="data:image/gif;base64,R0lGODlhAQABAIAAAP///////yH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==" style="max-width:72px;max-height:72px" id="dimg_11" data-deferred="1">

                            string desc = html.Substring(y2, z - y2);

                            int zz = desc.IndexOf("AP7Wnd\">");
                            if (zz > -1)
                            {
                                desc = desc.Substring(zz + 8);

                                int z2 = desc.IndexOf("</");
                                if (z2 > -1)
                                {
                                    desc = desc.Substring(0, z2);
                                }
                            }
                            else
                            {
                                desc = "";
                            }
                            
                            desc = desc.Replace("<em>", "");
                            desc = desc.Replace("</em>", "");
                            desc = desc.Replace("<b>", "");
                            desc = desc.Replace("</b>", "");
                            desc = desc.Replace("&amp;", "&");


                            searchDiv = html.IndexOf("<div class=\"jfp3ef\"", z) - 1;
                            System.Diagnostics.Debug.WriteLine("zSearchDiv:" + z + ":" + searchDiv);

                            if (desc.Length > 0)
                            {

                                if (!link.StartsWith("/search?q="))
                                {

                                    if (link != previousLink)
                                    {

                                        counter++;

                                        if (self == false)
                                        {
                                            a.Command = "/notice " + args.Nick + " " + link + " : " + desc;
                                            OnCommand(a);
                                        }
                                        else
                                        {
                                            a.Command = "/echo " + link + " : " + desc;
                                            OnCommand(a);
                                        }
                                        
                                        previousLink = link;

                                    }
                                }
                            }

                            System.Diagnostics.Debug.WriteLine(link);
                        }
                        System.Diagnostics.Debug.WriteLine(searchDiv + ":" + html.Length);
                        
                        if (searchDiv >= html.Length || i == -1)
                        {
                            break;
                        }
                    } while (counter < 5);
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(html);
                }


            }
        }

    }
}

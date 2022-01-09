/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2022 Paul Vanderzee <snerf@icechat.net>
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
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;

using IceChat.Properties;
using IceChatPlugin;

namespace IceChat
{
    public partial class FormMain
    {
        private void LoadDefaultMessageSettings()
        {
            IceChatMessageFormat oldMessage = new IceChatMessageFormat
            {
                MessageSettings = new ServerMessageFormatItem[49]
            };

            if (iceChatMessages.MessageSettings != null)
                iceChatMessages.MessageSettings.CopyTo(oldMessage.MessageSettings, 0);
            
            iceChatMessages.MessageSettings = new ServerMessageFormatItem[49];

            if (oldMessage.MessageSettings[0] == null || oldMessage.MessageSettings[0].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[0] = NewMessageFormat("Server Connect", "&#x3;00*** Attempting to connect to $server ($serverip) on port $port");
            else
                iceChatMessages.MessageSettings[0] = oldMessage.MessageSettings[0];

            if (oldMessage.MessageSettings[1] == null || oldMessage.MessageSettings[1].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[1] = NewMessageFormat("Server Disconnect", "&#x3;04*** Server disconnected on $server");
            else
                iceChatMessages.MessageSettings[1] = oldMessage.MessageSettings[1];

            if (oldMessage.MessageSettings[2] == null || oldMessage.MessageSettings[2].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[2] = NewMessageFormat("Server Reconnect", "&#x3;00*** Attempting to re-connect to $server");
            else
                iceChatMessages.MessageSettings[2] = oldMessage.MessageSettings[2];

            if (oldMessage.MessageSettings[3] == null || oldMessage.MessageSettings[3].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[3] = NewMessageFormat("Channel Invite", "&#x3;00* $nick invites you to $channel");
            else
                iceChatMessages.MessageSettings[3] = oldMessage.MessageSettings[3];

            if (oldMessage.MessageSettings[7] == null || oldMessage.MessageSettings[7].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[7] = NewMessageFormat("Channel Mode", "&#x3;04* $nick sets mode $mode $modeparam for $channel");
            else
                iceChatMessages.MessageSettings[7] = oldMessage.MessageSettings[7];

            if (oldMessage.MessageSettings[8] == null || oldMessage.MessageSettings[8].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[8] = NewMessageFormat("Server Mode", "&#x3;09* Your mode is now $mode");
            else
                iceChatMessages.MessageSettings[8] = oldMessage.MessageSettings[8];

            if (oldMessage.MessageSettings[9] == null || oldMessage.MessageSettings[9].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[9] = NewMessageFormat("Server Notice", "&#x3;09*** $server $message");
            else
                iceChatMessages.MessageSettings[9] = oldMessage.MessageSettings[9];

            if (oldMessage.MessageSettings[10] == null || oldMessage.MessageSettings[10].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[10] = NewMessageFormat("Server Message", "&#x3;00-$server- $message");
            else
                iceChatMessages.MessageSettings[10] = oldMessage.MessageSettings[10];

            if (oldMessage.MessageSettings[11] == null || oldMessage.MessageSettings[11].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[11] = NewMessageFormat("User Notice", "&#x3;00--$nick-- $message");
            else
                iceChatMessages.MessageSettings[11] = oldMessage.MessageSettings[11];

            if (oldMessage.MessageSettings[12] == null || oldMessage.MessageSettings[12].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[12] = NewMessageFormat("Channel Message", "&#x3;00<$color$status$nick&#x3;> $message");
            else
                iceChatMessages.MessageSettings[12] = oldMessage.MessageSettings[12];

            if (oldMessage.MessageSettings[13] == null || oldMessage.MessageSettings[13].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[13] = NewMessageFormat("Self Channel Message", "&#x3;08<$nick&#x3;> $message");
            else
                iceChatMessages.MessageSettings[13] = oldMessage.MessageSettings[13];

            if (oldMessage.MessageSettings[14] == null || oldMessage.MessageSettings[14].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[14] = NewMessageFormat("Channel Action", "&#x3;13* $nick $message");
            else
                iceChatMessages.MessageSettings[14] = oldMessage.MessageSettings[14];

            if (oldMessage.MessageSettings[15] == null || oldMessage.MessageSettings[15].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[15] = NewMessageFormat("Self Channel Action", "&#x3;13* $nick $message");
            else
                iceChatMessages.MessageSettings[15] = oldMessage.MessageSettings[15];

            if (oldMessage.MessageSettings[16] == null || oldMessage.MessageSettings[16].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[16] = NewMessageFormat("Channel Join", "&#x3;07* $nick ($host) has joined channel $channel");
            else
                iceChatMessages.MessageSettings[16] = oldMessage.MessageSettings[16];

            if (oldMessage.MessageSettings[17] == null || oldMessage.MessageSettings[17].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[17] = NewMessageFormat("Self Channel Join", "&#x3;04* You have joined $channel");
            else
                iceChatMessages.MessageSettings[17] = oldMessage.MessageSettings[17];

            if (oldMessage.MessageSettings[18] == null || oldMessage.MessageSettings[18].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[18] = NewMessageFormat("Channel Part", "&#x3;03* $nick ($host) has left $channel ($reason)");
            else
                iceChatMessages.MessageSettings[18] = oldMessage.MessageSettings[18];

            if (oldMessage.MessageSettings[19] == null || oldMessage.MessageSettings[19].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[19] = NewMessageFormat("Self Channel Part", "&#x3;04* You have left $channel - You will be missed &#x3;10($reason)");
            else
                iceChatMessages.MessageSettings[19] = oldMessage.MessageSettings[19];

            if (oldMessage.MessageSettings[20] == null || oldMessage.MessageSettings[20].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[20] = NewMessageFormat("Server Quit", "&#x3;09* $nick ($host) Quit ($reason)");
            else
                iceChatMessages.MessageSettings[20] = oldMessage.MessageSettings[20];

            if (oldMessage.MessageSettings[21] == null || oldMessage.MessageSettings[21].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[21] = NewMessageFormat("Channel Nick Change", "&#x3;07* $nick is now known as $newnick");
            else
                iceChatMessages.MessageSettings[21] = oldMessage.MessageSettings[21];

            if (oldMessage.MessageSettings[22] == null || oldMessage.MessageSettings[22].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[22] = NewMessageFormat("Self Nick Change", "&#x3;04* You are now known as $newnick");
            else
                iceChatMessages.MessageSettings[22] = oldMessage.MessageSettings[22];

            if (oldMessage.MessageSettings[23] == null || oldMessage.MessageSettings[23].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[23] = NewMessageFormat("Channel Kick", "&#x3;08* $kickee was kicked by $nick($host) &#x3;03 - Reason ($reason)");
            else
                iceChatMessages.MessageSettings[23] = oldMessage.MessageSettings[23];

            if (oldMessage.MessageSettings[24] == null || oldMessage.MessageSettings[24].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[24] = NewMessageFormat("Self Channel Kick", "&#x3;04* You were kicked from $channel by $kicker (&#x3;03$reason)");
            else
                iceChatMessages.MessageSettings[24] = oldMessage.MessageSettings[24];

            if (oldMessage.MessageSettings[25] == null || oldMessage.MessageSettings[25].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[25] = NewMessageFormat("Private Message", "&#x3;00<$nick> $message");
            else
                iceChatMessages.MessageSettings[25] = oldMessage.MessageSettings[25];

            if (oldMessage.MessageSettings[26] == null || oldMessage.MessageSettings[26].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[26] = NewMessageFormat("Self Private Message", "&#x3;04<$nick>&#x3;04 $message");
            else
                iceChatMessages.MessageSettings[26] = oldMessage.MessageSettings[26];

            if (oldMessage.MessageSettings[27] == null || oldMessage.MessageSettings[27].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[27] = NewMessageFormat("Private Action", "&#x3;13* $nick $message");
            else
                iceChatMessages.MessageSettings[27] = oldMessage.MessageSettings[27];

            if (oldMessage.MessageSettings[28] == null || oldMessage.MessageSettings[28].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[28] = NewMessageFormat("Self Private Action", "&#x3;13* $nick $message");
            else
                iceChatMessages.MessageSettings[28] = oldMessage.MessageSettings[28];

            if (oldMessage.MessageSettings[35] == null || oldMessage.MessageSettings[35].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[35] = NewMessageFormat("Channel Topic Change", "&#x3;03* $nick changes topic to: $topic");
            else
                iceChatMessages.MessageSettings[35] = oldMessage.MessageSettings[35];

            if (oldMessage.MessageSettings[36] == null || oldMessage.MessageSettings[36].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[36] = NewMessageFormat("Channel Topic Text", "&#x3;00Topic: $topic");
            else
                iceChatMessages.MessageSettings[36] = oldMessage.MessageSettings[36];

            if (oldMessage.MessageSettings[37] == null || oldMessage.MessageSettings[37].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[37] = NewMessageFormat("Server MOTD", "&#x3;00$message");
            else
                iceChatMessages.MessageSettings[37] = oldMessage.MessageSettings[37];

            if (oldMessage.MessageSettings[38] == null || oldMessage.MessageSettings[38].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[38] = NewMessageFormat("Channel Notice", "&#x3;04-$nick:$status$channel- $message");
            else
                iceChatMessages.MessageSettings[38] = oldMessage.MessageSettings[38];

            if (oldMessage.MessageSettings[39] == null || oldMessage.MessageSettings[39].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[39] = NewMessageFormat("Channel Other", "&#x3;00$message");
            else
                iceChatMessages.MessageSettings[39] = oldMessage.MessageSettings[39];

            if (oldMessage.MessageSettings[40] == null || oldMessage.MessageSettings[40].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[40] = NewMessageFormat("User Echo", "&#x3;07$message");
            else
                iceChatMessages.MessageSettings[40] = oldMessage.MessageSettings[40];

            if (oldMessage.MessageSettings[41] == null || oldMessage.MessageSettings[41].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[41] = NewMessageFormat("Server Error", "&#x3;04ERROR: $message");
            else
                iceChatMessages.MessageSettings[41] = oldMessage.MessageSettings[41];

            if (oldMessage.MessageSettings[42] == null || oldMessage.MessageSettings[42].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[42] = NewMessageFormat("User Whois", "&#x3;12->> $nick $data");
            else
                iceChatMessages.MessageSettings[42] = oldMessage.MessageSettings[42];

            if (oldMessage.MessageSettings[43] == null || oldMessage.MessageSettings[43].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[43] = NewMessageFormat("User Error", "&#x3;04ERROR: $message");
            else
                iceChatMessages.MessageSettings[43] = oldMessage.MessageSettings[43];

            if (oldMessage.MessageSettings[44] == null || oldMessage.MessageSettings[44].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[44] = NewMessageFormat("DCC Chat Connect", "&#x3;00* DCC Chat Connection Established with $nick");
            else
                iceChatMessages.MessageSettings[44] = oldMessage.MessageSettings[44];
            
            if (oldMessage.MessageSettings[45] == null || oldMessage.MessageSettings[45].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[45] = NewMessageFormat("DCC Chat Disconnect", "&#x3;04* DCC Chat Disconnected from $nick");
            else
                iceChatMessages.MessageSettings[45] = oldMessage.MessageSettings[45];

            if (oldMessage.MessageSettings[48] == null || oldMessage.MessageSettings[13].FormattedMessage.Length == 0)
                iceChatMessages.MessageSettings[48] = NewMessageFormat("Self Notice", "&#x3;04--> $nick - $message");
            else
                iceChatMessages.MessageSettings[48] = oldMessage.MessageSettings[48];

            //still do customize these messages
            iceChatMessages.MessageSettings[4] = NewMessageFormat("Ctcp Reply", "&#x3;12[$nick $ctcp Reply] : $reply");
            iceChatMessages.MessageSettings[5] = NewMessageFormat("Ctcp Send", "&#x3;10--> [$nick] $ctcp");
            iceChatMessages.MessageSettings[6] = NewMessageFormat("Ctcp Request", "&#x3;07[$nick] $ctcp");
            
            iceChatMessages.MessageSettings[29] = NewMessageFormat("DCC Chat Action", "&#x3;13* $nick $message");
            iceChatMessages.MessageSettings[30] = NewMessageFormat("Self DCC Chat Action", "&#x3;13* $nick $message");
            iceChatMessages.MessageSettings[31] = NewMessageFormat("DCC Chat Message", "&#x3;00<$nick> $message");
            iceChatMessages.MessageSettings[32] = NewMessageFormat("Self DCC Chat Message", "&#x3;04<$nick> $message");
            
            iceChatMessages.MessageSettings[33] = NewMessageFormat("DCC Chat Request", "&#x3;04* $nick ($host) is requesting a DCC Chat");
            iceChatMessages.MessageSettings[34] = NewMessageFormat("DCC File Send", "&#x3;04* $nick ($host) is trying to send you a file ($file) [$filesize bytes]");
            
            iceChatMessages.MessageSettings[46] = NewMessageFormat("DCC Chat Outgoing", "&#x3;00* DCC Chat Requested with $nick");
            iceChatMessages.MessageSettings[47] = NewMessageFormat("DCC Chat Timeout", "&#x3;00* DCC Chat with $nick timed out");

            SaveMessageFormat();

        }

        private void LoadDefaultFontSettings()
        {
            IceChatFontSetting oldFonts = new IceChatFontSetting
            {
                FontSettings = new FontSettingItem[9]
            };

            if (iceChatFonts.FontSettings != null)
            {
                iceChatFonts.FontSettings.CopyTo(oldFonts.FontSettings, 0);
                iceChatFonts.FontSettings = new FontSettingItem[9];
                oldFonts.FontSettings.CopyTo(iceChatFonts.FontSettings, 0);
            }
            else
                iceChatFonts.FontSettings = new FontSettingItem[9];

            if (oldFonts.FontSettings[0] == null || iceChatFonts.FontSettings[0].FontName.Length == 0)
                iceChatFonts.FontSettings[0] = NewFontSetting("Console", "Verdana", 10);
            else
                iceChatFonts.FontSettings[0] = oldFonts.FontSettings[0];

            if (oldFonts.FontSettings[1] == null || iceChatFonts.FontSettings[1].FontName.Length == 0)
                iceChatFonts.FontSettings[1] = NewFontSetting("Channel", "Verdana", 10);
            else
                iceChatFonts.FontSettings[1] = oldFonts.FontSettings[1];

            if (oldFonts.FontSettings[2] == null || iceChatFonts.FontSettings[2].FontName.Length == 0)
                iceChatFonts.FontSettings[2] = NewFontSetting("Query", "Verdana", 10);
            else
                iceChatFonts.FontSettings[2] = oldFonts.FontSettings[2];

            if (oldFonts.FontSettings[3] == null || iceChatFonts.FontSettings[3].FontName.Length == 0)
                iceChatFonts.FontSettings[3] = NewFontSetting("Nicklist", "Verdana", 10);
            else
                iceChatFonts.FontSettings[3] = oldFonts.FontSettings[3];

            if (oldFonts.FontSettings[4] == null || iceChatFonts.FontSettings[4].FontName.Length == 0)
                iceChatFonts.FontSettings[4] = NewFontSetting("Serverlist", "Verdana", 10);
            else
                iceChatFonts.FontSettings[4] = oldFonts.FontSettings[4];

            if (oldFonts.FontSettings[5] == null || iceChatFonts.FontSettings[5].FontName.Length == 0)
                iceChatFonts.FontSettings[5] = NewFontSetting("InputBox", "Verdana", 10);
            else
                iceChatFonts.FontSettings[5] = oldFonts.FontSettings[5];

            if (oldFonts.FontSettings[6] == null || iceChatFonts.FontSettings[6].FontName.Length == 0)
                iceChatFonts.FontSettings[6] = NewFontSetting("DockTabs", "Verdana", 10);
            else
                iceChatFonts.FontSettings[6] = oldFonts.FontSettings[6];

            if (oldFonts.FontSettings[7] == null || iceChatFonts.FontSettings[7].FontName.Length == 0)
                iceChatFonts.FontSettings[7] = NewFontSetting("MenuBar", "Verdana", 10);
            else
                iceChatFonts.FontSettings[7] = oldFonts.FontSettings[7];

            if (oldFonts.FontSettings[8] == null || iceChatFonts.FontSettings[8].FontName.Length == 0)
                iceChatFonts.FontSettings[8] = NewFontSetting("ChannelBar", "Verdana", 10);
            else
                iceChatFonts.FontSettings[8] = oldFonts.FontSettings[8];

            
            oldFonts = null;

            SaveFonts();
        }

        private ServerMessageFormatItem NewMessageFormat(string messageName, string message)
        {
            ServerMessageFormatItem m = new ServerMessageFormatItem
            {
                MessageName = messageName,
                FormattedMessage = message
            };
            return m;
        }

        private FontSettingItem NewFontSetting(string windowType, string fontName, int fontSize)
        {
            FontSettingItem f = new FontSettingItem
            {
                WindowType = windowType,
                FontName = fontName,
                FontSize = fontSize
            };
            return f;
        }


        private void LoadMessageFormat()
        {
            if (File.Exists(messagesFile))
            {
                XmlTextReader textReader = null;
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatMessageFormat));
                    textReader = new XmlTextReader(messagesFile);
                    iceChatMessages = (IceChatMessageFormat)deserializer.Deserialize(textReader);
                    textReader.Close();
                    if (iceChatMessages.MessageSettings.Length != 49)
                        LoadDefaultMessageSettings();
                }
                catch (Exception)
                {
                    textReader.Close();
                    errorMessages.Add("There was a problem loading IceChatMessages.xml. Default color settings loaded");
                    iceChatMessages = new IceChatMessageFormat();
                    LoadDefaultMessageSettings();
                }
            }
            else
            {
                iceChatMessages = new IceChatMessageFormat();
                LoadDefaultMessageSettings();
            }
        }

        private void SaveMessageFormat()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatMessageFormat));
            TextWriter textWriter = new StreamWriter(messagesFile);
            serializer.Serialize(textWriter, iceChatMessages);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadChannelSettings()
        {
            if (File.Exists(channelSettingsFile))
            {
                XmlTextReader textReader = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(ChannelSettings));
                try
                {
                    textReader = new XmlTextReader(channelSettingsFile);
                    channelSettings = (ChannelSettings)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    if (textReader != null)
                        textReader.Close();



                    errorMessages.Add("There was a problem loading ChannelSettings.xml. Default channel settings loaded");
                    channelSettings = new ChannelSettings();
                }
            }
            else
            {
                channelSettings = new ChannelSettings();
            }
        }

        internal void SaveChannelSettings()
        {
            // check if file is being used
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ChannelSettings));
                TextWriter textWriter = new StreamWriter(channelSettingsFile);
                serializer.Serialize(textWriter, channelSettings);
                textWriter.Close();
                textWriter.Dispose();
            } 
            catch(Exception)
            {
                // ignore it
            }
        }

        private void LoadAliases()
        {

            if (File.Exists(aliasesFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatAliases));
                TextReader textReader = null;

                try
                {
                    textReader = new StreamReader(aliasesFile);
                    iceChatAliases = (IceChatAliases)deserializer.Deserialize(textReader);
                    
                    textReader.Close();
                }
                catch (Exception)
                {
                    if (textReader != null)
                        textReader.Close();

                    errorMessages.Add("There was a problem loading IceChatAliases.xml. Default aliases loaded");
                    iceChatAliases = new IceChatAliases();
                    SaveAliases();
                }
            }
            else
            {
                iceChatAliases = new IceChatAliases();
                SaveAliases();
            }
        }

        private void SaveAliases()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatAliases));
            TextWriter textWriter = new StreamWriter(aliasesFile);
            serializer.Serialize(textWriter, iceChatAliases);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadPluginFiles()
        {

            if (File.Exists(pluginsFile))
            {
                XmlTextReader textReader = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatPluginFile));
                try
                {
                    textReader = new XmlTextReader(pluginsFile);
                    iceChatPlugins = (IceChatPluginFile)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    if (textReader != null)
                        textReader.Close();
                    
                    
                    errorMessages.Add("There was a problem loading IceChatPlugins.xml. No plugins loaded");
                    iceChatPlugins = new IceChatPluginFile();
                    SavePluginFiles();
                }
            }
            else
            {
                iceChatPlugins = new IceChatPluginFile();
                SavePluginFiles();
            }
        }

        private void SavePluginFiles()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatPluginFile));
            TextWriter textWriter = new StreamWriter(pluginsFile);
            serializer.Serialize(textWriter, iceChatPlugins);
            textWriter.Close();
            textWriter.Dispose();
        }


        private void LoadEmoticons()
        {
            if (File.Exists(emoticonsFile))
            {
                XmlTextReader textReader = null;
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatEmoticon));
                    textReader = new XmlTextReader(emoticonsFile);
                    iceChatEmoticons = (IceChatEmoticon)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    textReader.Close();
                    errorMessages.Add("There was a problem loading IceChatEmoticons.xml. No emoticons loaded");
                    iceChatEmoticons = new IceChatEmoticon();
                    SaveEmoticons();
                }
            }
            else
            {
                iceChatEmoticons = new IceChatEmoticon();
                SaveEmoticons();
            }
        }

        private void SaveEmoticons()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatEmoticon));
            //check if emoticons Folder Exists
            if (!System.IO.File.Exists(EmoticonsFolder))
                System.IO.Directory.CreateDirectory(EmoticonsFolder);

            TextWriter textWriter = new StreamWriter(emoticonsFile);
            serializer.Serialize(textWriter, iceChatEmoticons);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadOptions()
        {
            XmlTextReader textReader = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(IceChatOptions));

            if (File.Exists(optionsFile))
            {

                try
                {
                    textReader = new XmlTextReader(optionsFile);
                    iceChatOptions = (IceChatOptions)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {

                    if (textReader != null)
                        textReader.Close();

                    try
                    {
                        // check if there is a backup file
                        string backupFile = CurrentFolder + Path.DirectorySeparatorChar + "Backups" + Path.DirectorySeparatorChar + "IceChatOptions.xml";
                        if (File.Exists(backupFile))
                        {
                            textReader = new XmlTextReader(backupFile);
                            iceChatOptions = (IceChatOptions)deserializer.Deserialize(textReader);
                            textReader.Close();

                            loadErrors.Add("There was a problem with IceChatOptions.xml, restored from backup");

                            File.Copy(backupFile, optionsFile, true);
                        }
                        else
                        {
                            errorMessages.Add("There was a problem loading IceChatOptions.xml. Default options loaded");

                            iceChatOptions = new IceChatOptions();
                            SaveOptions();
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        errorMessages.Add("There was a problem loading IceChatOptions.xml, invalid backup. Default options loaded");

                        iceChatOptions = new IceChatOptions();
                        SaveOptions();
                    }
                }
            }
            else
            {
                //create default settings
                iceChatOptions = new IceChatOptions();
                SaveOptions();
            }
        }

        private void SaveOptions()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatOptions));
            TextWriter textWriter = new StreamWriter(optionsFile);
            serializer.Serialize(textWriter, iceChatOptions);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadPopups()
        {
            if (File.Exists(popupsFile))
            {
                XmlTextReader textReader = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatPopupMenus));

                try
                {                
                    textReader = new XmlTextReader(popupsFile);
                    iceChatPopups = (IceChatPopupMenus)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    if (textReader != null)
                        textReader.Close();

                    try
                    {
                        string backupFile = CurrentFolder + Path.DirectorySeparatorChar + "Backups" + Path.DirectorySeparatorChar + "IceChatPopups.xml";
                        if (File.Exists(backupFile))
                        {
                            textReader = new XmlTextReader(backupFile);
                            iceChatPopups = (IceChatPopupMenus)deserializer.Deserialize(textReader);
                            textReader.Close();

                            loadErrors.Add("There was a problem with IceChatPopups.xml, restored from backup");

                            File.Copy(backupFile, popupsFile, true);
                        }
                        else
                        {
                            errorMessages.Add("There was a problem loading IceChatPopups.xml. No popup menus loaded");
                            iceChatPopups = new IceChatPopupMenus();
                        }

                    }
                    catch (InvalidOperationException)
                    {
                        errorMessages.Add("There was a problem loading IceChatPopups.xml. No backup to restore");
                        iceChatPopups = new IceChatPopupMenus();

                    }
                }
            }
            else
                iceChatPopups = new IceChatPopupMenus();

        }

        private void SavePopups()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatPopupMenus));
            TextWriter textWriter = new StreamWriter(popupsFile);
            serializer.Serialize(textWriter, iceChatPopups);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadFonts()
        {
            if (File.Exists(fontsFile))
            {
                XmlTextReader textReader = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatFontSetting));

                try
                {
                
                    textReader = new XmlTextReader(fontsFile);
                    iceChatFonts = (IceChatFontSetting)deserializer.Deserialize(textReader);
                    textReader.Close();

                    if (iceChatFonts.FontSettings.Length < 9)
                        LoadDefaultFontSettings();
                }
                catch (Exception)
                {
                    if (textReader != null)
                        textReader.Close();



                    errorMessages.Add("There was a problem loading IceChatFonts.xml. Default font settings loaded");
                    // if we have a backup, use this instead


                    iceChatFonts = new IceChatFontSetting();
                    LoadDefaultFontSettings();
                }
            }
            else
            {
                iceChatFonts = new IceChatFontSetting();
                LoadDefaultFontSettings();
            }
        }

        private void SaveFonts()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatFontSetting));
            TextWriter textWriter = new StreamWriter(fontsFile);
            serializer.Serialize(textWriter, iceChatFonts);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadColorPalette()
        {
            if (File.Exists(colorPaletteFile))
            {
                XmlTextReader textReader = null;
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatColorPalette));
                    textReader = new XmlTextReader(colorPaletteFile);
                    colorPalette = (IceChatColorPalette)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    textReader.Close();
                    errorMessages.Add("There was a problem loading ColorPalette.xml. Default color palette loaded");
                    colorPalette = new IceChatColorPalette();
                    // colors 0 - 15
                    colorPalette.listColors.Add("#FFFFFF");
                    colorPalette.listColors.Add("#000000");
                    colorPalette.listColors.Add("#00007F");
                    colorPalette.listColors.Add("#009300");
                    colorPalette.listColors.Add("#FF0000");
                    colorPalette.listColors.Add("#7F0000");
                    colorPalette.listColors.Add("#9C009C");
                    colorPalette.listColors.Add("#FC7F00");
                    colorPalette.listColors.Add("#FFFF00");
                    colorPalette.listColors.Add("#00FC00");
                    colorPalette.listColors.Add("#009393");
                    colorPalette.listColors.Add("#00FFFF");
                    colorPalette.listColors.Add("#0000FC");
                    colorPalette.listColors.Add("#FF00FF");
                    colorPalette.listColors.Add("#7F7F7F");
                    colorPalette.listColors.Add("#D2D2D2");

                    // colors 16 - 31
                    colorPalette.listColors.Add("#CCFFCC");
                    colorPalette.listColors.Add("#0066FF");
                    colorPalette.listColors.Add("#FAEBD7");
                    colorPalette.listColors.Add("#FFD700");
                    colorPalette.listColors.Add("#E6E6E6");
                    colorPalette.listColors.Add("#4682B4");
                    colorPalette.listColors.Add("#993333");
                    colorPalette.listColors.Add("#FF99FF");
                    colorPalette.listColors.Add("#DDA0DD");
                    colorPalette.listColors.Add("#8B4513");
                    colorPalette.listColors.Add("#CC0000");
                    colorPalette.listColors.Add("#FFFF99");
                    colorPalette.listColors.Add("#339900");
                    colorPalette.listColors.Add("#FF9900");
                    colorPalette.listColors.Add("#FFDAB9");
                    colorPalette.listColors.Add("#2F4F4F");

                    // colors 32 - 47
                    colorPalette.listColors.Add("#ECE9D8");
                    colorPalette.listColors.Add("#5FDAEE");
                    colorPalette.listColors.Add("#E2FF00");
                    colorPalette.listColors.Add("#00009E");
                    colorPalette.listColors.Add("#FFFFCC");
                    colorPalette.listColors.Add("#FFFF99");
                    colorPalette.listColors.Add("#FFFF66");
                    colorPalette.listColors.Add("#FFCC33");
                    colorPalette.listColors.Add("#FF9933");
                    colorPalette.listColors.Add("#FF6633");
                    colorPalette.listColors.Add("#c6ffc6");
                    colorPalette.listColors.Add("#84ff84");
                    colorPalette.listColors.Add("#00ff00");
                    colorPalette.listColors.Add("#00c700");
                    colorPalette.listColors.Add("#008600");
                    colorPalette.listColors.Add("#004100");

                    //colors 48 - 59
                    colorPalette.listColors.Add("#C6FFFF");
                    colorPalette.listColors.Add("#84FFFF");
                    colorPalette.listColors.Add("#00FFFF");
                    colorPalette.listColors.Add("#6699FF");
                    colorPalette.listColors.Add("#6666FF");
                    colorPalette.listColors.Add("#3300FF");
                    colorPalette.listColors.Add("#FFCC99");
                    colorPalette.listColors.Add("#FF9966");
                    colorPalette.listColors.Add("#ff6633");
                    colorPalette.listColors.Add("#FF0033");
                    colorPalette.listColors.Add("#CC0000");
                    colorPalette.listColors.Add("#AA0000");

                    //colors 60 - 71
                    colorPalette.listColors.Add("#ffc7ff");
                    colorPalette.listColors.Add("#ff86ff");
                    colorPalette.listColors.Add("#ff00ff");
                    colorPalette.listColors.Add("#FF00CC");
                    colorPalette.listColors.Add("#CC0099");
                    colorPalette.listColors.Add("#660099");
                    colorPalette.listColors.Add("#EEEEEE");
                    colorPalette.listColors.Add("#CCCCCC");
                    colorPalette.listColors.Add("#AAAAAA");
                    colorPalette.listColors.Add("#888888");
                    colorPalette.listColors.Add("#666666");
                    colorPalette.listColors.Add("#444444");

                    // colors 72 - 87
                    colorPalette.listColors.Add("#5959ff");
                    colorPalette.listColors.Add("#c459ff");
                    colorPalette.listColors.Add("#ff66ff");
                    colorPalette.listColors.Add("#ff59bc");

                    colorPalette.listColors.Add("#ff9c9c");
                    colorPalette.listColors.Add("#ffd39c");
                    colorPalette.listColors.Add("#ffff9c");
                    colorPalette.listColors.Add("#e2ff9c");
                    colorPalette.listColors.Add("#9cff9c");
                    colorPalette.listColors.Add("#9cffdb");
                    colorPalette.listColors.Add("#9cffff");
                    colorPalette.listColors.Add("#9cd3ff");
                    colorPalette.listColors.Add("#9c9cff");
                    colorPalette.listColors.Add("#dc9cff");
                    colorPalette.listColors.Add("#ff9cff");
                    colorPalette.listColors.Add("#ff94d3");

                    // colors 88 - 98
                    colorPalette.listColors.Add("#000000");
                    colorPalette.listColors.Add("#131313");
                    colorPalette.listColors.Add("#282828");
                    colorPalette.listColors.Add("#363636");
                    colorPalette.listColors.Add("#4d4d4d");
                    colorPalette.listColors.Add("#656565");
                    colorPalette.listColors.Add("#818181");
                    colorPalette.listColors.Add("#9f9f9f");
                    colorPalette.listColors.Add("#bcbcbc");
                    colorPalette.listColors.Add("#e2e2e2");
                    colorPalette.listColors.Add("#ffffff");

                }
            }
            else
            {
                //create new default color palette
                colorPalette = new IceChatColorPalette();
                // colors 0 - 15
                colorPalette.listColors.Add("#FFFFFF");
                colorPalette.listColors.Add("#000000");
                colorPalette.listColors.Add("#00007F");
                colorPalette.listColors.Add("#009300");
                colorPalette.listColors.Add("#FF0000");
                colorPalette.listColors.Add("#7F0000");
                colorPalette.listColors.Add("#9C009C");
                colorPalette.listColors.Add("#FC7F00");
                colorPalette.listColors.Add("#FFFF00");
                colorPalette.listColors.Add("#00FC00");
                colorPalette.listColors.Add("#009393");
                colorPalette.listColors.Add("#00FFFF");
                colorPalette.listColors.Add("#0000FC");
                colorPalette.listColors.Add("#FF00FF");
                colorPalette.listColors.Add("#7F7F7F");
                colorPalette.listColors.Add("#D2D2D2");
                
                
                // IceChat Colors 16 - 71
                // colors 16 - 31
                colorPalette.listColors.Add("#CCFFCC");
                colorPalette.listColors.Add("#0066FF");
                colorPalette.listColors.Add("#FAEBD7");
                colorPalette.listColors.Add("#FFD700");
                colorPalette.listColors.Add("#E6E6E6");
                colorPalette.listColors.Add("#4682B4");
                colorPalette.listColors.Add("#993333");
                colorPalette.listColors.Add("#FF99FF");
                colorPalette.listColors.Add("#DDA0DD");
                colorPalette.listColors.Add("#8B4513");
                colorPalette.listColors.Add("#CC0000");
                colorPalette.listColors.Add("#FFFF99");
                colorPalette.listColors.Add("#339900");
                colorPalette.listColors.Add("#FF9900");
                colorPalette.listColors.Add("#FFDAB9");
                colorPalette.listColors.Add("#2F4F4F");

                // colors 32 - 47
                colorPalette.listColors.Add("#ECE9D8");
                colorPalette.listColors.Add("#5FDAEE");
                colorPalette.listColors.Add("#E2FF00");
                colorPalette.listColors.Add("#00009E");
                colorPalette.listColors.Add("#FFFFCC");
                colorPalette.listColors.Add("#FFFF99");
                colorPalette.listColors.Add("#FFFF66");
                colorPalette.listColors.Add("#FFCC33");
                colorPalette.listColors.Add("#FF9933");
                colorPalette.listColors.Add("#FF6633");
                colorPalette.listColors.Add("#c6ffc6");
                colorPalette.listColors.Add("#84ff84");
                colorPalette.listColors.Add("#00ff00");
                colorPalette.listColors.Add("#00c700");
                colorPalette.listColors.Add("#008600");
                colorPalette.listColors.Add("#004100");

                //colors 48 - 59
                colorPalette.listColors.Add("#C6FFFF");
                colorPalette.listColors.Add("#84FFFF");
                colorPalette.listColors.Add("#00FFFF");
                colorPalette.listColors.Add("#6699FF");
                colorPalette.listColors.Add("#6666FF");
                colorPalette.listColors.Add("#3300FF");
                colorPalette.listColors.Add("#FFCC99");
                colorPalette.listColors.Add("#FF9966");
                colorPalette.listColors.Add("#ff6633");
                colorPalette.listColors.Add("#FF0033");
                colorPalette.listColors.Add("#CC0000");
                colorPalette.listColors.Add("#AA0000");
                
                //colors 60 - 71
                colorPalette.listColors.Add("#ffc7ff");
                colorPalette.listColors.Add("#ff86ff");
                colorPalette.listColors.Add("#ff00ff");
                colorPalette.listColors.Add("#FF00CC");
                colorPalette.listColors.Add("#CC0099");
                colorPalette.listColors.Add("#660099");
                colorPalette.listColors.Add("#EEEEEE");
                colorPalette.listColors.Add("#CCCCCC");
                colorPalette.listColors.Add("#AAAAAA");
                colorPalette.listColors.Add("#888888");
                colorPalette.listColors.Add("#666666");
                colorPalette.listColors.Add("#444444");

                // end of IceChat Colors 16-71
                
                // starf of new extended color palette
                /*
                colorPalette.listColors.Add("#470000");
                colorPalette.listColors.Add("#472100");
                colorPalette.listColors.Add("#474700");
                colorPalette.listColors.Add("#324700");
                colorPalette.listColors.Add("#004700");
                colorPalette.listColors.Add("#00472c");
                colorPalette.listColors.Add("#004747");
                colorPalette.listColors.Add("#002747");
                colorPalette.listColors.Add("#000047");
                colorPalette.listColors.Add("#2e0047");
                colorPalette.listColors.Add("#470047");
                colorPalette.listColors.Add("#47002a");
                colorPalette.listColors.Add("#740000");
                colorPalette.listColors.Add("#743a00");
                colorPalette.listColors.Add("#747400");
                colorPalette.listColors.Add("#517400");
                colorPalette.listColors.Add("#007400");
                colorPalette.listColors.Add("#007449");
                colorPalette.listColors.Add("#007474");
                colorPalette.listColors.Add("#004074");
                colorPalette.listColors.Add("#000074");
                colorPalette.listColors.Add("#4b0074");
                colorPalette.listColors.Add("#740074");
                colorPalette.listColors.Add("#740045");

                colorPalette.listColors.Add("#b50000");
                colorPalette.listColors.Add("#b56300");
                colorPalette.listColors.Add("#b5b500");
                colorPalette.listColors.Add("#7db500");
                colorPalette.listColors.Add("#00b500");
                colorPalette.listColors.Add("#00b571");
                colorPalette.listColors.Add("#00b5b5");
                colorPalette.listColors.Add("#0063b5");
                colorPalette.listColors.Add("#0000b5");
                colorPalette.listColors.Add("#7500b5");
                colorPalette.listColors.Add("#b500b5");
                colorPalette.listColors.Add("#b5006b");

                colorPalette.listColors.Add("#ff0000");
                colorPalette.listColors.Add("#ff8c00");
                colorPalette.listColors.Add("#ffff00");
                colorPalette.listColors.Add("#b2ff00");
                colorPalette.listColors.Add("#00ff00");
                colorPalette.listColors.Add("#00ffa0");
                colorPalette.listColors.Add("#00ffff");
                colorPalette.listColors.Add("#008cff");
                colorPalette.listColors.Add("#0000ff");
                colorPalette.listColors.Add("#a500ff");
                colorPalette.listColors.Add("#ff00ff");
                colorPalette.listColors.Add("#ff0098");

                colorPalette.listColors.Add("#ff5959");
                colorPalette.listColors.Add("#ffb459");
                colorPalette.listColors.Add("#ffff71");
                colorPalette.listColors.Add("#cfff60");
                colorPalette.listColors.Add("#6fff6f");
                colorPalette.listColors.Add("#65ffc9");
                colorPalette.listColors.Add("#6dffff");
                colorPalette.listColors.Add("#59b4ff");
                // end of new extended color palette
                */

                // colors 72 - 87
                colorPalette.listColors.Add("#5959ff");
                colorPalette.listColors.Add("#c459ff");
                colorPalette.listColors.Add("#ff66ff");
                colorPalette.listColors.Add("#ff59bc");
                colorPalette.listColors.Add("#ff9c9c");
                colorPalette.listColors.Add("#ffd39c");
                colorPalette.listColors.Add("#ffff9c");
                colorPalette.listColors.Add("#e2ff9c");
                colorPalette.listColors.Add("#9cff9c");
                colorPalette.listColors.Add("#9cffdb");
                colorPalette.listColors.Add("#9cffff");
                colorPalette.listColors.Add("#9cd3ff");
                colorPalette.listColors.Add("#9c9cff");
                colorPalette.listColors.Add("#dc9cff");
                colorPalette.listColors.Add("#ff9cff");
                colorPalette.listColors.Add("#ff94d3");

                // colors 88 - 98
                colorPalette.listColors.Add("#000000");
                colorPalette.listColors.Add("#131313");
                colorPalette.listColors.Add("#282828");
                colorPalette.listColors.Add("#363636");
                colorPalette.listColors.Add("#4d4d4d");
                colorPalette.listColors.Add("#656565");
                colorPalette.listColors.Add("#818181");
                colorPalette.listColors.Add("#9f9f9f");
                colorPalette.listColors.Add("#bcbcbc");
                colorPalette.listColors.Add("#e2e2e2");
                colorPalette.listColors.Add("#ffffff");


                SaveColorPalette();
            }
        }

        public void SaveColorPalette()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatColorPalette));
            TextWriter textWriter = new StreamWriter(colorPaletteFile);
            serializer.Serialize(textWriter, colorPalette);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void LoadColors()
        {
            if (File.Exists(colorsFile))
            {                
                XmlTextReader textReader = null;
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatColors));
                    textReader = new XmlTextReader(colorsFile);
                    iceChatColors = (IceChatColors)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    textReader.Close();
                    errorMessages.Add("There was a problem loading IceChatColors.xml. Default colors loaded");
                    // if we have a backup, use this instead
                    
                    
                    iceChatColors = new IceChatColors();
                }
            }
            else
                iceChatColors = new IceChatColors();
        }

        private void SaveColors()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatColors));
            TextWriter textWriter = new StreamWriter(colorsFile);
            serializer.Serialize(textWriter, iceChatColors);
            textWriter.Close();
            textWriter.Dispose();
        }
    
        public void LoadSounds()
        {
            if (File.Exists(soundsFile))
            {
                XmlTextReader textReader = null;
                try
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatSounds));
                    textReader = new XmlTextReader(soundsFile);
                    iceChatSounds = (IceChatSounds)deserializer.Deserialize(textReader);
                    textReader.Close();
                }
                catch (Exception)
                {
                    textReader.Close();
                    errorMessages.Add("There was a problem loading IceChatSounds.xml. Default sounds loaded");
                    // if we have a backup, use this instead


                    iceChatSounds = new IceChatSounds();
                    iceChatSounds.AddDefaultSounds();
                }
            }
            else
            {
                iceChatSounds = new IceChatSounds();
                iceChatSounds.AddDefaultSounds();
            }
        }

        private void SaveSounds()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatSounds));
            TextWriter textWriter = new StreamWriter(soundsFile);
            serializer.Serialize(textWriter, iceChatSounds);
            textWriter.Close();
            textWriter.Dispose();
        }
    }
}

/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2020 Paul Vanderzee <snerf@icechat.net>
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

namespace IceChat
{
    public delegate void OutGoingCommandDelegate(IRCConnection connection, string data);
    public delegate void RawServerIncomingDataDelegate(IRCConnection connection, string data);
    public delegate void RawServerOutgoingDataDelegate(IRCConnection connection, string data);

    public delegate string RawServerIncomingDataOverRideDelegate(IRCConnection connection, string data);

    public delegate void ChannelMessageDelegate(IRCConnection connection, string channel, string nick, string host, string message, string timeStamp);
    public delegate void ChannelActionDelegate(IRCConnection connection, string channel, string nick, string host, string message, string timeStamp);
    public delegate void QueryMessageDelegate(IRCConnection connection, string nick, string host, string message, string timeStamp);
    public delegate void QueryActionDelegate(IRCConnection connection, string nick, string host, string message, string timeStamp);
    public delegate void GenericChannelMessageDelegate(IRCConnection connection, string channel, string message, string timeStamp);
    public delegate void ChannelNoticeDelegate(IRCConnection connection, string nick, string host, char status, string channel, string message, string timeStamp);

    public delegate void JoinChannelDelegate(IRCConnection connection, string channel, string nick, string host, string account, bool refresh, string timeStamp);
    public delegate void PartChannelDelegate(IRCConnection connection, string channel, string nick, string host, string reason, string timeStamp);
    public delegate void QuitServerDelegate(IRCConnection connection, string nick, string host, string reason, string timeStamp);

    public delegate void AddNickNameDelegate(IRCConnection connection, string channel, string nick);
    public delegate void RemoveNickNameDelegate(IRCConnection connection, string channel, string nick);
    public delegate void ClearNickListDelegate(IRCConnection connection, string channel);
    public delegate void UserHostReplyDelegate(IRCConnection connection, string fullhost);

    public delegate void ChannelKickDelegate(IRCConnection connection, string channel, string nick, string reason, string kickUser, string timeStamp);
    public delegate void ChannelKickSelfDelegate(IRCConnection connection, string channel, string reason, string kickUser, string timeStamp);
    public delegate void ChangeNickDelegate(IRCConnection connection, string oldnick, string newnick, string host, string timeStamp);
    public delegate void UserNoticeDelegate(IRCConnection connection, string nick, string message, string timeStamp);
    public delegate void ServerNoticeDelegate(IRCConnection connection, string message, string timeStamp);

    public delegate void JoinChannelMyselfDelegate(IRCConnection connection, string channel, string host, string account, string timeStamp);
    public delegate void PartChannelMyselfDelegate(IRCConnection connection, string channel, string timeStamp);

    public delegate void ChannelTopicDelegate(IRCConnection connection, string channel, string nick, string host, string topic, string timeStamp);

    public delegate void UserModeChangeDelegate(IRCConnection connection, string nick, string mode, string timeStamp);
    public delegate void ChannelModeChangeDelegate(IRCConnection connection, string modeSetter, string modeSetterHost, string channel, string fullmode, string timeStamp);

    public delegate void ServerMessageDelegate(IRCConnection connection, string message, string timeStamp);
    public delegate void ServerMOTDDelegate(IRCConnection connection, string message, string timeStamp);
    public delegate void ServerErrorDelegate(IRCConnection connection, string message, bool current);
    public delegate void WhoisDataDelegate(IRCConnection connection, string nick, string data, string timeStamp);
    public delegate void CtcpMessageDelegate(IRCConnection connection, string nick, string ctcp, string message, string timeStamp);
    public delegate void CtcpReplyDelegate(IRCConnection connection, string nick, string ctcp, string message, string timeStamp);

    public delegate void ChannelListStartDelegate(IRCConnection connection);
    public delegate void ChannelListEndDelegate(IRCConnection connection);
    public delegate void ChannelListDelegate(IRCConnection connection, string channel, string users, string topic);
    public delegate void ChannelInviteDelegate(IRCConnection connection, string channel, string nick, string host, string timeStamp);

    public delegate void DCCChatDelegate(IRCConnection connection, string nick, string host, string port, string ip);
    public delegate void DCCFileDelegate(IRCConnection connection, string nick, string host, string port, string ip, string file, uint fileSize, uint filePos, bool resume);
    public delegate void DCCPassiveDelegate(IRCConnection connection, string nick, string host, string ip, string file, uint fileSize, uint filePos, bool resume, string id);

    public delegate void ServerFullyConnectedDelegate(IRCConnection connection);
    public delegate void AutoAwayDelegate(IRCConnection connection);

    //for the Server Tree
    public delegate void NewServerConnectionDelegate(ServerSetting serverSetting);

    //for the Buddy List
    public delegate void BuddyListDelegate(IRCConnection connection, string[] buddies, string timeStamp);
    public delegate void BuddyListClearDelegate(IRCConnection connection);
    public delegate void BuddyRemoveDelegate(IRCConnection connection, BuddyListItem buddy);
    public delegate void MonitorListDelegate(IRCConnection connection, string buddy, bool online, string timeStamp);

    //for the IAL (internal address list)
    public delegate void IALUserDataDelegate(IRCConnection connection, string nick, string host, string channel);
    public delegate void IALUserDataAwayOnlyDelegate(IRCConnection connection, string nick, bool away, string awayMessage);
    public delegate void IALUserChangeDelegate(IRCConnection connection, string oldnick, string newnick);
    public delegate void IALUserPartDelegate(IRCConnection connection, string nick, string channel);
    public delegate void IALUserQuitDelegate(IRCConnection connection, string nick);
    public delegate void IALUserAccountDelegate(IRCConnection connection, string nick, string account);

    public delegate void AutoPerformDelegate(IRCConnection connection, string[] commands);
    public delegate void AutoJoinDelegate(IRCConnection connection, string[] channels);
    public delegate void AutoRejoinDelegate(IRCConnection connection);

    public delegate void EndofNamesDelegate(IRCConnection connection, string channel);
    public delegate void EndofWhoReplyDelegate(IRCConnection connection, string channel);
    public delegate void WhoReplyDelegate(IRCConnection connection, string channel, string nick, string host, string flags, string message);
    public delegate void ChannelUserListDelegate(IRCConnection connection, string channel, string[] nicks, string message);

    public delegate void StatusTextDelegate(IRCConnection connection, string statusText);

    public delegate bool UserInfoWindowExistsDelegate(IRCConnection connection, string nick);

    public delegate void UserInfoHostFullnameDelegate(IRCConnection connection, string nick, string host, string full);
    public delegate void UserInfoIdleLogonDelegate(IRCConnection connection, string nick, string idleTime, string logonTime);
    public delegate void UserInfoAddChannelsDelegate(IRCConnection connection, string nick, string[] channels);
    public delegate void UserInfoAwayStatusDelegate(IRCConnection connection, string nick, string awayMessage);
    public delegate void UserInfoServerDelegate(IRCConnection connection, string nick, string server);
    public delegate void UserInfoLoggedInDelegate(IRCConnection connection, string nick, string name);

    public delegate bool ChannelInfoWindowExistsDelegate(IRCConnection connection, string channel);
    public delegate void ChannelInfoAddBanDelegate(IRCConnection connection, string channel, string host, string bannedBy);
    public delegate void ChannelInfoAddExceptionDelegate(IRCConnection connection, string channel, string host, string bannedBy);
    public delegate void ChannelInfoAddQuietDelegate(IRCConnection connection, string channel, string host, string bannedBy);
    public delegate void ChannelInfoTopicSetDelegate(IRCConnection connection, string channel, string nick, string time);

    public delegate void RefreshServerTreeDelegate(IRCConnection connection);
    public delegate void ServerReconnectDelegate(IRCConnection connection);
    public delegate void ServerDisconnectDelegate(IRCConnection connection);
    public delegate void ServerConnectDelegate(IRCConnection connection, string address);
    public delegate void ServerForceDisconnectDelegate(IRCConnection connection);
    public delegate void ServerPreConnectDelegate(IRCConnection connection);

    public delegate void WriteErrorFileDelegate(IRCConnection connection, string method, Exception e);

}

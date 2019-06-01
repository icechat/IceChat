/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2018 Paul Vanderzee <snerf@icechat.net>
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
    public class IrcTimer : System.Timers.Timer
    {
        private string timerID;
        private int timerRepetitions;
        private int timerCounter;
        private string timerCommand;

        public delegate void TimerElapsed(string timerID, string command);
        public event TimerElapsed OnTimerElapsed;

        public IrcTimer(string ID, int repetitions, double interval, string command)
        {
            this.timerID = ID;
            this.timerCommand = command;
            this.timerRepetitions = repetitions;
            this.Interval = interval;
            this.Elapsed += new System.Timers.ElapsedEventHandler(IrcTimer_Elapsed);

            timerCounter = 0;
        }

        private void IrcTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (OnTimerElapsed != null)
                OnTimerElapsed(this.timerID, this.timerCommand);

            timerCounter++;
            
            if (timerCounter == timerRepetitions)
            {
                //timer has expired, dispose of it
                this.DisableTimer();
            }
        }

        public void DisableTimer()
        {
            this.Stop();
            base.Dispose();
        }

        public int TimerCounter
        {
            get
            {
                return this.timerCounter;
            }
        }

        public double TimerInterval
        {
            get
            {
                return (this.Interval / 1000);
            }
        }

        public int TimerRepetitions
        {
            get
            {
                return this.timerRepetitions;
            }
        }

        public string TimerCommand
        {
            get
            {
                return this.timerCommand;
            }
        }

        public string TimerID
        {
            get
            {
                return this.timerID;
            }
        }
    }

}

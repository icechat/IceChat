/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2014 Paul Vanderzee <snerf@icechat.net>
 * <www.icechat.net>
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
using System.Text;
using System.Runtime.InteropServices;

// http://www.codeproject.com/Articles/14709/Playing-MP3s-using-MCI

namespace IceChat
{

    public class MP3Player
    {
        private string Pcommand, FName;
        private bool Opened, Playing, Paused, Loop,
                     MutedAll, MutedLeft, MutedRight;
        private int rVolume, lVolume, aVolume,
                    tVolume, bVolume;
        private ulong Lng;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand,
                StringBuilder strReturn, int iReturnLength,
                IntPtr hwndCallback);

        public MP3Player()
        {
            Opened = false;
            Pcommand = "";
            FName = "";
            Playing = false;
            Paused = false;
            Loop = false;
            MutedAll = MutedLeft = MutedRight = false;
            rVolume = lVolume = aVolume =
                      tVolume = bVolume = 1000;
            Lng = 0;
        }

        public bool MuteAll
        {
            get
            {
                return MutedAll;
            }
            set
            {
                MutedAll = value;
                if (MutedAll)
                {
                    Pcommand = "setaudio MediaFile off";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    Pcommand = "setaudio MediaFile on";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }

        }

        public bool MuteLeft
        {
            get
            {
                return MutedLeft;
            }
            set
            {
                MutedLeft = value;
                if (MutedLeft)
                {
                    Pcommand = "setaudio MediaFile left off";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    Pcommand = "setaudio MediaFile left on";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }

        }

        public bool MuteRight
        {
            get
            {
                return MutedRight;
            }
            set
            {
                MutedRight = value;
                if (MutedRight)
                {
                    Pcommand = "setaudio MediaFile right off";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    Pcommand = "setaudio MediaFile right on";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }

        }

        public int VolumeAll
        {
            get
            {
                return aVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    aVolume = value;
                    Pcommand = String.Format("setaudio MediaFile volume to {0}", aVolume);
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }

        public int VolumeLeft
        {
            get
            {
                return lVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    lVolume = value;
                    Pcommand = String.Format("setaudio MediaFile left volume to {0}", lVolume);
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }

        public int VolumeRight
        {
            get
            {
                return rVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    rVolume = value;
                    Pcommand = String.Format("setaudio MediaFile right volume to {0}", rVolume);
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }

        public int VolumeTreble
        {
            get
            {
                return tVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    tVolume = value;
                    Pcommand = String.Format("setaudio MediaFile treble to {0}", tVolume);
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }

        public int VolumeBass
        {
            get
            {
                return bVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    bVolume = value;
                    Pcommand = String.Format("setaudio MediaFile bass to {0}", bVolume);
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }


        public string FileName
        {
            get
            {
                return FName;
            }
        }

        public bool Looping
        {
            get
            {
                return Loop;
            }
            set
            {
                Loop = value;
            }
        }

        public void Seek(ulong Millisecs)
        {
            if (Opened && Millisecs <= Lng)
            {
                if (Playing)
                {
                    if (Paused)
                    {
                        Pcommand = String.Format("seek MediaFile to {0}", Millisecs);
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                    }
                    else
                    {
                        Pcommand = String.Format("seek MediaFile to {0}", Millisecs);
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                        Pcommand = "play MediaFile";
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                    }
                }
            }
        }

        private void CalculateLength()
        {
            StringBuilder str = new StringBuilder(128);
            mciSendString("status MediaFile length", str, 128, IntPtr.Zero);
            if (str.Length == 0)
                Lng = 0;
            else
                Lng = Convert.ToUInt64(str.ToString());

        }

        public ulong AudioLength
        {
            get
            {
                if (Opened) return Lng;
                else return 0;
            }
        }

        public void Close()
        {
            if (Opened)
            {
                Pcommand = "close MediaFile";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
                Opened = false;
                Playing = false;
                Paused = false;
            }
        }

        public void Open(string sFileName)
        {
            if (!Opened)
            {
                //get the proper type
                string ext = sFileName.Substring(sFileName.LastIndexOf(".") + 1).ToLower();
                string filetype = "mpegvideo";
                if (ext == "wav")
                    filetype = "waveaudio";

                //mid == sequencer
                //avi == avivideo
                //cdaudio == cdaudio

                Pcommand = "open \"" + sFileName + "\" type " + filetype + " alias MediaFile";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
                FName = sFileName;
                Opened = true;
                Playing = false;
                Paused = false;
                Pcommand = "set MediaFile time format milliseconds";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
                Pcommand = "set MediaFile seek exactly on";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
                CalculateLength();
            }
            else
            {
                this.Close();
                this.Open(sFileName);
            }
        }

        public void Play()
        {
            if (Opened)
            {
                if (!Playing)
                {
                    Playing = true;
                    Pcommand = "play MediaFile";
                    if (Loop) Pcommand += " REPEAT";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    if (!Paused)
                    {
                        Pcommand = "seek MediaFile to start";
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                        Pcommand = "play MediaFile";
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                    }
                    else
                    {
                        Paused = false;
                        Pcommand = "play MediaFile";
                        mciSendString(Pcommand, null, 0, IntPtr.Zero);
                    }
                }
            }
        }

        public void Pause()
        {
            if (Opened)
            {
                if (!Paused)
                {
                    Paused = true;
                    Pcommand = "pause MediaFile";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
                else
                {
                    Paused = false;
                    Pcommand = "play MediaFile";
                    mciSendString(Pcommand, null, 0, IntPtr.Zero);
                }
            }
        }

        public void Stop()
        {
            if (Opened && Playing)
            {
                Playing = false;
                Paused = false;
                Pcommand = "seek MediaFile to start";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
                Pcommand = "stop MediaFile";
                mciSendString(Pcommand, null, 0, IntPtr.Zero);
            }
        }

        public ulong CurrentPosition
        {
            get
            {
                if (Opened && Playing)
                {
                    StringBuilder s = new StringBuilder(128);
                    Pcommand = "status MediaFile position";
                    mciSendString(Pcommand, s, 128, IntPtr.Zero);
                    return Convert.ToUInt64(s.ToString());
                }
                else return 0;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace B20_Ex02_1
{
    internal class Player
    {
        private string m_PlayerName;
        private int m_Score;

        internal Player(string i_PlayerName, int i_Score)
        {
            this.m_PlayerName = i_PlayerName;
            this.m_Score = i_Score;
        }

        internal string PlayerName
        {
            get { return m_PlayerName; }
        }

        internal int Score
        {
            get { return this.m_Score; }

            set { this.m_Score = value; }
        }
    }
}

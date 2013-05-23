using System;
using System.Drawing;

namespace MSR.LST.ConferenceXP
{
    public class UIFont
    {
        private static float m_Size = SystemFonts.MessageBoxFont.Size;
        private static string m_FontName = SystemFonts.MessageBoxFont.Name;

        private static Font m_FormFont = new Font("Microsoft Sans Serif", m_Size);
        private static Font m_StatusFont = new Font(m_FontName, m_Size);
        private static Font m_StringFont = new Font(m_FontName, m_Size);
        private static Font m_BoldStringFont = new Font(m_FontName, m_Size, FontStyle.Bold);

        public static void SetUIFont(string name, float size) {
            m_FontName = name;
            m_Size = size;

            m_FormFont = new Font("Microsoft Sans Serif", size);
            m_StatusFont = new Font(name, size);
            m_StringFont = new Font(name, size);
            m_BoldStringFont = new Font(name, size, FontStyle.Bold);
        }

        public static string Name {
            get { return m_FontName; }
        }
        
        public static float Size {
            get { return m_Size; }
        }

        public static Font FormFont {
            get { return m_FormFont; }
        }

        public static Font StatusFont
        {
            get { return m_StatusFont; }
        }

        public static Font StringFont
        {
            get { return m_StringFont; }
        }

        public static Font BoldStringFont
        {
            get { return m_BoldStringFont; }
        }
    }
}

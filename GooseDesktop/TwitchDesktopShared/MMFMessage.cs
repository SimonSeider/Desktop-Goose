using System;
using System.Text;

namespace TwitchDesktopShared
{
    [Serializable]
    public class MMFMessage
    {
        public MessageType type;

        public string args;


        public MMFMessage(MessageType t, string a)
        {
            this.type = t;
            this.args = a;
        }

        public MMFMessage(MessageType t, string[] args)
        {
            this.type = t;
            StringBuilder stringBuilder = new StringBuilder(4095);
            for (int i = 0; i < args.Length; i++)
            {
                stringBuilder.Append(args[i] + " ");
            }
        }

        public MMFMessage(string s)
        {
            int num = s.IndexOf(" ");
            int num2 = -1;
            int.TryParse(s.Substring(0, num), out num2);
            this.type = (MessageType)num2;
            this.args = s.Substring(num);
        }

        public MMFMessage(byte[] bytes)
            : this(Encoding.UTF8.GetString(bytes))
        {
        }

        public byte[] GetBytes()
        {
            Encoding utf = Encoding.UTF8;
            int num = (int)this.type;
            return utf.GetBytes(num.ToString() + " " + this.args);
        }

        public enum MessageType
        {
            AttachToTwitch,
            StartTask
        }
    }
}

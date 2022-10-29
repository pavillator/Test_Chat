using System.Runtime.Serialization;

namespace ChatDemo
{
    [DataContract]
    internal class Message
    {
        [DataMember]
        string name;
        [DataMember]
        string message;

        public Message()
        {
            name = "";
            message = "";
        }

        public string getName()
        {
            return name;
        }

        public void setName(string mName)
        {
            name = mName;
        }

        public string getMessage()
        {
            return message;
        }

        public void setMessage(string mMessage)
        {
            message = mMessage;
        }
    }
}

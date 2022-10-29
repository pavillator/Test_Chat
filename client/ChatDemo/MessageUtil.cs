using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChatDemo
{
    class MessageUtil
    {
        public static string WriteFromObject(Message msg)
        {
            string result = JsonConvert.SerializeObject(msg);
            return result;
        }

        public static Message ReadToObject(string json)
        {
            return JsonConvert.DeserializeObject<Message>(json);
        }
    }
}

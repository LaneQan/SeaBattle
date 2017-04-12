using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace SeaBattle
{

    class SeaBattle
    {
        public int[,] myField = new int[10,10];
        public int[,] opponentField = new int[10, 10];
        public void generateMyShips()
        {
            List<int> ships = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            while (ships.Count!=0)
            {

            }
        }
    }
    class VkAPI
    {
        VkApi vk = new VkApi();
        public void Auth()
        {
            Settings scope = Settings.All;
            List<string> info = new List<string>();
            using (StreamReader sr = new StreamReader("info.config"))
            {
                String line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    info.Add(line);
                }
            }

            vk.Authorize(new ApiAuthParams
            {
                ApplicationId = Convert.ToUInt64(info[0]),
                Login = info[1],
                Password = info[2],
                Settings = scope,
            });          
        }
        public void printMessages()
        {
            var get = vk.Messages.Get(new MessagesGetParams
            {
                Out = VkNet.Enums.MessageType.Received,
                Count = 20

            });
            //Console.WriteLine(get.Messages[0].Body);
        }
    }
    class Program
    {  

        static void Main(string[] args)
        {
            VkAPI vk = new VkAPI();
            vk.Auth();
            vk.printMessages();
            Console.ReadLine();
        }
    }
}

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
        VkApi vk = new VkApi();
        Random r = new Random();
        private int[,] myField = new int[10, 10];
        private int[,] opponentField = new int[10, 10];
        private int[,] shootedField = new int[10, 10];

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
        public void getMessages()
        {
            var get = vk.Messages.Get(new MessagesGetParams
            {
                Out = VkNet.Enums.MessageType.Received,
                Count = 6

            });
            foreach (var k in get.Messages)
            {
                if (k.Body.Contains("Стреляю"))
                    Console.WriteLine(k.Body[8]+""+k.Body[9] + " and owner ID - " + k.UserId);
            }
        }
        private bool checkPoint(int[,] arr, int x, int y)
        {
            bool success = true;
            if (arr[x, y] == 1)
                success = false;
            if (x != 0 && arr[x - 1, y] == 1)
                success = false;
            if (x != 0 && y != 0 && arr[x - 1, y - 1] == 1)
                success = false;
            if (y!=0 && arr[x,y-1]==1)
                success = false;
            if (x!=9 && arr[x+1,y]==1)
                success = false;
            if (x!=9 && y!=9 && arr[x+1,y+1]==1)
                success = false;
            if (y!=9 && arr[x,y+1]==1)
                success = false;
            if (x!=0 && y!=9 && arr[x-1,y+1]==1)
                success = false;
            if (x!=9 && y!=0 && arr[x+1,y-1]==1)
                success = false;
            return success;

        }
        public void generateMyShips()
        {
            List<int> ships = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            while (ships.Count!=0)
            {
           rerandom:
                int x = r.Next(9); // генерация начальной точки корабля
                int y = r.Next(9); // генерация начальной точки корабля
                int d = r.Next(1, 3); // 1 - горизонтально, 2 - вертикально
                if (myField[x, y] == 1) goto rerandom; // проверка начальной точки

                if (d==1) 
                {
                    if ((x + ships[0] - 1) < 10)
                    {
                        bool success = true;
                        for (int i=x;i<= (x + ships[0] - 1);i++)
                            if (!checkPoint(myField, i, y))
                            {
                                success = false;
                            }
                        if (!success)
                            goto rerandom;
                        else
                        {
                            for (int i = x; i <= (x + ships[0] - 1); i++)
                                myField[i, y] = 1;
                        }

                    }
                    else goto rerandom;
                }
                else
                {
                    if ((y + ships[0] - 1) < 10)
                    {
                        bool success = true;
                        for (int i = y; i <= (y + ships[0] - 1); i++)
                            if (!checkPoint(myField, x, i))
                            {
                                success = false;
                            }
                        if (!success)
                            goto rerandom;
                        else
                        {
                            for (int i = y; i <= (y + ships[0] - 1); i++)
                                myField[x, i] = 1;
                        }

                    }
                    else goto rerandom;
                }
                ships.RemoveAt(0);
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                    Console.Write(myField[i, j]+"   ");
                Console.WriteLine();
            }
            }
    }
    class Program
    {  

        static void Main(string[] args)
        {
            SeaBattle sb = new SeaBattle();
            sb.Auth(); // авторизация VK
            sb.generateMyShips();
            sb.getMessages();
            Console.ReadLine();
        }
    }
}

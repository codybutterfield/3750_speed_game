using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            if (UserHandler.ConnectedIds.Count == 2)
            {
                Response.Redirect("https://localhost:44385//game.html");
            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GameMessages(string user, string message)
        {
            if (UserHandler.ConnectedIds.Count == 2)
            {
                int count = 0;
                string p1 = "";
                string p2 = "";
                foreach (string a in UserHandler.ConnectedIds)
                {
                    if (count == 0)
                    {
                        p1 = a;
                        count++;
                    }
                    else
                    {
                        p2 = a;
                    }
                }
                Console.WriteLine("p1: " + p1);
                Console.WriteLine("p2: " + p2);
                Console.WriteLine("ready to play!");

                List<int> listInts = new List<int>();
                listInts.Add(1);
                listInts.Add(2);
                listInts.Add(3);

                Deck deck = new Deck();
                deck.FillDeck();
                deck.Shuffle();
                deck.PrintDeck();
                //Create hand
                PlayerStack playerStack1 = new PlayerStack();
                playerStack1.CreatePlayerStack(deck);
                PlayerStack playerStack2 = new PlayerStack();
                playerStack2.CreatePlayerStack(deck);
                //Create Draw Piles
                DrawStack drawStack1 = new DrawStack();
                drawStack1.CreateDrawStack(deck);
                DrawStack drawStack2 = new DrawStack();
                drawStack2.CreateDrawStack(deck);
                //Create Stacks to play on
                PlayStack playStack1 = new PlayStack();
                playStack1.CreatePlayStack(deck);
                PlayStack playstack2 = new PlayStack();
                playstack2.CreatePlayStack(deck);
                //Create Stacks of extras
                ExtraStack extraStack1 = new ExtraStack();
                extraStack1.CreateExtraStack(deck);
                ExtraStack extraStack2 = new ExtraStack();
                extraStack2.CreateExtraStack(deck);


                await Clients.Client(p1).SendAsync("DisplayHand", playerStack1.ReturnHand());
                await Clients.Client(p2).SendAsync("DisplayHand", playerStack2.ReturnHand());


            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
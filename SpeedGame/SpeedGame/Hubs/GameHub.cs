using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SignalRChat.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
    public class GameHub : Hub
    {
        public async Task InitiateGame()
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

                Deck deck = new Deck();
                PlayerStack playerStack1 = new PlayerStack();
                PlayerStack playerStack2 = new PlayerStack();
                DrawStack drawStack1 = new DrawStack();
                DrawStack drawStack2 = new DrawStack();
                PlayStack playStack1 = new PlayStack();
                PlayStack playStack2 = new PlayStack();
                ExtraStack extraStack1 = new ExtraStack();
                ExtraStack extraStack2 = new ExtraStack();

                deck.FillDeck();
                deck.Shuffle();
                //deck.PrintDeck();
                //Create hand
                
                playerStack1.CreatePlayerStack(deck);
                
                playerStack2.CreatePlayerStack(deck);
                //Create Draw Piles
                
                drawStack1.CreateDrawStack(deck);
                
                drawStack2.CreateDrawStack(deck);
                //Create Stacks to play on
                
                playStack1.CreatePlayStack(deck);
                
                playStack2.CreatePlayStack(deck);
                //Create Stacks of extras
                
                extraStack1.CreateExtraStack(deck);
                
                extraStack2.CreateExtraStack(deck);


                string p1Hand = JsonConvert.SerializeObject(playerStack1.getHand());
                string p2Hand = JsonConvert.SerializeObject(playerStack2.getHand());
                string ps1 = JsonConvert.SerializeObject(playStack1.ShowTop());
                string ps2 = JsonConvert.SerializeObject(playStack2.ShowTop());
                string ds1 = JsonConvert.SerializeObject(drawStack1.getDraw());
                string ds2 = JsonConvert.SerializeObject(drawStack2.getDraw());

                await Clients.Client(p1).SendAsync("UpdateGame", p1Hand, playerStack2.getHand().Count, ds1, drawStack2.getDraw().Count, ps1, ps2, extraStack1.isEmpty(), extraStack2.isEmpty());
                await Clients.Client(p2).SendAsync("UpdateGame", p2Hand, playerStack1.getHand().Count, ds2, drawStack1.getDraw().Count, ps1, ps2, extraStack1.isEmpty(), extraStack2.isEmpty());
            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task selectCard(int index)
        {
            Console.WriteLine("1: " + playerStack1.getHand().Count);

            Card cardChoice = playerStack1.CardChoice(index);
            Console.WriteLine("2:" + cardChoice.Value);
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
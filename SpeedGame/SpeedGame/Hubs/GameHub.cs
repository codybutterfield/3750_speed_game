using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
/*using Newtonsoft.Json;*/

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
                deck.FillDeck();
                deck.Shuffle();
                PlayerStack playerStack1 = new PlayerStack();
                PlayerStack playerStack2 = new PlayerStack();
                ExtraStack extraStack1 = new ExtraStack();
                ExtraStack extraStack2 = new ExtraStack();
                PlayStack playStack1 = new PlayStack();
                PlayStack playStack2 = new PlayStack();
                DrawStack drawStack1 = new DrawStack();
                DrawStack drawStack2 = new DrawStack();
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

                string p1Hand = JsonSerializer.Serialize(playerStack1.getHand());
                string p2Hand = JsonSerializer.Serialize(playerStack2.getHand());
                string ps1 = JsonSerializer.Serialize(playStack1.getPlayStack());
                string ps2 = JsonSerializer.Serialize(playStack2.getPlayStack());
                string ds1 = JsonSerializer.Serialize(drawStack1.getDraw());
                string ds2 = JsonSerializer.Serialize(drawStack2.getDraw());
                string es1 = JsonSerializer.Serialize(extraStack1.getExtraStack());
                string es2 = JsonSerializer.Serialize(extraStack2.getExtraStack());

                await Clients.Client(p1).SendAsync("UpdateGame", p1Hand, playerStack2.getHand().Count, ds1, drawStack2.getDraw().Count, ps1, ps2, es1, es2);
                await Clients.Client(p2).SendAsync("UpdateGame", p2Hand, playerStack1.getHand().Count, ds2, drawStack1.getDraw().Count, ps1, ps2, es1, es2);
            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task compareCard(string c, string h)
        {
            Card card = JsonSerializer.Deserialize<Card>(c);
            Card hand = JsonSerializer.Deserialize<Card>(h);

            if (hand.Value == 14)
            {
                if (card.Value == 13 || card.Value == 2)
                {
                    Console.WriteLine("Top");
                }
            }
            else if (card.Value == 14)
            {
                if (hand.Value == 13 || hand.Value == 2)
                {
                    Console.WriteLine("Middle");
                }
            }
            else if (hand.Value == card.Value + 1 || hand.Value == card.Value - 1)
            {
                Console.WriteLine("Bottom");
            }
            else
            {
                Console.WriteLine("Very Bottom");
                return;
            }
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
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
                string psTop1 = JsonSerializer.Serialize(playStack1.ShowTop());
                string psTop2 = JsonSerializer.Serialize(playStack2.ShowTop());

                await Clients.Client(p1).SendAsync("CreateGame", p1Hand, playerStack2.getHand().Count, ds1, drawStack2.getDraw().Count, ps1, ps2, es1, es2, psTop1, psTop2);
                await Clients.Client(p2).SendAsync("CreateGame", p2Hand, playerStack1.getHand().Count, ds2, drawStack1.getDraw().Count, ps1, ps2, es1, es2, psTop1, psTop2);
            }
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task compareCard(string PlayStack1JS, string CardFromHand, string Hand, int OpponentHandCountJS, string PlayerDrawStackJS, int OpponentDrawStackCountJS, string PlayStack1JSTop, string exStack1, string exStack2, int stackNum)
        {
            Card card = JsonSerializer.Deserialize<Card>(PlayStack1JSTop);
            Card cardFromHand = JsonSerializer.Deserialize<Card>(CardFromHand);

            //Make Stacks
            List<Card> hand = JsonSerializer.Deserialize<List<Card>>(Hand);
            List<Card> PlayerDrawStack = JsonSerializer.Deserialize<List<Card>>(PlayerDrawStackJS);
            List<Card> PlayStack1 = JsonSerializer.Deserialize<List<Card>>(PlayStack1JS);
            //List<Card> PlayStack2 = JsonSerializer.Deserialize<List<Card>>(PlayStack2JS);
            List<Card> ExStack1 = JsonSerializer.Deserialize<List<Card>>(exStack1);
            List<Card> ExStack2 = JsonSerializer.Deserialize<List<Card>>(exStack2);

            Stack<Card> playerDrawStackStack = new Stack<Card>();
            Stack<Card> PlayStack1Stack = new Stack<Card>();
            /*Stack<Card> PlayStack2Stack = new Stack<Card>();*/
            Stack<Card> ExStack1Stack = new Stack<Card>();
            Stack<Card> ExStack2Stack = new Stack<Card>();

            PlayerDrawStack.Reverse();
            foreach (Card c in PlayerDrawStack)
            {
                playerDrawStackStack.Push(c);
            }

            PlayStack1.Reverse();
            foreach (Card c in PlayStack1)
            {
                PlayStack1Stack.Push(c);
            }

           /* PlayStack2.Reverse();
            foreach (Card c in PlayStack2)
            {
                PlayStack2Stack.Push(c);
            }*/

            ExStack1.Reverse();
            foreach (Card c in ExStack1)
            {
                ExStack1Stack.Push(c);
            }

            ExStack2.Reverse();
            foreach (Card c in ExStack2)
            {
                ExStack2Stack.Push(c);
            }

            int pos = 0;
            if (cardFromHand.Value == 14)
            {
                if (card.Value == 13 || card.Value == 2)
                {
                    Console.WriteLine("Top");
                    PlayStack1Stack.Push(cardFromHand);
                    for (int i = 0; i < hand.Count; i++)
                    {
                        if (hand[i].Name.CompareTo(cardFromHand.Name) == 0)
                        {
                            pos = i;
                            hand.RemoveAt(i);
                        }
                    }
                    if (playerDrawStackStack.Count != 0)
                    {
                        hand.Insert(pos, playerDrawStackStack.Pop());
                    } 
                    else if (hand.Count == 0)
                    {
                        Console.WriteLine("Poggers, you win twerp");
                    }
                }
            }
            else if (card.Value == 14)
            {
                if (cardFromHand.Value == 13 || cardFromHand.Value == 2)
                {
                    Console.WriteLine("Top");
                    PlayStack1Stack.Push(cardFromHand);
                    for (int i = 0; i < hand.Count; i++)
                    {
                        if (hand[i].Name.CompareTo(cardFromHand.Name) == 0)
                        {
                            pos = i;
                            hand.RemoveAt(i);
                        }
                    }
                    if (playerDrawStackStack.Count != 0)
                    {
                        hand.Insert(pos, playerDrawStackStack.Pop());
                    }
                    else if (hand.Count == 0)
                    {
                        Console.WriteLine("Poggers, you win twerp");
                    }
                }
            }
            else if (cardFromHand.Value == card.Value + 1 || cardFromHand.Value == card.Value - 1)
            {
                Console.WriteLine("Top");
                PlayStack1Stack.Push(cardFromHand);
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i].Name.CompareTo(cardFromHand.Name) == 0)
                    {
                        pos = i;
                        hand.RemoveAt(i);
                        break;
                    }
                }
                if (playerDrawStackStack.Count != 0)
                {
                    hand.Insert(pos, playerDrawStackStack.Pop());
                }
                else if (hand.Count == 0)
                {
                    Console.WriteLine("Poggers, you win twerp");
                }
            }
            else
            {
                Console.WriteLine("Very Bottom");
                return;
            }

            string p1 = String.Empty; 
            string p2 = String.Empty;
            List<string> cIds = UserHandler.ConnectedIds.ToList<string>();
            if (cIds[0] == Context.ConnectionId)
            {
                p1 = cIds[0];
                p2 = cIds[1];
            } 
            else
            {
                p1 = cIds[1];
                p2 = cIds[0];
            }

            string handStr = JsonSerializer.Serialize(hand);
            string playerDrawStackStr = JsonSerializer.Serialize(playerDrawStackStack);
            string playStack1Str = JsonSerializer.Serialize(PlayStack1Stack);
            string exStack1Str = JsonSerializer.Serialize(ExStack1Stack);
            string exStack2Str = JsonSerializer.Serialize(ExStack2Stack);
            string playStack1Top = JsonSerializer.Serialize(PlayStack1Stack.Peek());
           
            await Clients.Client(p1).SendAsync("UpdateGame", handStr, playerDrawStackStr, playStack1Str, exStack1Str, exStack2Str, playStack1Top, stackNum);
            await Clients.Client(p2).SendAsync("UpdateGameOpp", playerDrawStackStack.Count, playStack1Top, stackNum);
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
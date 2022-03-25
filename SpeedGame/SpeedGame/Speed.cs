using System;
using System.Collections.Generic;
using System.Linq;
public class Card
{
    public enum Suites
    {
        Hearts = 0,
        Diamonds,
        Clubs,
        Spades
    }

    public int Value
    {
        get;
        set;
    }

    public Suites Suite
    {
        get;
        set;
    }

    //Used to get full name, also useful 
    //if you want to just get the named value
    public string NamedValue
    {
        get
        {
            string name = string.Empty;
            switch (Value)
            {
                case (14):
                    name = "Ace";
                    break;
                case (13):
                    name = "King";
                    break;
                case (12):
                    name = "Queen";
                    break;
                case (11):
                    name = "Jack";
                    break;
                default:
                    name = Value.ToString();
                    break;
            }

            return name;
        }
    }

    public string Name
    {
        get
        {
            return NamedValue + " of  " + Suite.ToString();
        }
    }

    public Card(int Value, Suites Suite)
    {
        this.Value = Value;
        this.Suite = Suite;
    }
}

public class Deck
{
    public List<Card> Cards = new List<Card>();
    public void FillDeck()
    {
        for (int i = 0; i < 52; i++)
        {
            Card.Suites suite = (Card.Suites)(Math.Floor((decimal)i / 13));
            int val = i % 13 + 2;
            Cards.Add(new Card(val, suite));
        }

    }

    public void Shuffle()
    {
        var rnd = new Random();
        var randomized = Cards.OrderBy(item => rnd.Next());
        List<Card> Cards2 = new List<Card>();
        //this.Cards.Clear();
        foreach (Card card in randomized)
        {
            Cards2.Add(card);
        }
        Cards.Clear();
        foreach (Card card in Cards2)
        {
            Cards.Add(card);
        }

    }
    public void PrintDeck()
    {
        foreach (Card card in this.Cards)
        {
            Console.WriteLine(card.Name);
        }
    }
}

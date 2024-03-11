/* Создать модель карточной игры. 
  Требования:
    1. Класс Game формирует и обеспечивает:
        1.1.1.Список игроков(минимум 2);
        1.1.2.Колоду карт(36 карт);
        1.1.3.Перетасовку карт(случайным образом);
        1.1.4.Раздачу карт игрокам(равными частями каждому игроку);
        1.1.5.Игровой процесс.Принцип: Игроки кладут по одной карте. У кого карта больше,
              то тот игрок забирает все карты и кладет их в конец своей колоды.
              Упрощение: при совпадении карт забирает первый игрок, шестерка не забирает туза.
              Выигрывает игрок, который забрал все карты.
	  2. Класс Player (набор имеющихся карт, вывод имеющихся карт).
	  3. Класс Karta (масть и тип карты (6–10, валет, дама, король, туз).*/

using static System.Console;

public class Game
{
    public List<Card> cardDeck;
    public List<Player> players;

    private Random _random;
    private int _cardsAmount = 36;
    public Game(int playersCount = 2)
    {
        _random = new Random();

        players = new List<Player>();
        for (int i = 0; i < playersCount; i++)
        {
            players.Add(new Player());
        }

        cardDeck = createCardDeck();
        shuffleCards(cardDeck);

        dealCardsToPlayers(players, cardDeck);
    }

    public List<Card> createCardDeck()
    {
        cardDeck = new List<Card>();
        int suitCount = _cardsAmount / 4;

        for (int i = 0; i < suitCount; i++)
        {
            cardDeck.Add(new Card((CardValue)i, (CardSuit)0));
            cardDeck.Add(new Card((CardValue)i, (CardSuit)1));
            cardDeck.Add(new Card((CardValue)i, (CardSuit)2));
            cardDeck.Add(new Card((CardValue)i, (CardSuit)3));
        }

        return cardDeck;
    }

    public void shuffleCards(List<Card> cards)
    {
        cards.Sort((a, b) => _random.Next(-2, 2));
    }

    public void dealCardsToPlayers(List<Player> players, List<Card> cards)
    {
        int currentPlayer = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            players[currentPlayer].cards.Add(cards[i]);

            currentPlayer++;
            currentPlayer %= players.Count;
        }
    }

    public bool playersTurn()
    {
        WriteLine("Ход игроков:");
        WriteLine("игрок\tкол-во карт\tход картой");

        int maxValue = -1;
        Player playerWithMaxValue = null;
        Stack<Card> cardStack = new Stack<Card>();

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];

            if (player.cards.Count > 0)
            {
                Card card = player.cards[_random.Next(player.cards.Count)];

                WriteLine($"{i}\t{player.cards.Count}\t\t{card}");
                player.cards.Remove(card);

                if ((int)card.value > maxValue)
                {
                    maxValue = (int)card.value;
                    playerWithMaxValue = player;
                }

                cardStack.Push(card);

            }
        }

        playerWithMaxValue.cards.AddRange(cardStack);
        WriteLine($"Забрал игрок {players.IndexOf(playerWithMaxValue)}.");
        WriteLine("------------------------------------------------");

        if (playerWithMaxValue.cards.Count == _cardsAmount)
        {
            WriteLine($"Победил игрок номер {players.IndexOf(playerWithMaxValue)}");
            return false;
        }

        return true;
    }
}

public class Player
{
    public List<Card> cards = new List<Card>();
}

public enum CardValue
{
    Шестёрка = 0, Семёрка, Восьмёрка, Девятка, Десятка, Валет, Дама, Король, Туз
}

public enum CardSuit
{
    ЧЕРВЫ = 0, БУБЫ, ТРЕФЫ, ПИКИ
}
public class Card
{

    public readonly CardValue value;
    public readonly CardSuit suit;

    public Card(CardValue value, CardSuit suit)
    {
        this.value = value;
        this.suit = suit;
    }

    public override string ToString()
    {
        return $"{value} {suit}";
    }
}
class Program
{
    static void Main()
    {
        Game game = new Game(4);
        while (game.playersTurn()) { }
    }
}

using CasinoGame.Profiles;
using CasinoGame.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CasinoGame.Games
{
    public class BlackjackGame : CasinoGameBase
    {
        private readonly PlayerProfile _playerProfile;
        private readonly int _numCards;
        private Queue<Card> _deck;
        private List<Card> _playerHand;
        private List<Card> _computerHand;

        public BlackjackGame(PlayerProfile playerProfile, int numCards)
        {
            if (numCards <= 0)
                throw new ArgumentException("The number of cards must be greater than zero.");

            _playerProfile = playerProfile;
            _numCards = numCards;

            FactoryMethod(); // Создание колоды карт
        }

        protected override void FactoryMethod()
        {
            _deck = new Queue<Card>();
            _playerHand = new List<Card>();
            _computerHand = new List<Card>();

            // Создание колоды карт
            var suits = Enum.GetValues(typeof(CardSuit)).Cast<CardSuit>();
            var values = Enum.GetValues(typeof(CardValue)).Cast<CardValue>();

            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    _deck.Enqueue(new Card(suit, value));
                }
            }

            Shuffle(); // Перетасовываем колоду
        }

        private void Shuffle()
        {
            var shuffledDeck = _deck.OrderBy(card => Guid.NewGuid()).ToList(); // Перетасовываем колоду случайным образом
            _deck = new Queue<Card>(shuffledDeck); // Обновляем очередь с картами
        }

        // Подсчёт очков в руке
        private int GetHandScore(List<Card> hand)
        {
            int score = 0;
            int aceCount = 0;

            foreach (var card in hand)
            {
                if (card.Value == CardValue.Ace)
                {
                    aceCount++;
                    score += 11; // Изначально считаем туз за 11 очков
                }
                else if (card.Value >= CardValue.Ten)
                {
                    score += 10; // Для карт 10, J, Q, K добавляем 10 очков
                }
                else
                {
                    score += (int)card.Value; // Для остальных карт добавляем их номинал
                }
            }

            // Если у нас есть тузы, которые считаются за 11 очков, но сумма очков больше 21, то меняем туз на 1
            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }

            return score;
        }

        // Выводим карты игрока и компьютера
        private void PrintHands()
        {
            Console.WriteLine("Your hand:");
            foreach (var card in _playerHand)
            {
                Console.WriteLine($"[{card.Value} of {card.Suit}]");
            }
            Console.WriteLine($"Your score: {GetHandScore(_playerHand)}\n");

            Console.WriteLine("Computer's hand:");
            foreach (var card in _computerHand)
            {
                Console.WriteLine($"[{card.Value} of {card.Suit}]");
            }
            Console.WriteLine($"Computer's score: {GetHandScore(_computerHand)}\n");
        }

        // Игра начинается
        public override void PlayGame()
        {
            // Раздаем по 2 карты игроку и компьютеру
            _playerHand.Add(_deck.Dequeue());
            _computerHand.Add(_deck.Dequeue());
            _playerHand.Add(_deck.Dequeue());
            _computerHand.Add(_deck.Dequeue());

            // Выводим карты игроков
            PrintHands();

            // Игрок принимает решение о том, брать ли карты
            while (true)
            {
                Console.WriteLine("Do you want to (1) hit or (2) stand?");
                string choice = Console.ReadLine();

                if (choice == "1") // Игрок берет карту
                {
                    _playerHand.Add(_deck.Dequeue());
                    PrintHands();

                    if (GetHandScore(_playerHand) > 21)
                    {
                        Console.WriteLine("You bust! You lose.");
                        OnLooseInvoke();
                        return;
                    }
                }
                else if (choice == "2") // Игрок остановился
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select (1) hit or (2) stand.");
                }
            }

            // Компьютер действует по своим правилам
            while (GetHandScore(_computerHand) < 17) // Компьютер берет карту, если у него меньше 17 очков
            {
                _computerHand.Add(_deck.Dequeue());
            }

            // Подсчитываем результат
            int playerScore = GetHandScore(_playerHand);
            int computerScore = GetHandScore(_computerHand);

            // Выводим итоговые карты
            PrintHands();

            // Определяем победителя
            if (playerScore > 21)
            {
                Console.WriteLine("You bust! You lose.");
                OnLooseInvoke();
            }
            else if (computerScore > 21)
            {
                Console.WriteLine("Computer busts! You win.");
                OnWinInvoke();
            }
            else if (playerScore > computerScore)
            {
                Console.WriteLine("You win!");
                OnWinInvoke();
            }
            else if (playerScore < computerScore)
            {
                Console.WriteLine("You lose.");
                OnLooseInvoke();
            }
            else
            {
                Console.WriteLine("It's a draw!");
                OnDrawInvoke();
            }

            // Проверка на деньги игрока
            if (_playerProfile.Money <= 0)
            {
                Console.WriteLine("No money? Kicked!");
            }
        }
    }
}

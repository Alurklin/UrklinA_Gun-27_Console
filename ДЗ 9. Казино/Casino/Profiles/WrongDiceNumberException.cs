using System;

public class WrongDiceNumberException : Exception
{
    public WrongDiceNumberException(int number, int min, int max)
        : base($"Invalid dice number {number}. Valid range: {min} to {max}.")
    {
    }
}
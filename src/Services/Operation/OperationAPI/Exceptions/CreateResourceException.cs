﻿namespace OperationAPI.Exceptions;

public class CreateResourceException : Exception
{
    public CreateResourceException(string? message) : base(message)
    {

    }
}
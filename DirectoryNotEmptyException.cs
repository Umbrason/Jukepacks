[Serializable]
internal class DirectoryNotEmptyException : Exception
{
    public DirectoryNotEmptyException()
    {
    }

    public DirectoryNotEmptyException(string? message) : base(message)
    {
    }

    public DirectoryNotEmptyException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}